using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Companies.Selectors
{
    public class IdCompanySelector : ICompanySelector
    {
        private readonly long _companyId;

        public IdCompanySelector(long companyId)
        {
            _companyId = companyId;
        }
        public Task<Model.Companies.Company> GetCompany(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery)
        {
            return client.Companies.GetByIdAsync(_companyId);
        }
    }
}
