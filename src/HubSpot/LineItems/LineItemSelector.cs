using HubSpot.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot.LineItems
{
    public class LineItemSelector : ILineItemSelector
    {
        private readonly long _lineItemId;
        private readonly IReadOnlyList<long> _lineItemIds;

        public LineItemSelector(long lineItemId)
        {
            _lineItemId = lineItemId;
        }

        public LineItemSelector(IReadOnlyList<long> lineItemIds)
        {
            _lineItemIds = lineItemIds;
        }
        public async Task<Model.LineItems.LineItem> GetLineItemAsync(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery)
        {
            return await client.LineItems.GetAsync(_lineItemId, propertiesToQuery);
        }

        public async Task<Model.LineItems.LineItem> GetLineItemBySKUAsync(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, string sku)
        {
            return await client.LineItems.GetBySKUAsync(_lineItemIds, propertiesToQuery, sku);
        }
    }
}
