using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.Model.LineItems
{
    public class LineItemAssociation
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class LineItemAssociationList
    {
        [JsonProperty("results")]
        public IReadOnlyList<LineItemAssociation> LineItemAssociations { get; set; }
    }
}
