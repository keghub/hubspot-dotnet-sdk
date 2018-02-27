using System.Collections.Generic;
using System.Linq;
using HubSpot.Internal;
using HubSpotContact = HubSpot.Model.Contacts.Contact;

namespace HubSpot.Contacts
{
    public interface IContactTypeManager : ITypeManager<HubSpotContact, Contact> { }

    public class ContactTypeManager : TypeManager<HubSpotContact, Contact>, IContactTypeManager
    {
        public ContactTypeManager(ITypeStore typeStore) : base(typeStore) { }

        protected override IReadOnlyList<KeyValuePair<string, string>> GetCustomProperties(HubSpotContact item)
        {
            var properties = from kvp in item.Properties
                             let key = kvp.Key
                             let value = kvp.Value.Value
                             select new KeyValuePair<string, string>(key, value);

            return properties.ToArray();
        }

        protected override IReadOnlyList<KeyValuePair<string, object>> GetDefaultProperties(HubSpotContact item)
        {
            return new[]
            {
                new KeyValuePair<string, object>("vid", item.Id),
                new KeyValuePair<string, object>("canonical-vid", item.CanonicalId),
                new KeyValuePair<string, object>("is-contact", item.IsContact),
                new KeyValuePair<string, object>("portal-id", item.PortalId),
                new KeyValuePair<string, object>("merged-vids", item.MergedIds),
                new KeyValuePair<string, object>("profile-token", item.ProfileToken),
                new KeyValuePair<string, object>("profile-url", item.ProfileUrl)
            };
        }
    }
}