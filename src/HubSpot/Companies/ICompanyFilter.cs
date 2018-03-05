using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Companies.Filters;
using HubSpot.Model;

namespace HubSpot.Companies
{
    public interface ICompanyFilter
    {
        Task<IReadOnlyList<Model.Companies.Company>> GetCompanies(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery);
    }

    public static class FilterCompanies
    {
        public static ICompanyFilter All => new AllCompanyFilter();

        public static ICompanyFilter RecentlyUpdated => new RecentlyUpdatedCompanyFilter();

        public static ICompanyFilter RecentlyCreated => new RecentlyCreatedCompanyFilter();

        public static ICompanyFilter ByDomain(string domain) => new DomainCompanyFilter(domain);
    }

    public static class FilterCompaniesExtensions
    {
        public static Task<IReadOnlyList<TCompany>> FindAllAsync<TCompany>(this IHubSpotCompanyConnector connector)
            where TCompany : Company, new()
            => connector.FindAsync<TCompany>(FilterCompanies.All);

        public static Task<IReadOnlyList<TCompany>> FindRecentlyUpdatedAsync<TCompany>(this IHubSpotCompanyConnector connector)
            where TCompany : Company, new()
            => connector.FindAsync<TCompany>(FilterCompanies.RecentlyUpdated);

        public static Task<IReadOnlyList<TCompany>> FindRecentlyCreatedAsync<TCompany>(this IHubSpotCompanyConnector connector)
            where TCompany : Company, new()
            => connector.FindAsync<TCompany>(FilterCompanies.RecentlyCreated);

        public static Task<IReadOnlyList<TCompany>> FindByDomainAsync<TCompany>(this IHubSpotCompanyConnector connector, string domain)
            where TCompany : Company, new()
            => connector.FindAsync<TCompany>(FilterCompanies.ByDomain(domain));

        public static Task<IReadOnlyList<Company>> FindAllAsync(this IHubSpotCompanyConnector connector) => FindAllAsync<Company>(connector);

        public static Task<IReadOnlyList<Company>> FindRecentlyUpdatedAsync(this IHubSpotCompanyConnector connector) => FindRecentlyUpdatedAsync<Company>(connector);

        public static Task<IReadOnlyList<Company>> FindRecentlyCreatedAsync(this IHubSpotCompanyConnector connector) => FindRecentlyCreatedAsync<Company>(connector);

        public static Task<IReadOnlyList<Company>> FindByDomainAsync(this IHubSpotCompanyConnector connector, string domain) => FindByDomainAsync<Company>(connector, domain);

    }
}