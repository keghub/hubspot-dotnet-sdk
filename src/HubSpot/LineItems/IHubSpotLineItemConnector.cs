﻿using HubSpot.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot.LineItems
{
    public interface IHubSpotLineItemConnector
    {
        Task<TLineItem> GetAsync<TLineItem>(ILineItemSelector selector, string[] customProperties) where TLineItem : LineItem, new();
        Task<TLineItem> GetBySKUAsync<TLineItem>(ILineItemSelector selector, string sku, string[] customProperties) where TLineItem : LineItem, new();
    }
}
