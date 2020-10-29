using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Contacts;
using HubSpot.Contacts.Selectors;
using Moq;
using NUnit.Framework;
// ReSharper disable InvokeAsExtensionMethod

namespace Tests.Contacts
{
    [TestFixture]
    public class SelectContactExtensionsTests
    {
        private Mock<IHubSpotContactConnector> mockConnector;

        [SetUp]
        public void Initialize()
        {
            mockConnector = new Mock<IHubSpotContactConnector>(MockBehavior.Strict);
        }

        [Test, CustomAutoData]
        public async Task GetByIdAsync_forwards_with_selector(long contactId, Contact expected)
        {
            mockConnector.Setup(p => p.GetAsync<Contact>(It.IsAny<IContactSelector>()))
                         .ReturnsAsync(expected);

            var contact = await SelectContactExtensions.GetByIdAsync(mockConnector.Object, contactId);

            Assert.That(contact, Is.SameAs(expected));

            mockConnector.Verify(p => p.GetAsync<Contact>(It.IsAny<IdContactSelector>()), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetByEmailAsync_forwards_with_selector(string email, Contact expected)
        {
            mockConnector.Setup(p => p.GetAsync<Contact>(It.IsAny<IContactSelector>()))
                         .ReturnsAsync(expected);

            var contact = await SelectContactExtensions.GetByEmailAsync(mockConnector.Object, email);

            Assert.That(contact, Is.SameAs(expected));

            mockConnector.Verify(p => p.GetAsync<Contact>(It.IsAny<EmailContactSelector>()), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetByUserTokenAsync_forwards_with_selector(string userToken, Contact expected)
        {
            mockConnector.Setup(p => p.GetAsync<Contact>(It.IsAny<IContactSelector>()))
                         .ReturnsAsync(expected);

            var contact = await SelectContactExtensions.GetByUserTokenAsync(mockConnector.Object, userToken);

            Assert.That(contact, Is.SameAs(expected));

            mockConnector.Verify(p => p.GetAsync<Contact>(It.IsAny<UserTokenContactSelector>()), Times.Once);
        }
    }
}