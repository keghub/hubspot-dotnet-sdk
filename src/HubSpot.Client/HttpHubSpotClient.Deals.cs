using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Deals;
using HubSpot.Utils;
using Kralizek.Extensions.Http;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotDealClient
    {
        async Task<Deal> IHubSpotDealClient.CreateAsync(IReadOnlyList<long> associatedContactIds, IReadOnlyList<long> associatedCompanyIds, IReadOnlyList<ValuedProperty> properties)
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

            var response = await SendAsync<object, Deal>(HttpMethod.Post, "/deals/v1/deal/", request);

            return response;
        }

        async Task<Deal> IHubSpotDealClient.UpdateAsync(long dealId, IReadOnlyList<ValuedProperty> propertiesToUpdate)
        {
            if (propertiesToUpdate == null)
            {
                throw new ArgumentNullException(nameof(propertiesToUpdate));
            }

            var list = new PropertyList { Properties = propertiesToUpdate };
            var result = await SendAsync<PropertyList, Deal>(HttpMethod.Put, $"/deals/v1/deal/{dealId}", list);

            return result;
        }

        async Task IHubSpotDealClient.UpdateManyAsync(IReadOnlyList<ObjectPropertyList> dealsToUpdate)
        {
            if (dealsToUpdate == null)
            {
                throw new ArgumentNullException(nameof(dealsToUpdate));
            }

            await SendAsync(HttpMethod.Post, "/deals/v1/batch-async/update", dealsToUpdate);
        }

        async Task IHubSpotDealClient.DeleteAsync(long dealId)
        {
            await SendAsync(HttpMethod.Delete, $"/deals/v1/deal/{dealId}");
        }

        async Task<Deal> IHubSpotDealClient.GetByIdAsync(long dealId, bool includePropertyVersions)
        {
            var builder = new HttpQueryStringBuilder();
            builder.Add("includePropertyVersions", includePropertyVersions);

            var result = await SendAsync<Deal>(HttpMethod.Get, $"/deals/v1/deal/{dealId}", builder.BuildQuery());

            return result;
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

            await SendAsync(HttpMethod.Put, $"/deals/v1/deal/{dealId}/associations/CONTACT", builder.BuildQuery());
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

            await SendAsync(HttpMethod.Delete, $"/deals/v1/deal/{dealId}/associations/CONTACT", builder.BuildQuery());
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

            await SendAsync(HttpMethod.Put, $"/deals/v1/deal/{dealId}/associations/COMPANY", builder.BuildQuery());

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

            await SendAsync(HttpMethod.Delete, $"/deals/v1/deal/{dealId}/associations/COMPANY", builder.BuildQuery());

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

            var result = await SendAsync<DealList>(HttpMethod.Get, "/deals/v1/deal/paged", builder.BuildQuery());

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

            var result = await SendAsync<PagedList<Deal>>(HttpMethod.Get, "/deals/v1/deal/recent/created", builder.BuildQuery());

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

            var result = await SendAsync<PagedList<Deal>>(HttpMethod.Get, "/deals/v1/deal/recent/modified", builder.BuildQuery());

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

            var result = await SendAsync<DealList>(HttpMethod.Get, $"/deals/v1/deal/associated/CONTACT/{contactId}/paged", builder.BuildQuery());

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

            var result = await SendAsync<DealList>(HttpMethod.Get, $"/deals/v1/deal/associated/COMPANY/{companyId}/paged", builder.BuildQuery());

            return result;

        }
    }
}