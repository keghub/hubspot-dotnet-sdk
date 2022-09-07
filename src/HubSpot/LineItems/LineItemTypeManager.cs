using HubSpot.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HubSpot.LineItems
{
    public class LineItemTypeManager : TypeManager<Model.LineItems.LineItem, LineItem>, ILineItemTypeManager
    {
        public LineItemTypeManager(ITypeStore typeStore) : base(typeStore)
        {

        }

        protected override IReadOnlyList<KeyValuePair<string, string>> GetCustomProperties(Model.LineItems.LineItem item)
        {
            var properties = item.Properties.Select(x => new KeyValuePair<string, string>(x.Key, x.Value?.ToString()));
            return properties.ToArray();
        }

        protected override IReadOnlyList<KeyValuePair<string, object>> GetDefaultProperties(Model.LineItems.LineItem item)
        {
            return new[]
            {
                new KeyValuePair<string, object>("hs_object_id", item.Id),
            };
        }
    }
}
