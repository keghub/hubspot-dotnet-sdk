using AutoFixture.NUnit3;
using HubSpot.Contacts;
using HubSpot.Contacts.Filters;
using NUnit.Framework;

namespace Tests.Contacts
{
    [TestFixture]
    public class FilterContactsTests
    {
        [Test, AutoData]
        public void All_returns_proper_filter()
        {
            Assert.That(FilterContacts.All, Is.InstanceOf<AllContactFilter>());
        }

        [Test, AutoData]
        public void RecentlyCreated_returns_proper_filter()
        {
            Assert.That(FilterContacts.RecentlyCreated, Is.InstanceOf<RecentlyCreatedContactFilter>());
        }

        [Test, AutoData]
        public void RecentlyUpdated_returns_proper_filter()
        {
            Assert.That(FilterContacts.RecentlyModified, Is.InstanceOf<RecentlyUpdatedContactFilter>());
        }

        [Test, AutoData]
        public void ByEmail_returns_proper_filter(string[] emails)
        {
            Assert.That(FilterContacts.ByEmail(emails), Is.InstanceOf<EmailContactFilter>());
        }

        [Test, AutoData]
        public void ById_returns_proper_filter(long[] ids)
        {
            Assert.That(FilterContacts.ById(ids), Is.InstanceOf<IdContactFilter>());
        }

        [Test, AutoData]
        public void ByCompany_returns_proper_filter(long companyId)
        {
            Assert.That(FilterContacts.ByCompanyId(companyId), Is.InstanceOf<CompanyContactFilter>());
        }

        [Test, AutoData]
        public void Query_returns_proper_filter(string query)
        {
            Assert.That(FilterContacts.Query(query), Is.InstanceOf<SearchContactFilter>());
        }
    }
}