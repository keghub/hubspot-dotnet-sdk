using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot.Model.LineItems
{
    public interface IHubSpotLineItemClient
    {
        Task<LineItem> GetAsync(long lineItemId, IReadOnlyList<IProperty> properties);

        Task<LineItem> GetBySKUAsync(IReadOnlyList<long> lineItemId, IReadOnlyList<IProperty> properties, string sku);
    }
}
