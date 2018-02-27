using HubSpot;

namespace Tests.Contacts
{
    public class TestContact : HubSpot.Contacts.Contact
    {
        [CustomProperty("customProperty")]
        public string CustomProperty { get; set; }
    }
}