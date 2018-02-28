using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Contacts.Filters;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Moq;
using NUnit.Framework;

namespace Tests.Contacts.Filters
{
    [TestFixture]
    public class SearchContactFilterTests
    {
        private IFixture fixture;
        private Mock<IHubSpotClient> mockClient;
        private Mock<IHubSpotContactClient> mockContactClient;

        [SetUp]
        public void Initialize()
        {
            fixture = new Fixture();
            fixture.Customize<ContactListItem>(cu => cu.OmitAutoProperties());

            mockContactClient = new Mock<IHubSpotContactClient>(MockBehavior.Strict);

            mockClient = new Mock<IHubSpotClient>(MockBehavior.Strict);
            mockClient.SetupGet(p => p.Contacts).Returns(mockContactClient.Object);
        }

        [Test, AutoData]
        public async Task Single_page_of_contacts_is_fetched(SearchContactFilter sut, IReadOnlyList<Property> properties)
        {
            var list = fixture.Build<SearchResponse>()
                              .With(p => p.HasMore, false)
                              .With(p => p.Offset, null)
                              .Create();

            mockContactClient.Setup(p => p.SearchAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<int>(), It.IsAny<long?>()))
                             .ReturnsAsync(list);

            var response = await sut.GetContacts(mockClient.Object, properties);

            CollectionAssert.AreEquivalent(response, list.Contacts);

            mockContactClient.Verify(p => p.SearchAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<int>(), null), Times.Once);
        }

        [Test, AutoData]
        public async Task Multiple_pages_of_contacts_are_fetched(SearchContactFilter sut, IReadOnlyList<Property> properties)
        {
            var listBuilder = fixture.Build<SearchResponse>();

            var lists = new[]
            {
                listBuilder.With(p => p.HasMore, true).With(p => p.Offset).Create(),
                listBuilder.With(p => p.HasMore, false).Without(p => p.Offset).Create()
            };

            mockContactClient.SetupSequence(p => p.SearchAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<int>(), It.IsAny<long?>()))
                             .ReturnsSequenceAsync(lists);

            var response = await sut.GetContacts(mockClient.Object, properties);

            CollectionAssert.AreEquivalent(lists.SelectMany(l => l.Contacts), response);

            mockContactClient.Verify(p => p.SearchAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<int>(), null), Times.Once);
            mockContactClient.Verify(p => p.SearchAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<int>(), lists.First().Offset), Times.Once);
        }
    }
}