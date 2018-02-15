using System.Threading.Tasks;

namespace HubSpot.Contacts
{
    public interface IHubSpotContactConnector
    {
        Task<TContact> GetByIdAsync<TContact>(long contactId) 
            where TContact : Contact;

        Task<TContact> GetByEmailAsync<TContact>(string email)
            where TContact : Contact;

        Task<TContact> GetByUserTokenAsync<TContact>(string userToken)
            where TContact : Contact;

        Task<TContact> CreateAsync<TContact>()
            where TContact : Contact;

        Task SaveAsync<TContact>(TContact contact)
            where TContact : Contact;
    }

    public static class HubSpotContactConnectorExtensions
    {
        public static Task<Contact> GetContactByIdAsync(this IHubSpotContactConnector connector, long contactId) => connector.GetByIdAsync<Contact>(contactId);

        public static Task<Contact> GetContactByEmailAsync(this IHubSpotContactConnector connector, string email) => connector.GetByEmailAsync<Contact>(email);

        public static Task<Contact> GetContactByUserTokenAsync(this IHubSpotContactConnector connector, string userToken) => connector.GetByUserTokenAsync<Contact>(userToken);

        public static Task<Contact> CreateContactAsync(this IHubSpotContactConnector connector) => connector.CreateAsync<Contact>();

        public static Task SaveContactAsync(this IHubSpotContactConnector connector, Contact contact) => connector.SaveAsync(contact);
    }
}