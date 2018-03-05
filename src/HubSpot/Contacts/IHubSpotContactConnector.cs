using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Contacts
{
    public interface IHubSpotContactConnector
    {
        Task<TContact> GetAsync<TContact>(IContactSelector selector)
            where TContact : Contact, new();

        Task<TContact> SaveAsync<TContact>(TContact contact)
            where TContact : Contact, new();

        Task<IReadOnlyList<TContact>> FindAsync<TContact>(IContactFilter filter = null)
            where TContact : Contact, new();
    }

    public static class HubSpotContactConnectorExtensions
    {
        public static Task<Contact> GetAsync(this IHubSpotContactConnector connector, IContactSelector selector) => connector.GetAsync<Contact>(selector);

        public static Task<Contact> SaveAsync(this IHubSpotContactConnector connector, Contact contact) => connector.SaveAsync(contact);

        public static Task<IReadOnlyList<Contact>> FindAsync(this IHubSpotContactConnector connector, IContactFilter filter = null) => connector.FindAsync<Contact>(filter);
    }
}