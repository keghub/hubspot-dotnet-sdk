using System;
using System.Text;
using HubSpot;
using Contact = HubSpot.Contacts.Contact;
using HubSpotContact = HubSpot.Model.Contacts.Contact;

namespace Tests.Contacts
{
    public class TestContact : HubSpot.Contacts.Contact
    {
        [CustomProperty("customProperty")]
        public string CustomProperty { get; set; }

        public TestContact(HubSpotContact entity) : base(entity) { }
    }
}
