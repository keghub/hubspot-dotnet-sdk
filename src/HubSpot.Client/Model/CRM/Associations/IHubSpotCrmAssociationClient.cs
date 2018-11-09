using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HubSpot.Model.Companies;
using Newtonsoft.Json;

namespace HubSpot.Model.CRM.Associations
{
    public interface IHubSpotCrmAssociationClient
    {
        Task<AssociationIdList> GetAllAsync(long objectId, AssociationType associationType, int limit = 10, long? offset = null);

        Task CreateAsync(Association association);

        Task CreateManyAsync(IReadOnlyList<Association> associations);

        Task DeleteAsync(Association association);

        Task DeleteManyAsync(IReadOnlyList<Association> associations);
    }


    public class AssociationIdList
    {
        [JsonProperty("results")]
        public IReadOnlyList<long> Results { get; set; }

        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }
    }
}
