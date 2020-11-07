using AutoFixture.NUnit3;
using HubSpot.Contacts;
using HubSpot.Contacts.Filters;
using NUnit.Framework;

namespace Tests.Contacts
{
    [TestFixture]
    public class FilterContactsTests
    {
        [Test, CustomAutoData]
        public void All_returns_proper_filter()
        {
            Assert.That(FilterContacts.All, Is.InstanceOf<AllContactFilter>());
        }

        [Test, CustomAutoData]
        public void RecentlyCreated_returns_proper_filter()
        {
            Assert.That(FilterContacts.RecentlyCreated, Is.InstanceOf<RecentlyCreatedContactFilter>());
        }

        [Test, CustomAutoData]
        public void RecentlyUpdated_returns_proper_filter()
        {
            Assert.That(FilterContacts.RecentlyModified, Is.InstanceOf<RecentlyUpdatedContactFilter>());
        }

        [Test, CustomAutoData]
        public void ByEmail_returns_proper_filter(string[] emails)
        {
            Assert.That(FilterContacts.ByEmail(emails), Is.InstanceOf<EmailContactFilter>());
        }

        [Test, CustomAutoData]
        public void ById_returns_proper_filter(long[] ids)
        {
            Assert.That(FilterContacts.ById(ids), Is.InstanceOf<IdContactFilter>());
        }

        [Test, CustomAutoData]
        public void ByCompany_returns_proper_filter(long companyId)
        {
            Assert.That(FilterContacts.ByCompanyId(companyId), Is.InstanceOf<CompanyContactFilter>());
        }

        [Test, CustomAutoData]
        public void Query_returns_proper_filter(string query)
        {
            Assert.That(FilterContacts.Query(query), Is.InstanceOf<SearchContactFilter>());
        }

        [Test, CustomAutoData]
        public void ByList_returns_proper_filter(long listId)
        {
            Assert.That(FilterContacts.ByListId(listId), Is.InstanceOf<ListContactFilter>());
        }
    }
}