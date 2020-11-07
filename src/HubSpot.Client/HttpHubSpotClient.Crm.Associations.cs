using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model.CRM.Associations;
using Kralizek.Extensions.Http;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotCrmAssociationClient
    {
        async Task<AssociationIdList> IHubSpotCrmAssociationClient.GetAllAsync(long objectId, AssociationType associationType, int limit, long? offset)
        {
            if (limit > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), "Up to 100 items can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();

            builder.Add("limit", limit);

            if (offset.HasValue)
            {
                builder.Add("offset", offset.Value);
            }

            string path = $"/crm-associations/v1/associations/{objectId}/HUBSPOT_DEFINED/{associationType.Id}";
            IQueryString query = builder.BuildQuery();
            var list = await _client.SendAsync<AssociationIdList>(HttpMethod.Get, path, query);

            return list;
        }

        async Task IHubSpotCrmAssociationClient.CreateAsync(Association association)
        {
            if (association == null)
            {
                throw new ArgumentNullException(nameof(association));
            }

            await _client.SendAsync<Association>(HttpMethod.Put, "/crm-associations/v1/associations", association, null);
        }

        async Task IHubSpotCrmAssociationClient.CreateManyAsync(IReadOnlyList<Association> associations)
        {
            if (associations == null)
            {
                throw new ArgumentNullException(nameof(associations));
            }

            if (associations.Count == 0)
            {
                return;
            }

            if (associations.Count > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(associations), "Up to 100 items can be created in the same request");
            }

            await _client.SendAsync<IReadOnlyList<Association>>(HttpMethod.Put, "/crm-associations/v1/associations/create-batch", associations, null);
        }

        async Task IHubSpotCrmAssociationClient.DeleteAsync(Association association)
        {
            if (association == null)
            {
                throw new ArgumentNullException(nameof(association));
            }

            await _client.SendAsync<Association>(HttpMethod.Put, "/crm-associations/v1/associations/delete", association, null);

        }

        async Task IHubSpotCrmAssociationClient.DeleteManyAsync(IReadOnlyList<Association> associations)
        {
            if (associations == null)
            {
                throw new ArgumentNullException(nameof(associations));
            }

            if (associations.Count == 0)
            {
                return;
            }

            if (associations.Count > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(associations), "Up to 100 items can be deleted in the same request");
            }

            await _client.SendAsync<IReadOnlyList<Association>>(HttpMethod.Put, "/crm-associations/v1/associations/delete-batch", associations, null);
        }
    }
}