using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;

namespace HubSpot.Contacts.Selectors {
    public class UserTokenContactSelector : IContactSelector
    {
        private readonly string _userToken;

        public UserTokenContactSelector(string userToken)
        {
            _userToken = userToken ?? throw new ArgumentNullException(nameof(userToken));
        }

        public Task<Model.Contacts.Contact> GetContact(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery)
        {
            return client.Contacts.GetByUserTokenAsync(_userToken, propertiesToQuery, PropertyMode.ValueOnly, FormSubmissionMode.None, false);
        }
    }
}