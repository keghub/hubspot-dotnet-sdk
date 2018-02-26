using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Contacts.Filters;
using HubSpot.Model;
using HubSpotContact = HubSpot.Model.Contacts.Contact;

namespace HubSpot.Contacts
{
    public interface IContactFilter
    {
        Task<IReadOnlyList<HubSpotContact>> GetContacts(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery);
    }

    public static class FilterContacts
    {
        public static IContactFilter All => new AllContactFilter();

        public static IContactFilter RecentlyModified => new RecentlyUpdatedContactFilter();

        public static IContactFilter RecentlyCreated => new RecentlyCreatedContactFilter();

        public static IContactFilter ByEmail(params string[] emails) => new EmailContactFilter(emails);

        public static IContactFilter ById(params long[] ids) => new IdContactFilter(ids);

        public static IContactFilter Query(string searchQuery) => new SearchContactFilter(searchQuery);

        public static IContactFilter ByCompanyId(long companyId) => new CompanyContactFilter(companyId);
    }
}