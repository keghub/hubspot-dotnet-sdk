using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Model.Companies
{
    public class Company
    {
        [JsonProperty("companyId")]
        public long Id { get; set; }

        [JsonProperty("portalId")]
        public int PortalId { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("properties")]
        public IReadOnlyDictionary<string, VersionedProperty> Properties { get; set; }
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
        public IReadOnlyList<TimestampedValue> Versions { get; set; }

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
}