using System;
using HubSpotContact = HubSpot.Model.Contacts.Contact;

namespace HubSpot.Contacts
{
    public class Contact : HubSpotEntity<HubSpotContact>
    {
        public Contact(HubSpotContact entity) : base(entity) { }

        [DefaultProperty(nameof(HubSpotContact.Id))]
        public long Id { get; set; }

        [DefaultProperty(nameof(HubSpotContact.PortalId))]
        public long PortalId { get; set; }

        [DefaultProperty(nameof(HubSpotContact.IsContact))]
        public bool IsContact { get; set; }

        [DefaultProperty(nameof(HubSpotContact.ProfileToken))]
        public string UserToken { get; set; }

        [DefaultProperty(nameof(HubSpotContact.ProfileUrl))]
        public string ProfileUrl { get; set; }

        [CustomProperty("email")]
        public string Email { get; set; }

        [CustomProperty("createdate")]
        public DateTimeOffset Created { get; set; }

        [CustomProperty("firstname")]
        public string FirstName { get; set; }

        [CustomProperty("lasttname")]
        public string LastName { get; set; }
    }
}
