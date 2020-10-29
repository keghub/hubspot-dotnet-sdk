using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Contacts;
using HubSpot.Contacts.Filters;
using Moq;
using NUnit.Framework;
// ReSharper disable InvokeAsExtensionMethod

namespace Tests.Contacts {
    [TestFixture]
    public class FilterContactExtensionsTests
    {
        private Mock<IHubSpotContactConnector> mockConnector;

        [SetUp]
        public void Initialize()
        {
            mockConnector = new Mock<IHubSpotContactConnector>(MockBehavior.Strict);
        }

        [Test, CustomAutoData]
        public async Task FindAllAsync_forwards_with_filter(Contact[] expected)
        {
            mockConnector.Setup(p => p.FindAsync<Contact>(It.IsAny<IContactFilter>()))
                         .ReturnsAsync(expected);

            var contacts = await FilterContactExtensions.FindAllAsync<Contact>(mockConnector.Object);

            Assert.That(contacts,Is.SameAs(expected));

            mockConnector.Verify(p => p.FindAsync<Contact>(It.IsAny<AllContactFilter>()));
        }

        [Test, CustomAutoData]
        public async Task FindRecentlyModifiedAsync_forwards_with_filter(Contact[] expected)
        {
            mockConnector.Setup(p => p.FindAsync<Contact>(It.IsAny<IContactFilter>()))
                         .ReturnsAsync(expected);

            var contacts = await FilterContactExtensions.FindRecentlyModifiedAsync<Contact>(mockConnector.Object);

            Assert.That(contacts, Is.SameAs(expected));

            mockConnector.Verify(p => p.FindAsync<Contact>(It.IsAny<RecentlyUpdatedContactFilter>()));
        }

        [Test, CustomAutoData]
        public async Task FindRecentlyCreatedAsync_forwards_with_filter(Contact[] expected)
        {
            mockConnector.Setup(p => p.FindAsync<Contact>(It.IsAny<IContactFilter>()))
                         .ReturnsAsync(expected);

            var contacts = await FilterContactExtensions.FindRecentlyCreatedAsync<Contact>(mockConnector.Object);

            Assert.That(contacts, Is.SameAs(expected));

            mockConnector.Verify(p => p.FindAsync<Contact>(It.IsAny<RecentlyCreatedContactFilter>()));
        }

        [Test, CustomAutoData]
        public async Task FindByEmailAsync_forwards_with_filter(Contact[] expected, string[] emails)
        {
            mockConnector.Setup(p => p.FindAsync<Contact>(It.IsAny<IContactFilter>()))
                         .ReturnsAsync(expected);

            var contacts = await FilterContactExtensions.FindByEmailAsync<Contact>(mockConnector.Object, emails);

            Assert.That(contacts, Is.SameAs(expected));

            mockConnector.Verify(p => p.FindAsync<Contact>(It.IsAny<EmailContactFilter>()));
        }

        [Test, CustomAutoData]
        public async Task FindByIdAsync_forwards_with_filter(Contact[] expected, long[] contactIds)
        {
            mockConnector.Setup(p => p.FindAsync<Contact>(It.IsAny<IContactFilter>()))
                         .ReturnsAsync(expected);

            var contacts = await FilterContactExtensions.FindByIdAsync<Contact>(mockConnector.Object, contactIds);

            Assert.That(contacts, Is.SameAs(expected));

            mockConnector.Verify(p => p.FindAsync<Contact>(It.IsAny<IdContactFilter>()));
        }

        [Test, CustomAutoData]
        public async Task FindByCompanyIdAsync_forwards_with_filter(Contact[] expected, long companyId)
        {
            mockConnector.Setup(p => p.FindAsync<Contact>(It.IsAny<IContactFilter>()))
                         .ReturnsAsync(expected);

            var contacts = await FilterContactExtensions.FindByCompanyIdAsync<Contact>(mockConnector.Object, companyId);

            Assert.That(contacts, Is.SameAs(expected));

            mockConnector.Verify(p => p.FindAsync<Contact>(It.IsAny<CompanyContactFilter>()));
        }

        [Test, CustomAutoData]
        public async Task FindInListAsync_forwards_with_filter(Contact[] expected, long listId)
        {
            mockConnector.Setup(p => p.FindAsync<Contact>(It.IsAny<IContactFilter>()))
                         .ReturnsAsync(expected);

            var contacts = await FilterContactExtensions.FindInListAsync<Contact>(mockConnector.Object, listId);

            Assert.That(contacts, Is.SameAs(expected));

            mockConnector.Verify(p => p.FindAsync<Contact>(It.IsAny<ListContactFilter>()));
        }

        [Test, CustomAutoData]
        public async Task FindAsync_forwards_with_filter(Contact[] expected, string query)
        {
            mockConnector.Setup(p => p.FindAsync<Contact>(It.IsAny<IContactFilter>()))
                         .ReturnsAsync(expected);

            var contacts = await FilterContactExtensions.FindAsync<Contact>(mockConnector.Object, query);

            Assert.That(contacts, Is.SameAs(expected));

            mockConnector.Verify(p => p.FindAsync<Contact>(It.IsAny<SearchContactFilter>()));
        }
    }

    [TestFixture]
    public class HubSpotContactConnectorExtensionsTests
    {
        private Mock<IHubSpotContactConnector> mockConnector;

        [SetUp]
        public void Initialize()
        {
            mockConnector = new Mock<IHubSpotContactConnector>(MockBehavior.Strict);
        }

        [Test, CustomAutoData]
        public async Task GetAsync_forwards_to_connector(Contact expected)
        {
            mockConnector.Setup(p => p.GetAsync<Contact>(It.IsAny<IContactSelector>()))
                         .ReturnsAsync(expected)
                         .Verifiable();

            var contact = await HubSpotContactConnectorExtensions.GetAsync(mockConnector.Object, Mock.Of<IContactSelector>());

            mockConnector.Verify();
        }

        [Test, CustomAutoData]
        public async Task SaveAsync_forwards_to_connector(Contact testContact)
        {
            mockConnector.Setup(p => p.SaveAsync(It.IsAny<Contact>()))
                         .ReturnsAsync(testContact)
                         .Verifiable();

            var savedContact = await HubSpotContactConnectorExtensions.SaveAsync(mockConnector.Object, testContact);

            mockConnector.Verify();
        }

        [Test, CustomAutoData]
        public async Task FindAsync_forwards_to_connector(Contact[] expected)
        {
            mockConnector.Setup(p => p.FindAsync<Contact>(It.IsAny<IContactFilter>()))
                         .ReturnsAsync(expected)
                         .Verifiable();

            var contacts = await HubSpotContactConnectorExtensions.FindAsync(mockConnector.Object, Mock.Of<IContactFilter>());

            mockConnector.Verify();
        }
    }
}