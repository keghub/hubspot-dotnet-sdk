using HubSpot.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HubSpotLineItem = HubSpot.Model.LineItems.LineItem;

namespace HubSpot.LineItems
{
    public interface ILineItemSelector
    {
        Task<HubSpotLineItem> GetLineItemAsync(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery);
        Task<HubSpotLineItem> GetLineItemBySKUAsync(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, string sku);
    }
}
