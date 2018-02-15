using System;
using System.Linq;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Microsoft.Extensions.Logging;

namespace HubSpot.Contacts {
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
            where TContact : Contact
        {
            var properties = _typeManager.GetCustomProperties<TContact>().Select(p => new Property(p)).ToArray();

            var hubspotContact = await _client.GetByIdAsync(contactId, properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false).ConfigureAwait(false);

            var contact = await _typeManager.TransformAsync<TContact>(hubspotContact).ConfigureAwait(false);

            return contact;
        }

        public async Task<TContact> GetByEmailAsync<TContact>(string email)
            where TContact : Contact
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            var properties = _typeManager.GetCustomProperties<TContact>().Select(p => new Property(p)).ToArray();

            var hubspotContact = await _client.GetByEmailAsync(email, properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false).ConfigureAwait(false);

            var contact = await _typeManager.TransformAsync<TContact>(hubspotContact).ConfigureAwait(false);

            return contact;
        }
        public async Task<TContact> GetByUserTokenAsync<TContact>(string userToken)
            where TContact : Contact
        {
            if (userToken == null)
            {
                throw new ArgumentNullException(nameof(userToken));
            }

            var properties = _typeManager.GetCustomProperties<TContact>().Select(p => new Property(p)).ToArray();

            var hubspotContact = await _client.GetByUserTokenAsync(userToken, properties, PropertyMode.ValueOnly, FormSubmissionMode.None, false).ConfigureAwait(false);

            var contact = await _typeManager.TransformAsync<TContact>(hubspotContact).ConfigureAwait(false);

            return contact;
        }

        public async Task<TContact> CreateAsync<TContact>()
            where TContact : Contact
        {
            throw new System.NotImplementedException();
        }

        public async Task SaveAsync<TContact>(TContact contact)
            where TContact : Contact
        {
            throw new System.NotImplementedException();
        }
    }
}