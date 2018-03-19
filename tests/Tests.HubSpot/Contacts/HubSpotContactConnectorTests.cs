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
        private Mock<IContactTypeManager> mockTypeManager;

        private Mock<IHubSpotClient> mockHubSpotClient;

        [SetUp]
        public void Initialize()
        {
            mockTypeManager = new Mock<IContactTypeManager>();

            mockContactClient = new Mock<IHubSpotContactClient>();

            mockHubSpotClient = new Mock<IHubSpotClient>();
            mockHubSpotClient.SetupGet(p => p.Contacts).Returns(() => mockContactClient.Object);
        }

        private HubSpotContactConnector CreateSystemUnderTest()
        {
            return new HubSpotContactConnector(mockHubSpotClient.Object, mockTypeManager.Object, Mock.Of<ILogger<HubSpotContactConnector>>());
        }

        [Test]
        public void GetAsync_requires_a_selector()
        {
            var sut = CreateSystemUnderTest();

            Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetAsync<TestContact>(null));
        }

        [Test, ContactAutoData]
        public async Task GetAsync_forwards_to_selector(HubSpotContact contact, TestContact expected, CustomPropertyInfo[] properties)
        {
            mockTypeManager.Setup(p => p.ConvertTo<TestContact>(contact))
                           .Returns(expected)
                           .Verifiable();

            mockTypeManager.Setup(p => p.GetCustomProperties<TestContact>(TypeManager.AllProperties))
                           .Returns(properties);

            var mockSelector = new Mock<IContactSelector>(MockBehavior.Strict);
            mockSelector.Setup(p => p.GetContact(It.IsAny<IHubSpotClient>(), It.IsAny<IReadOnlyList<IProperty>>()))
                        .ReturnsAsync(contact)
                        .Verifiable();

            var sut = CreateSystemUnderTest();

            var result = await sut.GetAsync<TestContact>(mockSelector.Object);

            Assert.That(result, Is.SameAs(expected));

            mockTypeManager.Verify();

            mockSelector.Verify();
        }

        [Test, ContactAutoData]
        public async Task GetAsync_returns_null_if_NotFoundException(CustomPropertyInfo[] properties)
        {
            mockTypeManager.Setup(p => p.GetCustomProperties<TestContact>(TypeManager.AllProperties))
                           .Returns(properties)
                           .Verifiable();

            var mockSelector = new Mock<IContactSelector>(MockBehavior.Strict);
            mockSelector.Setup(p => p.GetContact(It.IsAny<IHubSpotClient>(), It.IsAny<IReadOnlyList<IProperty>>()))
                        .Throws(new NotFoundException(It.IsAny<string>(), It.IsAny<Exception>()))
                        .Verifiable();

            var sut = CreateSystemUnderTest();

            var result = await sut.GetAsync<TestContact>(mockSelector.Object);

            Assert.That(result, Is.Null);

            mockTypeManager.Verify();

            mockSelector.Verify();
        }

        [Test, ContactAutoData]
        public async Task FindAsync_forwards_to_filter(HubSpotContact[] contacts, TestContact[] expected, CustomPropertyInfo[] properties)
        {
            var mockFilter = new Mock<IContactFilter>();

            mockTypeManager.SetupSequence(p => p.ConvertTo<TestContact>(It.IsAny<HubSpotContact>())).ReturnsSequence(expected);

            mockTypeManager.Setup(p => p.GetCustomProperties<TestContact>(TypeManager.AllProperties))
                           .Returns(properties);

            mockFilter.Setup(p => p.GetContacts(It.IsAny<IHubSpotClient>(), It.IsAny<IReadOnlyList<IProperty>>())).ReturnsAsync(contacts);

            var sut = CreateSystemUnderTest();

            var result = await sut.FindAsync<TestContact>(mockFilter.Object);

            CollectionAssert.AreEquivalent(result, expected);
        }

        [Test]
        public void SaveAsync_requires_a_contact()
        {
            var sut = CreateSystemUnderTest();

            Assert.ThrowsAsync<ArgumentNullException>(() => sut.SaveAsync<TestContact>(null));
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