﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot.LineItems
{
    public static class SelectLineItem
    {
        public static ILineItemSelector ById(long lineItemId) => new LineItemSelector(lineItemId);
        public static ILineItemSelector ByIdRange(IReadOnlyList<long> lineItemIds) => new LineItemSelector(lineItemIds);
    }

    public static class LineItemSelectorExtensions
    {
        public static Task<TLineItem> GetByIdAsync<TLineItem>(this IHubSpotLineItemConnector connector, long lineItemId) where TLineItem : LineItem, new()
            => connector.GetAsync<TLineItem>(SelectLineItem.ById(lineItemId));

        public static Task<TLineItem> GetBySKUAsync<TLineItem>(this IHubSpotLineItemConnector connector, IReadOnlyList<long> lineItemIds, string sku) where TLineItem : LineItem, new()
            => connector.GetBySKUAsync<TLineItem>(SelectLineItem.ByIdRange(lineItemIds), sku);
    }
}
