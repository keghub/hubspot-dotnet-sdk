using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Contacts.Filters;
using HubSpot.Model;
using HubSpot.Model.Companies;
using HubSpot.Model.Contacts;
using Moq;
using NUnit.Framework;
using Contact = HubSpot.Model.Contacts.Contact;

namespace Tests.Contacts.Filters
{
    [TestFixture]
    public class CompanyContactFilterTests
    {
        private IFixture fixture;
        private Mock<IHubSpotClient> mockClient;
        private Mock<IHubSpotContactClient> mockContactClient;
        private Mock<IHubSpotCompanyClient> mockCompanyClient;

        [SetUp]
        public void Initialize()
        {
            fixture = new Fixture();
            fixture.Customize<Contact>(cu => cu.OmitAutoProperties().With(p => p.Id));

            mockContactClient = new Mock<IHubSpotContactClient>(MockBehavior.Strict);

            mockCompanyClient = new Mock<IHubSpotCompanyClient>(MockBehavior.Strict);

            mockClient = new Mock<IHubSpotClient>(MockBehavior.Strict);
            mockClient.SetupGet(p => p.Contacts).Returns(mockContactClient.Object);
            mockClient.SetupGet(p => p.Companies).Returns(mockCompanyClient.Object);
        }

        [Test, CustomAutoData]
        public async Task A_single_page_of_contact_ids_is_fetched(CompanyContactFilter sut, IReadOnlyList<Property> properties)
        {
            var list = fixture.Build<ContactIdList>()
                              .With(p => p.HasMore, false)
                              .Without(p => p.Offset)
                              .Create();

            mockCompanyClient.Setup(p => p.GetContactIdsInCompanyAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long?>())).ReturnsAsync(list);

            var contacts = fixture.CreateMany<Contact>().ToArray();

            mockContactClient.Setup(p => p.GetManyByIdAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsAsync(contacts.ToDictionary(k => k.Id));

            var response = await sut.GetContacts(mockClient.Object, properties);

            //CollectionAssert.AreEquivalent(contacts, response);

            mockCompanyClient.Verify(p => p.GetContactIdsInCompanyAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long?>()), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task Multiple_pages_of_contact_ids_are_fetched(CompanyContactFilter sut, IReadOnlyList<Property> properties)
        {
            var listBuilder = fixture.Build<ContactIdList>();

            var lists = new[]
            {
                listBuilder.With(p => p.HasMore, true).With(p => p.Offset).Create(),
                listBuilder.With(p => p.HasMore, false).Without(p => p.Offset).Create()
            };

            mockCompanyClient.SetupSequence(p => p.GetContactIdsInCompanyAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long?>())).ReturnsSequenceAsync(lists);

            var contacts = fixture.CreateMany<Contact>().ToArray();

            mockContactClient.Setup(p => p.GetManyByIdAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsAsync(contacts.ToDictionary(k => k.Id));

            var response = await sut.GetContacts(mockClient.Object, properties);

            mockCompanyClient.Verify(p => p.GetContactIdsInCompanyAsync(It.IsAny<long>(), It.IsAny<int>(), null), Times.Once);
            mockCompanyClient.Verify(p => p.GetContactIdsInCompanyAsync(It.IsAny<long>(), It.IsAny<int>(), lists.First().Offset), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task Multiple_pages_of_contacts_are_fetched_if_too_many(CompanyContactFilter sut, IReadOnlyList<Property> properties)
        {
            var batchSize = sut.BatchSize;

            var ids = fixture.CreateMany<long>(batchSize + 1).ToArray();

            var list = fixture.Build<ContactIdList>().With(p => p.HasMore, false).Without(p => p.Offset).With(p => p.ContactIds, ids).Create();

            mockCompanyClient.Setup(p => p.GetContactIdsInCompanyAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long?>())).ReturnsAsync(list);

            var contacts = fixture.CreateMany<IReadOnlyDictionary<long, Contact>>();

            mockContactClient.SetupSequence(p => p.GetManyByIdAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsSequenceAsync(contacts);

            var response = await sut.GetContacts(mockClient.Object, properties);


        }

        [Test, CustomAutoData]
        public async Task Single_page_of_contacts_is_requested(CompanyContactFilter sut, IReadOnlyList<Property> properties)
        {
            var ids = fixture.CreateMany<long>().ToArray();

            Assume.That(ids.Length, Is.LessThan(sut.BatchSize));

            var list = fixture.Build<ContactIdList>().With(p => p.HasMore, false).Without(p => p.Offset).With(p => p.ContactIds, ids).Create();

            mockCompanyClient.Setup(p => p.GetContactIdsInCompanyAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long?>())).ReturnsAsync(list);

            var contacts = fixture.CreateMany<IReadOnlyDictionary<long, Contact>>();

            mockContactClient.SetupSequence(p => p.GetManyByIdAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsSequenceAsync(contacts);

            var response = await sut.GetContacts(mockClient.Object, properties);

            mockContactClient.Verify(p => p.GetManyByIdAsync(ids, It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task Single_page_of_contacts_is_fetched(CompanyContactFilter sut, IReadOnlyList<Property> properties)
        {
            var ids = fixture.CreateMany<long>().ToArray();

            Assume.That(ids.Length, Is.LessThanOrEqualTo(sut.BatchSize));

            var list = fixture.Build<ContactIdList>().With(p => p.HasMore, false).Without(p => p.Offset).With(p => p.ContactIds, ids).Create();

            mockCompanyClient.Setup(p => p.GetContactIdsInCompanyAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long?>())).ReturnsAsync(list);

            var contacts = fixture.CreateMany<IReadOnlyDictionary<long, Contact>>();

            mockContactClient.SetupSequence(p => p.GetManyByIdAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsSequenceAsync(contacts);

            var response = await sut.GetContacts(mockClient.Object, properties);

            Assert.That(response, Is.EquivalentTo(contacts.Take(1).SelectMany(c => c.Values)));
        }

        [Test, CustomAutoData]
        public async Task Multiple_pages_of_contacts_are_requested_if_too_many(CompanyContactFilter sut, IReadOnlyList<Property> properties)
        {
            var batchSize = sut.BatchSize;

            var ids = fixture.CreateMany<long>(batchSize + 1).ToArray();

            var list = fixture.Build<ContactIdList>().With(p => p.HasMore, false).Without(p => p.Offset).With(p => p.ContactIds, ids).Create();

            mockCompanyClient.Setup(p => p.GetContactIdsInCompanyAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long?>())).ReturnsAsync(list);

            var contacts = fixture.CreateMany<IReadOnlyDictionary<long, Contact>>();

            mockContactClient.SetupSequence(p => p.GetManyByIdAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsSequenceAsync(contacts);

            var response = await sut.GetContacts(mockClient.Object, properties);

            Assert.That(response, Is.EquivalentTo(contacts.Take(2).SelectMany(c => c.Values)));
        }
    }
}