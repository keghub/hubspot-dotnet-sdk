using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Deals.Filters;
using HubSpot.Model;
using HubSpot.Model.Deals;
using HubSpotDeal = HubSpot.Model.Deals.Deal;

namespace HubSpot.Deals
{

    namespace Filters
    {
        public class AllDealFilter : DealFilter
        {
            protected override async Task<(IReadOnlyList<HubSpotDeal> list, bool hasMore, long? offset)> GetDeals(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
            {
                var response = await client.Deals.GetAllAsync(propertiesToQuery, limit: 25, offset: offset).ConfigureAwait(false);

                return (response.Deals, response.HasMore, response.Offset);
            }
        }
    }
}