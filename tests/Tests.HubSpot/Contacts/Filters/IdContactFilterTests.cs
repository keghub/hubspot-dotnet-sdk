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
    public class IdContactFilterTests
    {
        private IFixture fixture;
        private Mock<IHubSpotClient> mockClient;
        private Mock<IHubSpotContactClient> mockContactClient;

        [SetUp]
        public void Initialize()
        {
            fixture = new Fixture();
            fixture.Customize<Contact>(cu => cu.OmitAutoProperties().With(p => p.Id));

            mockContactClient = new Mock<IHubSpotContactClient>(MockBehavior.Strict);

            mockClient = new Mock<IHubSpotClient>(MockBehavior.Strict);
            mockClient.SetupGet(p => p.Contacts).Returns(mockContactClient.Object);
        }

        [Test, AutoData]
        public async Task Contacts_are_fetched_by_id(IdContactFilter sut, IReadOnlyList<Property> properties)
        {
            var contacts = fixture.CreateMany<Contact>().ToArray();

            mockContactClient.Setup(p => p.GetManyByIdAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsAsync(contacts.ToDictionary(k => k.Id));

            var response = await sut.GetContacts(mockClient.Object, properties);

            CollectionAssert.AreEquivalent(response, contacts);

            mockContactClient.Verify(p => p.GetManyByIdAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.AtLeastOnce);
        }
    }
}