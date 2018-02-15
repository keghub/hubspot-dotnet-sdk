using System.Collections.Generic;
using System.Linq;
using HubSpot.Internal;

namespace HubSpot.Contacts
{
    public class ContactTypeManager<TEntity> : TypeManager<Model.Contacts.Contact, TEntity>
        where TEntity : Contact
    {
        public ContactTypeManager(ITypeStore typeStore) : base(typeStore) { }

        protected override IReadOnlyList<string> GetCustomProperties(Model.Contacts.Contact item) => item.Properties.Keys.ToArray();

        protected override bool HasCustomProperty(Model.Contacts.Contact item, string propertyName) => item.Properties.ContainsKey(propertyName);

        protected override string GetCustomPropertyValue(Model.Contacts.Contact item, string propertyName) => item.Properties.TryGetValue(propertyName, out var property) ? property.Value : null;
    }
}