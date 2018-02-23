using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Deals.Filters {
    public class AssociatedToCompanyDealFilter : DealFilter
    {
        private readonly long _companyId;

        public AssociatedToCompanyDealFilter(long companyId)
        {
            _companyId = companyId;
        }

        protected override async Task<(IReadOnlyList<Model.Deals.Deal> list, bool hasMore, long? offset)> GetDeals(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Deals.FindByCompanyAsync(_companyId, propertiesToQuery, limit: 25, offset: offset);

            return (response.Deals, response.HasMore, response.Offset);
        }
    }
}