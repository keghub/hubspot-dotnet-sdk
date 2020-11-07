using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Contacts.Filters;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Moq;
using NUnit.Framework;
using ContactList = HubSpot.Model.Contacts.ContactList;

namespace Tests.Contacts.Filters
{
    [TestFixture]
    public class AllContactFilterTests
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

        [Test, CustomAutoData]
        public async Task A_single_page_of_contacts_is_fetched(AllContactFilter sut, IReadOnlyList<Property> properties)
        {
            var list = fixture.Build<ContactList>()
                              .With(p => p.HasMore, false)
                              .Without(p => p.ContactOffset)
                              .Create();

            mockContactClient.Setup(p => p.GetAllAsync(It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<long?>()))
                             .ReturnsAsync(list);

            var response = await sut.GetContacts(mockClient.Object, properties);

            CollectionAssert.AreEquivalent(response, list.Contacts);

            mockContactClient.Verify(p => p.GetAllAsync(properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false, It.IsAny<int>(), null), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task Multiple_pages_of_contacts_are_fetched(AllContactFilter sut, IReadOnlyList<Property> properties)
        {
            var listBuilder = fixture.Build<ContactList>();

            var lists = new[]
            {
                listBuilder.With(p => p.HasMore, true).With(p => p.ContactOffset).Create(),
                listBuilder.With(p => p.HasMore, false).Without(p => p.ContactOffset).Create()
            };

            mockContactClient.SetupSequence(p => p.GetAllAsync(It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<long?>()))
                             .ReturnsSequenceAsync(lists);

            var response = await sut.GetContacts(mockClient.Object, properties);

            CollectionAssert.AreEquivalent(lists.SelectMany(l => l.Contacts), response);

            mockContactClient.Verify(p => p.GetAllAsync(properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false, It.IsAny<int>(), null), Times.Once);
            mockContactClient.Verify(p => p.GetAllAsync(properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false, It.IsAny<int>(), lists.First().ContactOffset), Times.Once);
        }
    }
}