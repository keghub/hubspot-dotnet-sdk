using AutoFixture;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Internal;

namespace Tests.Contacts
{
    public class TestContact : HubSpot.Contacts.Contact
    {
        [CustomProperty("customProperty")]
        public string CustomProperty { get; set; }
    }

    public class ContactAutoDataAttribute : AutoDataAttribute
    {
        public ContactAutoDataAttribute() : base(CreateFixture)
        {
            
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize<CustomPropertyInfo>(c => c.Without(p => p.ValueAccessor));

            return fixture;
        }
    }
}