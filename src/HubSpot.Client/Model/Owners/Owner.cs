using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HubSpot.Model.Owners {
    public class Owner
    {
        [JsonProperty("ownerId")]
        public long Id { get; set; }

        [JsonProperty("portalId")]
        public long PortalId { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OwnerType Type { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public enum OwnerType
    {
        [EnumMember(Value = "PERSON")]
        Person = 1
    }
}