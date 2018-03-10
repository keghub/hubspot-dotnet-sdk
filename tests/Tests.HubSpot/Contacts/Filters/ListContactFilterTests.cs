using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Contacts.Filters;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using HubSpot.Model.Lists;
using Moq;
using NUnit.Framework;

namespace Tests.Contacts.Filters {
    [TestFixture]
    public class ListContactFilterTests
    {
        private IFixture fixture;
        private Mock<IHubSpotClient> mockClient;
        private Mock<IHubSpotListClient> mockListClient;

        [SetUp]
        public void Initialize()
        {
            fixture = new Fixture();
            fixture.Customize<ContactListItem>(cu => cu.OmitAutoProperties());

            mockListClient = new Mock<IHubSpotListClient>(MockBehavior.Strict);

            mockClient = new Mock<IHubSpotClient>(MockBehavior.Strict);
            mockClient.SetupGet(p => p.Lists).Returns(mockListClient.Object);
        }

        [Test, AutoData]
        public async Task A_single_page_of_contacts_is_fetched(ListContactFilter sut, IReadOnlyList<Property> properties)
        {
            var list = fixture.Build<ContactList>()
                              .With(p => p.HasMore, false)
                              .With(p => p.ContactOffset, null)
                              .Create();

            mockListClient.Setup(p => p.GetContactsInListAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<long?>()))
                          .ReturnsAsync(list);

            var response = await sut.GetContacts(mockClient.Object, properties);

            CollectionAssert.AreEquivalent(response, list.Contacts);

            mockListClient.Verify(p => p.GetContactsInListAsync(It.IsAny<long>(), properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false, It.IsAny<int>(), null), Times.Once);
        }

        [Test, AutoData]
        public async Task Multiple_pages_of_contacts_are_fetched(ListContactFilter sut, IReadOnlyList<Property> properties)
        {
            var listBuilder = fixture.Build<ContactList>();

            var lists = new[]
            {
                listBuilder.With(p => p.HasMore, true).With(p => p.ContactOffset).Create(),
                listBuilder.With(p => p.HasMore, false).Without(p => p.ContactOffset).Create()
            };

            mockListClient.SetupSequence(p => p.GetContactsInListAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<long?>()))
                          .ReturnsSequenceAsync(lists);

            var response = await sut.GetContacts(mockClient.Object, properties);

            CollectionAssert.AreEquivalent(lists.SelectMany(l => l.Contacts), response);

            mockListClient.Verify(p => p.GetContactsInListAsync(It.IsAny<long>(), properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false, It.IsAny<int>(), null), Times.Once);
            mockListClient.Verify(p => p.GetContactsInListAsync(It.IsAny<long>(), properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false, It.IsAny<int>(), lists.First().ContactOffset), Times.Once);
        }
    }
}