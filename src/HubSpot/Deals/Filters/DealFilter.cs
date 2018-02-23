using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Deals.Filters {
    public abstract class DealFilter : IDealFilter
    {
        public async Task<IReadOnlyList<Model.Deals.Deal>> GetDeals(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery)
        {
            long? offset = null;

            var result = new List<Model.Deals.Deal>();

            (IReadOnlyList<Model.Deals.Deal> list, bool hasMore, long? offset) tuple;

            do
            {
                tuple = await GetDeals(client, propertiesToQuery, offset).ConfigureAwait(false);

                offset = tuple.offset;

                result.AddRange(tuple.list);

            } while (tuple.hasMore);

            return result;
        }

        protected abstract Task<(IReadOnlyList<Model.Deals.Deal> list, bool hasMore, long? offset)> GetDeals(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset);
    }
}