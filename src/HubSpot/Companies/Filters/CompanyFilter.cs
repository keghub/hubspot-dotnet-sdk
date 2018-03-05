using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Companies.Filters
{
    public abstract class CompanyFilter : ICompanyFilter
    {
        public async Task<IReadOnlyList<Model.Companies.Company>> GetCompanies(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery)
        {
            long? offset = null;

            var result = new List<Model.Companies.Company>();

            (IReadOnlyList<Model.Companies.Company> list, bool hasMore, long? offset) tuple;

            do
            {
                tuple = await Get(client, propertiesToQuery, offset).ConfigureAwait(false);

                offset = tuple.offset;

                result.AddRange(tuple.list);

            } while (tuple.hasMore);

            return result;
        }

        protected abstract Task<(IReadOnlyList<Model.Companies.Company> list, bool hasMore, long? offset)> Get(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset);
    }
}
