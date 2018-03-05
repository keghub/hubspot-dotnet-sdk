using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Companies.Filters {
    public class DomainCompanyFilter : CompanyFilter
    {
        private readonly string _domain;

        public DomainCompanyFilter(string domain)
        {
            _domain = domain ?? throw new ArgumentNullException(nameof(domain));
        }

        protected override async Task<(IReadOnlyList<Model.Companies.Company> list, bool hasMore, long? offset)> Get(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Companies.SearchAsync(_domain, propertiesToQuery, 25, offset);

            return (response.Companies, response.HasMore, response.Offset?.CompanyId);
        }
    }
}