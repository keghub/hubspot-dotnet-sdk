using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.Model.LineItems
{
    public class LineItem
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("properties")]
        public IReadOnlyDictionary<string, object> Properties { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }
    }

    public class LineItemResult
    {
        [JsonProperty("results")]
        public IReadOnlyList<LineItem> LineItems { get; set; }
    }
}
