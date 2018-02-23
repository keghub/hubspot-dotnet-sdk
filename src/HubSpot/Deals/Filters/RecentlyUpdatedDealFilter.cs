using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Deals.Filters {
    public class RecentlyUpdatedDealFilter : DealFilter
    {
        protected override async Task<(IReadOnlyList<Model.Deals.Deal> list, bool hasMore, long? offset)> GetDeals(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Deals.GetRecentlyUpdatedAsync(count: 20, offset: offset);

            return (response.Results, response.HasMore, response.Offset);
        }
    }
}