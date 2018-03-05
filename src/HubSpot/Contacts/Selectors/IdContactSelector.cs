using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;

namespace HubSpot.Contacts.Selectors
{
    public class IdContactSelector : IContactSelector
    {
        private readonly long _contactId;

        public IdContactSelector(long contactId)
        {
            _contactId = contactId;
        }

        public Task<Model.Contacts.Contact> GetContact(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery)
        {
            return client.Contacts.GetByIdAsync(_contactId, propertiesToQuery, PropertyMode.ValueOnly, FormSubmissionMode.None, false);
        }
    }
}
