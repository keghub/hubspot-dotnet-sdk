using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Model.Companies
{
    public class DeleteCompanyResponse
    {
        [JsonProperty("companyId")]
        public long CompanyId { get; set; }

        [JsonProperty("deleted")]
        public bool IsDeleted { get; set; }
    }

    public class CompanyList
    {
        [JsonProperty("companies")]
        public IReadOnlyList<Company> Companies { get; set; }

        [JsonProperty("has-more")]
        public bool HasMore { get; set; }

        [JsonProperty("offset")]
        public long? Offset { get; set; }
    }

    public class SearchResponse
    {
        [JsonProperty("results")]
        public IReadOnlyList<Company> Companies { get; set; }

        [JsonProperty("has-more")]
        public bool HasMore { get; set; }

        [JsonProperty("offset")]
        public SearchResponseOffset Offset { get; set; }

        public class SearchResponseOffset
        {
            [JsonProperty("companyId", DefaultValueHandling = DefaultValueHandling.Include)]
            public long CompanyId { get; set; }

            [JsonProperty("isPrimary")]
            public bool IsPrimary { get; set; }
        }
    }
}