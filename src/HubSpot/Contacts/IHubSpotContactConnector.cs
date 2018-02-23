using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Contacts
{
    public interface IHubSpotContactConnector
    {
        Task<TContact> GetByIdAsync<TContact>(long contactId)
            where TContact : Contact, new();

        Task<TContact> GetByEmailAsync<TContact>(string email)
            where TContact : Contact, new();

        Task<TContact> GetByUserTokenAsync<TContact>(string userToken)
            where TContact : Contact, new();

        Task<TContact> SaveAsync<TContact>(TContact contact)
            where TContact : Contact, new();

        Task<IReadOnlyList<TContact>> FindContacts<TContact>(IContactFilter filter = null)
            where TContact : Contact, new();
    }

    public static class HubSpotContactConnectorExtensions
    {
        public static Task<Contact> GetByIdAsync(this IHubSpotContactConnector connector, long contactId) => connector.GetByIdAsync<Contact>(contactId);

        public static Task<Contact> GetByEmailAsync(this IHubSpotContactConnector connector, string email) => connector.GetByEmailAsync<Contact>(email);

        public static Task<Contact> GetByUserTokenAsync(this IHubSpotContactConnector connector, string userToken) => connector.GetByUserTokenAsync<Contact>(userToken);

        public static Task<Contact> SaveAsync(this IHubSpotContactConnector connector, Contact contact) => connector.SaveAsync(contact);

        public static Task<IReadOnlyList<Contact>> FindContacts(this IHubSpotContactConnector connector, IContactFilter filter = null) => connector.FindContacts<Contact>(filter);
    }
}