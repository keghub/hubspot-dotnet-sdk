using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Model.Deals
{
    public class Deal
    {
        [JsonProperty("dealId")]
        public long Id { get; set; }

        [JsonProperty("portalId")]
        public long PortalId { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("associations")]
        public IReadOnlyDictionary<string, IReadOnlyList<long>> Associations { get; set; }

        [JsonProperty("properties")]
        public IReadOnlyDictionary<string, VersionedProperty> Properties { get; set; }

        [JsonProperty("imports")]
        public IReadOnlyList<Import> Imports { get; set; }
    }

    public class VersionedProperty : IVersionedProperty
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("sourceId")]
        public string SourceId { get; set; }

        [JsonProperty("versions")]
        public IReadOnlyList<Companies.TimestampedValue> Versions { get; set; }

        IReadOnlyList<ITimestampedValue> IVersionedProperty.Versions => Versions;
    }

    public class TimestampedValue : ITimestampedValue
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("sourceId")]
        public string SourceId { get; set; }

        [JsonProperty("sourceVid")]
        public IReadOnlyList<long> SourceIds { get; set; }
    }

    public class Import
    {
        [JsonProperty("importKey")]
        public string ImportKey { get; set; }

        [JsonProperty("importDate")]
        public DateTimeOffset ImportDate { get; set; }
    }

    public class DealList
    {
        [JsonProperty("deals")]
        public IReadOnlyList<Deal> Deals { get; set; }

        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }

        [JsonProperty("offset")]
        public long? Offset { get; set; }
    }
}