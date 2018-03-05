using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Companies.Filters {
    public class RecentlyCreatedCompanyFilter : CompanyFilter
    {
        protected override async Task<(IReadOnlyList<Model.Companies.Company> list, bool hasMore, long? offset)> Get(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Companies.GetRecentlyCreatedAsync(count: 25, offset: offset);

            return (response.Results, response.HasMore, response.Offset);
        }
    }
}