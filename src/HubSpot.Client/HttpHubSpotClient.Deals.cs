using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Deals;
using HubSpot.Model.LineItems;
using HubSpot.Utils;
using Kralizek.Extensions.Http;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotDealClient
    {
        async Task<Deal> IHubSpotDealClient.CreateAsync(IReadOnlyList<long> associatedContactIds, IReadOnlyList<long> associatedCompanyIds, IReadOnlyList<ValuedPropertyV2> properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            var request = new
            {
                associations = new
                {
                    associatedCompanyIds,
                    associatedVids = associatedContactIds
                },
                properties
            };

            var response = await _client.PostAsync<object, Deal>("/deals/v1/deal/", request);

            return response;
        }

        async Task<Deal> IHubSpotDealClient.UpdateAsync(long dealId, IReadOnlyList<ValuedPropertyV2> propertiesToUpdate)
        {
            if (propertiesToUpdate == null)
            {
                throw new ArgumentNullException(nameof(propertiesToUpdate));
            }

            var list = new PropertyList<ValuedPropertyV2> { Properties = propertiesToUpdate };
            var result = await _client.PutAsync<PropertyList<ValuedPropertyV2>, Deal>($"/deals/v1/deal/{dealId}", list);

            return result;
        }

        async Task IHubSpotDealClient.UpdateManyAsync(IReadOnlyList<ObjectPropertyList<ValuedPropertyV2>> dealsToUpdate)
        {
            if (dealsToUpdate == null)
            {
                throw new ArgumentNullException(nameof(dealsToUpdate));
            }

            await _client.SendAsync<IReadOnlyList<ObjectPropertyList<ValuedPropertyV2>>>(HttpMethod.Post, "/deals/v1/batch-async/update", dealsToUpdate);
        }

        async Task IHubSpotDealClient.DeleteAsync(long dealId)
        {
            await _client.DeleteAsync($"/deals/v1/deal/{dealId}");
        }

        async Task<Deal> IHubSpotDealClient.GetByIdAsync(long dealId, bool includePropertyVersions)
        {
            var builder = new HttpQueryStringBuilder();
            builder.Add("includePropertyVersions", includePropertyVersions);

            try
            {
                var result = await _client.GetAsync<Deal>($"/deals/v1/deal/{dealId}", builder.BuildQuery());
                return result;
            }
            catch (HttpException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException("Deal not found", ex);
            }
        }

        async Task IHubSpotDealClient.AssociateContactsAsync(long dealId, IReadOnlyList<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return;
            }

            var builder = new HttpQueryStringBuilder();
            foreach (var id in ids)
            {
                builder.Add("id", id);
            }

            await _client.PutAsync($"/deals/v1/deal/{dealId}/associations/CONTACT", query: builder.BuildQuery());
        }

        async Task IHubSpotDealClient.RemoveAssociationToContactsAsync(long dealId, IReadOnlyList<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return;
            }

            var builder = new HttpQueryStringBuilder();
            foreach (var id in ids)
            {
                builder.Add("id", id);
            }

            await _client.DeleteAsync($"/deals/v1/deal/{dealId}/associations/CONTACT", query: builder.BuildQuery());
        }

        async Task IHubSpotDealClient.AssociateCompaniesAsync(long dealId, IReadOnlyList<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return;
            }

            var builder = new HttpQueryStringBuilder();
            foreach (var id in ids)
            {
                builder.Add("id", id);
            }

            await _client.PutAsync($"/deals/v1/deal/{dealId}/associations/COMPANY", query: builder.BuildQuery());
        }

        async Task IHubSpotDealClient.RemoveAssociationToCompaniesAsync(long dealId, IReadOnlyList<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return;
            }

            var builder = new HttpQueryStringBuilder();
            foreach (var id in ids)
            {
                builder.Add("id", id);
            }

            await _client.DeleteAsync($"/deals/v1/deal/{dealId}/associations/COMPANY", query: builder.BuildQuery());
        }

        async Task<DealList> IHubSpotDealClient.GetAllAsync(IReadOnlyList<IProperty> properties, IReadOnlyList<IProperty> propertiesWithHistory, bool includeAssociations, int limit, long? offset)
        {
            var builder = new HttpQueryStringBuilder();
            builder.AddProperties(properties, "properties");
            builder.AddProperties(propertiesWithHistory, "propertiesWithHistory");
            builder.Add("limit", limit.ToString());
            builder.Add("includeAssociations", includeAssociations);

            if (offset.HasValue)
                builder.Add("offset", offset.Value.ToString());

            var result = await _client.GetAsync<DealList>("/deals/v1/deal/paged", builder.BuildQuery());

            return result;
        }

        async Task<PagedList<Deal>> IHubSpotDealClient.GetRecentlyUpdatedAsync(DateTimeOffset? since, bool includePropertyVersions, int count, long? offset)
        {
            var builder = new HttpQueryStringBuilder();
            builder.Add("count", count.ToString());
            builder.Add("includePropertyVersions", includePropertyVersions);

            if (since.HasValue)
                builder.Add("since", since.Value.ToUnixTimeMilliseconds());

            if (offset.HasValue)
                builder.Add("offset", offset.Value.ToString());

            var result = await _client.GetAsync<PagedList<Deal>>("/deals/v1/deal/recent/created", builder.BuildQuery());

            return result;
        }

        async Task<PagedList<Deal>> IHubSpotDealClient.GetRecentlyCreatedAsync(DateTimeOffset? since, bool includePropertyVersions, int count, long? offset)
        {
            var builder = new HttpQueryStringBuilder();
            builder.Add("count", count.ToString());
            builder.Add("includePropertyVersions", includePropertyVersions);

            if (since.HasValue)
                builder.Add("since", since.Value.ToUnixTimeMilliseconds());

            if (offset.HasValue)
                builder.Add("offset", offset.Value.ToString());

            var result = await _client.GetAsync<PagedList<Deal>>("/deals/v1/deal/recent/modified", builder.BuildQuery());

            return result;
        }

        async Task<DealList> IHubSpotDealClient.FindByContactAsync(long contactId, IReadOnlyList<IProperty> properties, IReadOnlyList<IProperty> propertiesWithHistory, bool includeAssociations, int limit, long? offset)
        {
            var builder = new HttpQueryStringBuilder();
            builder.AddProperties(properties, "properties");
            builder.AddProperties(propertiesWithHistory, "propertiesWithHistory");
            builder.Add("limit", limit.ToString());
            builder.Add("includeAssociations", includeAssociations);

            if (offset.HasValue)
                builder.Add("offset", offset.Value.ToString());

            var result = await _client.GetAsync<DealList>($"/deals/v1/deal/associated/CONTACT/{contactId}/paged", builder.BuildQuery());

            return result;
        }

        async Task<DealList> IHubSpotDealClient.FindByCompanyAsync(long companyId, IReadOnlyList<IProperty> properties, IReadOnlyList<IProperty> propertiesWithHistory, bool includeAssociations, int limit, long? offset)
        {
            var builder = new HttpQueryStringBuilder();
            builder.AddProperties(properties, "properties");
            builder.AddProperties(propertiesWithHistory, "propertiesWithHistory");
            builder.Add("limit", limit.ToString());
            builder.Add("includeAssociations", includeAssociations);

            if (offset.HasValue)
                builder.Add("offset", offset.Value.ToString());

            var result = await _client.GetAsync<DealList>($"/deals/v1/deal/associated/COMPANY/{companyId}/paged", builder.BuildQuery());

            return result;
        }

        async Task<LineItemAssociationList> IHubSpotDealClient.GetLineItemAssociationsAsync(long dealId)
        {
            try
            {
                var result = await _client.GetAsync<LineItemAssociationList>($"/crm/v3/objects/deals/{dealId}/associations/line_items");
                return result;
            }
            catch (HttpException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException("Deal not found", ex);
            }
        }
    }
}