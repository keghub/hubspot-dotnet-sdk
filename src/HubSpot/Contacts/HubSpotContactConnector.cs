using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubSpot.Internal;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Microsoft.Extensions.Logging;

namespace HubSpot.Contacts
{
    public class HubSpotContactConnector : IHubSpotContactConnector
    {
        private readonly IHubSpotClient _client;
        private readonly ITypeManager<Model.Contacts.Contact, Contact> _typeManager;

        public HubSpotContactConnector(IHubSpotClient client, IContactTypeManager typeManager)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _typeManager = typeManager ?? throw new ArgumentNullException(nameof(typeManager));
        }

        public async Task<TContact> GetAsync<TContact>(IContactSelector selector)
            where TContact : Contact, new()
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var properties = _typeManager.GetCustomProperties<TContact>(TypeManager.AllProperties).Select(p => new Property(p.FieldName)).ToArray();

            try
            {
                var hubspotContact = await selector.GetContact(_client, properties).ConfigureAwait(false);
                var contact = _typeManager.ConvertTo<TContact>(hubspotContact);
                return contact;
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        public async Task<TContact> SaveAsync<TContact>(TContact contact)
            where TContact : Contact, new()
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }

            var customProperties = (from property in _typeManager.GetPropertyData(contact)
                                      select new ValuedProperty(property.PropertyName, property.Value?.ToString())).ToArray();

            if (customProperties.All(cp => string.IsNullOrEmpty(cp.Value)) && IsNewContact())
            {
                return contact;
            }

            if (IsNewContact())
            {
                var newContact = await _client.Contacts.CreateAsync(customProperties);
                return _typeManager.ConvertTo<TContact>(newContact);
            }

            await _client.Contacts.UpdateByIdAsync(contact.Id, customProperties);
            var modifiedContact = await GetAsync<TContact>(SelectContact.ById(contact.Id));
            return modifiedContact;

            bool IsNewContact()
            {
                return contact.Id == 0 && contact.Created == default;
            }
        }

        public async Task<IReadOnlyList<TContact>> FindAsync<TContact>(IContactFilter filter = null)
            where TContact : Contact, new()
        {
            filter ??= FilterContacts.All;

            var properties = _typeManager.GetCustomProperties<TContact>(TypeManager.AllProperties).Select(p => new Property(p.FieldName)).ToArray();

            var matchingContacts = await filter.GetContacts(_client, properties);

            return matchingContacts.Select(_typeManager.ConvertTo<TContact>).ToArray();
        }
    }
}