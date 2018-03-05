using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;

namespace HubSpot.Contacts.Selectors {
    public class EmailContactSelector : IContactSelector
    {
        private readonly string _email;

        public EmailContactSelector(string email)
        {
            _email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public  Task<Model.Contacts.Contact> GetContact(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery)
        {
            return client.Contacts.GetByEmailAsync(_email, propertiesToQuery, PropertyMode.ValueOnly, FormSubmissionMode.None, false);
        }
    }
}