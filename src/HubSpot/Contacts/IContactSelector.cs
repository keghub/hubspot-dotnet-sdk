using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Contacts.Selectors;
using HubSpot.Model;
using HubSpotContact = HubSpot.Model.Contacts.Contact;

namespace HubSpot.Contacts
{
    public interface IContactSelector
    {
        Task<HubSpotContact> GetContact(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery);
    }

    public static class SelectContact
    {
        public static IContactSelector ByEmail(string email) => new EmailContactSelector(email);

        public static IContactSelector ById(long id) => new IdContactSelector(id);

        public static IContactSelector ByUserToken(string userToken) => new UserTokenContactSelector(userToken);
    }

    public static class SelectContactExtensions
    {
        public static Task<TContact> GetByIdAsync<TContact>(this IHubSpotContactConnector connector, long contactId) where TContact : Contact, new() 
            => connector.GetAsync<TContact>(SelectContact.ById(contactId));

        public static Task<TContact> GetByEmailAsync<TContact>(this IHubSpotContactConnector connector, string email) where TContact : Contact, new()
            => connector.GetAsync<TContact>(SelectContact.ByEmail(email));

        public static Task<TContact> GetByUserTokenAsync<TContact>(this IHubSpotContactConnector connector, string userToken) where TContact : Contact, new()
            => connector.GetAsync<TContact>(SelectContact.ByUserToken(userToken));

        public static Task<Contact> GetByIdAsync(this IHubSpotContactConnector connector, long contactId) => GetByIdAsync<Contact>(connector, contactId);

        public static Task<Contact> GetByEmailAsync(this IHubSpotContactConnector connector, string email) => GetByEmailAsync<Contact>(connector, email);

        public static Task<Contact> GetByUserTokenAsync(this IHubSpotContactConnector connector, string userToken) => GetByUserTokenAsync<Contact>(connector, userToken);
    }
}