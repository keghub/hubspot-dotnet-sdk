using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HubSpot.Model.Lists
{
    public class List
    {
        [JsonProperty("parentId")]
        public long ParentId { get; set; }

        [JsonProperty("dynamic")]
        public bool IsDynamic { get; set; }

        [JsonProperty("metaData")]
        public ContactListMetadata Metadata { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("portalId")]
        public int PortalId { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("listId")]
        public long ListId { get; set; }


        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("deleteable")]
        public bool IsDeleteable { get; set; }

        [JsonProperty("filters")]
        public IReadOnlyList<Filter> Filters { get; set; }
    }

    public class ContactListMetadata
    {
        [JsonProperty("processing")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ContactListStatus Processing { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("lastProcessingStateChangeAt")]
        public DateTimeOffset LastProcessingStateChangeAt { get; set; }

        [JsonProperty("lastSizeChangeAt")]
        public DateTimeOffset LastSizeChangeAt { get; set; }
    }

    public enum ContactListStatus
    {
        [EnumMember(Value = "DONE")] Done,
        [EnumMember(Value = "REFRESHING")] Refreshing,
        [EnumMember(Value = "INITIALIZING")] Initializing,
        [EnumMember(Value = "PROCESSING")] Processing
    }

    public class Filter
    {
        [JsonProperty("checkPastVersions")]
        public bool CheckPastVersions { get; set; }

        [JsonProperty("filterFamily")]
        public string FilterFamily { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("property")]
        public string Property { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

    }
}
