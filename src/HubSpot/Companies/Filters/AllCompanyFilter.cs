using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Companies.Filters {
    public class AllCompanyFilter : CompanyFilter
    {
        protected override async Task<(IReadOnlyList<Model.Companies.Company> list, bool hasMore, long? offset)> Get(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Companies.GetAllAsync(propertiesToQuery, limit: 25, companyOffset: offset);

            return (response.Companies, response.HasMore, response.Offset);
        }
    }
}