using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Contacts.Selectors;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Moq;
using NUnit.Framework;

namespace Tests.Contacts.Selectors {
    [TestFixture]
    public class IdContactSelectorTests
    {
        private Mock<IHubSpotClient> mockClient;
        private Mock<IHubSpotContactClient> mockContactClient;

        [SetUp]
        public void Initialize()
        {
            mockContactClient = new Mock<IHubSpotContactClient>(MockBehavior.Strict);

            mockClient = new Mock<IHubSpotClient>(MockBehavior.Strict);
            mockClient.SetupGet(p => p.Contacts).Returns(mockContactClient.Object);
        }

        [Test, CustomAutoData]
        public async Task GetContact_forwards_to_client(IdContactSelector sut, Contact contact, IReadOnlyList<Property> properties)
        {
            mockContactClient.Setup(p => p.GetByIdAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>()))
                             .ReturnsAsync(contact);

            var response = await sut.GetContact(mockClient.Object, properties);

            Assert.That(response, Is.SameAs(contact));

            mockContactClient.Verify(p => p.GetByIdAsync(It.IsAny<long>(), properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false), Times.Once);
        }
    }
}