using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.LineItems
{
    public class LineItem : IHubSpotEntity
    {
        [DefaultProperty("hs_object_id")]
        public long Id { get; set; }

        [CustomProperty("createdate")]
        public DateTime CreateDate { get; set; }

        [CustomProperty("hs_lastmodifieddate")]
        public DateTime LastUpdated { get; set; }

        [CustomProperty("name")]
        public string Name { get; set; }

        [CustomProperty("hs_sku")]
        public string SKU { get; set; }

        [CustomProperty("hs_term_in_months")]
        public int MonthsTerm { get; set; }  
        
        [CustomProperty("hs_recurring_billing_start_date")]
        public DateTime? StartDate { get; set; }

        [CustomProperty("quantity")]
        public int Quantity { get; set; }

        [CustomProperty("price")]
        public decimal Price { get; set; }

        [CustomProperty("discount")]
        public decimal Discount { get; set; }

        [CustomProperty("amount")]
        public decimal NetPrice { get; set; }

        [CustomProperty("hs_margin_mrr")]
        public decimal MRR { get; set; }  
        
        [CustomProperty("hs_arr")]
        public decimal ARR { get; set; }   
        
        [CustomProperty("hs_margin_tcv")]
        public decimal TCV { get; set; }
       
        [CustomProperty("hs_line_item_currency_code")]
        public string Currency { get; set; }

        IReadOnlyDictionary<string, object> IHubSpotEntity.Properties { get; set; } = new Dictionary<string, object>();
    }
}
