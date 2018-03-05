using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Companies.Selectors;
using HubSpot.Model;

namespace HubSpot.Companies
{
    public interface ICompanySelector
    {
        Task<Model.Companies.Company> GetCompany(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery);
    }

    public static class SelectCompany
    {
        public static ICompanySelector ById(long companyId) => new IdCompanySelector(companyId);
    }

    public static class SelectCompanyExtensions
    {
        public static Task<TCompany> GetByIdAsync<TCompany>(this IHubSpotCompanyConnector connector, long companyId)
            where TCompany : Company, new()
            => connector.GetAsync<TCompany>(SelectCompany.ById(companyId));

        public static Task<Company> GetByIdAsync(this IHubSpotCompanyConnector connector, long companyId)
            => GetByIdAsync<Company>(connector, companyId);
    }
}