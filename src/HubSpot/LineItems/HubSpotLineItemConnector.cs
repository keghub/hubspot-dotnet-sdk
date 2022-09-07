using HubSpot.Internal;
using HubSpot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot.LineItems
{
    public class HubSpotLineItemConnector : IHubSpotLineItemConnector
    {
        private readonly IHubSpotClient _hubSpotClient;
        private readonly ILineItemTypeManager _lineItemTypeManager;

        public HubSpotLineItemConnector(IHubSpotClient hubSpotClient, ILineItemTypeManager lineItemTypeManager)
        {
            _hubSpotClient = hubSpotClient ?? throw new ArgumentNullException(nameof(hubSpotClient));
            _lineItemTypeManager = lineItemTypeManager ?? throw new ArgumentNullException(nameof(lineItemTypeManager));
        }
        public async Task<TLineItem> GetAsync<TLineItem>(ILineItemSelector selector, string[] customProperties) where TLineItem : LineItem, new()
        {
            if(selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            try
            {
                var properties = _lineItemTypeManager.GetCustomProperties<TLineItem>(TypeManager.AllProperties).Select(p => new Property(p.FieldName)).ToList();
                if (customProperties?.Count() > 0)
                {
                    properties.AddRange(customProperties.Select(x => new Property(x)));
                }
                var hubSpotLineItem = await selector.GetLineItemAsync(_hubSpotClient, properties).ConfigureAwait(false);
                var lineItem = _lineItemTypeManager.ConvertTo<TLineItem>(hubSpotLineItem);
                return lineItem;
            }
            catch(NotFoundException)
            {
                return null;
            }
        }

        public async Task<TLineItem> GetBySKUAsync<TLineItem>(ILineItemSelector selector, string sku, string[] customProperties) where TLineItem : LineItem, new()
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            try
            {
                var properties = _lineItemTypeManager.GetCustomProperties<TLineItem>(TypeManager.AllProperties).Select(p => new Property(p.FieldName)).ToList();
                if (customProperties?.Count() > 0)
                {
                    properties.AddRange(customProperties.Select(x => new Property(x)));
                }

                var hubSpotLineItem = await selector.GetLineItemBySKUAsync(_hubSpotClient, properties, sku).ConfigureAwait(false);
                var lineItem = _lineItemTypeManager.ConvertTo<TLineItem>(hubSpotLineItem);
                return lineItem;
            }
            catch (NotFoundException)
            {
                return null;
            }
        }
    }
}
