using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Model.Contacts
{
    public class PropertyList
    {
        [JsonProperty("properties")]
        public IReadOnlyList<ValuedProperty> Properties { get; set; }
    }

    public class CreateOrUpdateResponse
    {
        [JsonProperty("vid")]
        public long Id { get; set; }

        [JsonProperty("isNew")]
        public bool IsNew { get; set; }
    }

    public class DeleteContactResponse
    {
        [JsonProperty("vid")]
        public long Id { get; set; }

        [JsonProperty("deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}