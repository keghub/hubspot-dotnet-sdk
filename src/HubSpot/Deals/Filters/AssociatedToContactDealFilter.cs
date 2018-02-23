using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Deals.Filters {
    public class AssociatedToContactDealFilter : DealFilter
    {
        private readonly long _contactId;

        public AssociatedToContactDealFilter(long contactId)
        {
            _contactId = contactId;
        }

        protected override async Task<(IReadOnlyList<Model.Deals.Deal> list, bool hasMore, long? offset)> GetDeals(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Deals.FindByContactAsync(_contactId, propertiesToQuery, limit: 25, offset: offset);

            return (response.Deals, response.HasMore, response.Offset);
        }
    }
}