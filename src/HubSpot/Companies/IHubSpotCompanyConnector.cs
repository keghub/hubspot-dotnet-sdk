using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Companies
{
    public interface IHubSpotCompanyConnector
    {
        Task<TCompany> GetAsync<TCompany>(ICompanySelector selector)
            where TCompany : Company, new();

        Task<TCompany> SaveAsync<TCompany>(TCompany company)
            where TCompany : Company, new();

        Task<bool> UpdatePropertiesAsync(long id, IDictionary<string, string> properties);

        Task<IReadOnlyList<TCompany>> FindAsync<TCompany>(ICompanyFilter filter = null)
            where TCompany : Company, new();

    }

    public static class HubSpotCompanyConnectorExtensions
    {
        public static Task<Company> GetAsync(this IHubSpotCompanyConnector connector, ICompanySelector selector) => connector.GetAsync<Company>(selector);

        public static Task<Company> SaveAsync(this IHubSpotCompanyConnector connector, Company company) => connector.SaveAsync(company);

        public static Task<IReadOnlyList<Company>> FindAsync(this IHubSpotCompanyConnector connector, ICompanyFilter filter = null) => connector.FindAsync<Company>(filter);
    }
}