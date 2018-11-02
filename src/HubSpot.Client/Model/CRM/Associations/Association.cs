using Newtonsoft.Json;

namespace HubSpot.Model.CRM.Associations
{
    public class Association
    {
        [JsonProperty("fromObjectId")]
        public long FromId { get; set; }

        [JsonProperty("toObjectId")]
        public long ToId { get; set; }

        [JsonProperty("definitionId")]
        [JsonConverter(typeof(AssociationTypeConverter))]
        public AssociationType AssociationType { get; set; }

        [JsonProperty("category")]
        public string Category { get; private set; } = "HUBSPOT_DEFINED";
    }
}