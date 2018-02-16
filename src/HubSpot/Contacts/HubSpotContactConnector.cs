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
        private readonly IHubSpotContactClient _client;
        private readonly ITypeManager<Model.Contacts.Contact, Contact> _typeManager;
        private readonly ILogger<HubSpotContactConnector> _logger;

        public HubSpotContactConnector(IHubSpotContactClient client, ITypeManager<Model.Contacts.Contact, Contact> typeManager, ILogger<HubSpotContactConnector> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _typeManager = typeManager ?? throw new ArgumentNullException(nameof(typeManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TContact> GetByIdAsync<TContact>(long contactId)
            where TContact : Contact, new()
        {
            var properties = _typeManager.GetCustomProperties<TContact>(TypeManager.AllProperties).Select(p => new Property(p.metadata.PropertyName)).ToArray();

            var hubspotContact = await _client.GetByIdAsync(contactId, properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false).ConfigureAwait(false);

            var contact = _typeManager.ConvertFrom<TContact>(hubspotContact);

            return contact;
        }

        public async Task<TContact> GetByEmailAsync<TContact>(string email)
            where TContact : Contact, new()
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            var properties = _typeManager.GetCustomProperties<TContact>(TypeManager.AllProperties).Select(p => new Property(p.metadata.PropertyName)).ToArray();

            var hubspotContact = await _client.GetByEmailAsync(email, properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false).ConfigureAwait(false);

            var contact = _typeManager.ConvertFrom<TContact>(hubspotContact);

            return contact;
        }

        public async Task<TContact> GetByUserTokenAsync<TContact>(string userToken)
            where TContact : Contact, new()
        {
            if (userToken == null)
            {
                throw new ArgumentNullException(nameof(userToken));
            }

            var properties = _typeManager.GetCustomProperties<TContact>(TypeManager.AllProperties).Select(p => new Property(p.metadata.PropertyName)).ToArray();

            var hubspotContact = await _client.GetByUserTokenAsync(userToken, properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false).ConfigureAwait(false);

            var contact = _typeManager.ConvertFrom<TContact>(hubspotContact);

            return contact;
        }

        public async Task<TContact> SaveAsync<TContact>(TContact contact)
            where TContact : Contact, new()
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }

            var properties = from property in _typeManager.GetModifiedProperties(contact)
                             select new ValuedProperty(property.name, property.value);

            if (IsNewContact())
            {
                var newContact = await _client.CreateAsync(properties.ToArray());
                return _typeManager.ConvertFrom<TContact>(newContact);
            }
            else
            {
                await _client.UpdateByIdAsync(contact.Id, properties.ToArray());
                var newContact = await GetByIdAsync<TContact>(contact.Id);
                return newContact;
            }

            bool IsNewContact()
            {
                return contact.Id == 0;
            }
        }
    }
}