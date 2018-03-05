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

    public static class FilterContactExtensions
    {
        public static Task<IReadOnlyList<TContact>> FindAllAsync<TContact>(this IHubSpotContactConnector connector) where TContact : Contact, new()
            => connector.FindAsync<TContact>(FilterContacts.All);

        public static Task<IReadOnlyList<TContact>> FindRecentlyModifiedAsync<TContact>(this IHubSpotContactConnector connector) where TContact : Contact, new()
            => connector.FindAsync<TContact>(FilterContacts.RecentlyModified);

        public static Task<IReadOnlyList<TContact>> FindRecentlyCreatedAsync<TContact>(this IHubSpotContactConnector connector) where TContact : Contact, new()
            => connector.FindAsync<TContact>(FilterContacts.RecentlyCreated);

        public static Task<IReadOnlyList<TContact>> FindByEmailAsync<TContact>(this IHubSpotContactConnector connector, params string[] emails) where TContact : Contact, new()
            => connector.FindAsync<TContact>(FilterContacts.ByEmail(emails));

        public static Task<IReadOnlyList<TContact>> FindByIdAsync<TContact>(this IHubSpotContactConnector connector, params long[] ids) where TContact : Contact, new()
            => connector.FindAsync<TContact>(FilterContacts.ById(ids));

        public static Task<IReadOnlyList<TContact>> FindByCompanyIdAsync<TContact>(this IHubSpotContactConnector connector, long companyId) where TContact : Contact, new()
            => connector.FindAsync<TContact>(FilterContacts.ByCompanyId(companyId));

        public static Task<IReadOnlyList<TContact>> FindAsync<TContact>(this IHubSpotContactConnector connector, string searchQuery) where TContact : Contact, new()
            => connector.FindAsync<TContact>(FilterContacts.Query(searchQuery));
    }
}