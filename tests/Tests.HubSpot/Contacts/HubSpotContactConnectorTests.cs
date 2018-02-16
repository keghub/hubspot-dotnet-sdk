using System;
using System.Collections.Generic;
using System.Linq;
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
using Contact = HubSpot.Model.Contacts.Contact;

namespace Tests.Contacts
{
    [TestFixture]
    public class HubSpotContactConnectorTests
    {
        private Mock<IHubSpotContactClient> mockContactClient;
        private Mock<ITypeManager<Contact, HubSpot.Contacts.Contact>> mockTypeManager;

        private Mock<IHubSpotClient> mockHubSpotClient;

        [SetUp]
        public void Initialize()
        {
            mockTypeManager = new Mock<ITypeManager<Contact, HubSpot.Contacts.Contact>>();

            mockContactClient = new Mock<IHubSpotContactClient>();

            mockHubSpotClient = new Mock<IHubSpotClient>();
            mockHubSpotClient.SetupGet(p => p.Contacts).Returns(() => mockContactClient.Object);
        }


        private HubSpotContactConnector CreateSystemUnderTest()
        {
            return new HubSpotContactConnector(mockHubSpotClient.Object, mockTypeManager.Object, Mock.Of<ILogger<HubSpotContactConnector>>());
        }

        [Test, AutoData]
        public async Task GetByIdAsync_forwards_to_client(Contact contact, TestContact expected)
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
        public async Task GetByEmailAsync_forwards_to_client(string email, Contact contact, TestContact expected)
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
        public async Task GetByUserTokenAsync_forwards_to_client(string userToken, Contact contact, TestContact expected)
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
    }
}