using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Contacts;
using HubSpot.Internal;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using HubSpotContact = HubSpot.Model.Contacts.Contact;

namespace Tests.Contacts
{
    [TestFixture]
    public class HubSpotContactConnectorTests
    {
        private Mock<IHubSpotContactClient> mockContactClient;
        private Mock<ITypeManager<HubSpotContact, HubSpot.Contacts.Contact>> mockTypeManager;

        private Mock<IHubSpotClient> mockHubSpotClient;

        [SetUp]
        public void Initialize()
        {
            mockTypeManager = new Mock<ITypeManager<HubSpotContact, HubSpot.Contacts.Contact>>();

            mockContactClient = new Mock<IHubSpotContactClient>();

            mockHubSpotClient = new Mock<IHubSpotClient>();
            mockHubSpotClient.SetupGet(p => p.Contacts).Returns(() => mockContactClient.Object);
        }


        private HubSpotContactConnector CreateSystemUnderTest()
        {
            return new HubSpotContactConnector(mockHubSpotClient.Object, mockTypeManager.Object, Mock.Of<ILogger<HubSpotContactConnector>>());
        }

        [Test, AutoData]
        public async Task GetByIdAsync_forwards_to_client(HubSpotContact contact, TestContact expected)
        {
            mockContactClient.Setup(p => p.GetByIdAsync(contact.Id, It.IsAny<IReadOnlyList<IProperty>>(), PropertyMode.ValueOnly, FormSubmissionMode.None, false))
                             .ReturnsAsync(contact)
                             .Verifiable();

            mockTypeManager.Setup(p => p.ConvertTo<TestContact>(contact))
                           .Returns(expected)
                           .Verifiable();

            mockTypeManager.Setup(p => p.GetCustomProperties<TestContact>(TypeManager.AllProperties))
                           .Returns(Array.Empty<(string, PropertyInfo, CustomPropertyAttribute)>());

            var sut = CreateSystemUnderTest();

            var result = await sut.GetByIdAsync<TestContact>(contact.Id);

            mockContactClient.Verify();

            mockTypeManager.Verify();
        }

        [Test, AutoData]
        public async Task GetByEmailAsync_forwards_to_client(string email, HubSpotContact contact, TestContact expected)
        {
            mockContactClient.Setup(p => p.GetByEmailAsync(email, It.IsAny<IReadOnlyList<IProperty>>(), PropertyMode.ValueOnly, FormSubmissionMode.None, false))
                             .ReturnsAsync(contact)
                             .Verifiable();

            mockTypeManager.Setup(p => p.ConvertTo<TestContact>(contact))
                           .Returns(expected)
                           .Verifiable();

            mockTypeManager.Setup(p => p.GetCustomProperties<TestContact>(TypeManager.AllProperties))
                           .Returns(Array.Empty<(string, PropertyInfo, CustomPropertyAttribute)>());

            var sut = CreateSystemUnderTest();

            var result = await sut.GetByEmailAsync<TestContact>(email);

            mockContactClient.Verify();

            mockTypeManager.Verify();
        }

        [Test, AutoData]
        public async Task GetByUserTokenAsync_forwards_to_client(string userToken, HubSpotContact contact, TestContact expected)
        {
            mockContactClient.Setup(p => p.GetByUserTokenAsync(userToken, It.IsAny<IReadOnlyList<IProperty>>(), PropertyMode.ValueOnly, FormSubmissionMode.None, false))
                             .ReturnsAsync(contact)
                             .Verifiable();

            mockTypeManager.Setup(p => p.ConvertTo<TestContact>(contact))
                           .Returns(expected)
                           .Verifiable();

            mockTypeManager.Setup(p => p.GetCustomProperties<TestContact>(TypeManager.AllProperties))
                           .Returns(Array.Empty<(string, PropertyInfo, CustomPropertyAttribute)>());

            var sut = CreateSystemUnderTest();

            var result = await sut.GetByUserTokenAsync<TestContact>(userToken);

            mockContactClient.Verify();

            mockTypeManager.Verify();
        }

        [Test, AutoData]
        public async Task FindContacts_forwards_to_filter(HubSpotContact[] contacts, TestContact[] expected)
        {
            var mockFilter = new Mock<IContactFilter>();

            mockTypeManager.SetupSequence(p => p.ConvertTo<TestContact>(It.IsAny<HubSpotContact>())).ReturnsSequence(expected);

            mockTypeManager.Setup(p => p.GetCustomProperties<TestContact>(TypeManager.AllProperties))
                           .Returns(Array.Empty<(string, PropertyInfo, CustomPropertyAttribute)>());

            mockFilter.Setup(p => p.GetContacts(It.IsAny<IHubSpotClient>(), It.IsAny<IReadOnlyList<IProperty>>())).ReturnsAsync(contacts);

            var sut = CreateSystemUnderTest();

            var result = await sut.FindContactsAsync<TestContact>(mockFilter.Object);

            CollectionAssert.AreEquivalent(result, expected);
        }

        [Test, AutoData]
        public async Task SaveAsync_persists_a_new_contact(TestContact contact, (string, string)[] properties)
        {
            contact.Id = 0;
            contact.Created = default;

            var toCreate = CreateFromContact(contact);

            mockTypeManager.Setup(p => p.GetModifiedProperties(It.IsAny<TestContact>())).Returns(properties);

            mockContactClient.Setup(p => p.CreateAsync(It.IsAny<IReadOnlyList<ValuedProperty>>())).ReturnsAsync(toCreate);

            mockTypeManager.Setup(p => p.ConvertTo<TestContact>(It.IsAny<HubSpotContact>())).Returns(contact);

            var sut = CreateSystemUnderTest();

            var newContact = await sut.SaveAsync(contact);

            Assert.That(newContact, Is.SameAs(contact));
        }

        private static HubSpotContact CreateFromContact(TestContact contact)
        {
            var hubspot = new HubSpotContact
            {
                Id = contact.Id,
                Properties = new Dictionary<string, VersionedProperty>
                {
                    ["firstname"] = new VersionedProperty { Value = contact.FirstName },
                    ["lastname"] = new VersionedProperty { Value = contact.LastName },
                    ["email"] = new VersionedProperty { Value = contact.Email },
                    ["createdate"] = new VersionedProperty { Value = contact.Created.ToUnixTimeMilliseconds().ToString("D") },
                    ["associatedcompanyid"] = new VersionedProperty() { Value = contact.AssociatedCompanyId.ToString("D") },
                    ["customProperty"] = new VersionedProperty { Value = contact.CustomProperty }
                }
            };

            return hubspot;
        }
    }
}