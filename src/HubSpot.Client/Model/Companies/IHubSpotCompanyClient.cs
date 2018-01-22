using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Model.Companies
{
    public interface IHubSpotCompanyClient
    {
        Task<Company> GetByIdAsync(long companyId);

        Task<Company> CreateAsync(IReadOnlyList<ValuedProperty> properties);

        Task<Company> UpdateAsync(long companyId, IReadOnlyList<ValuedProperty> propertiesToUpdate);

        Task UpdateManyAsync(IReadOnlyList<ObjectPropertyList> companiesToUpdate);

        Task<DeleteCompanyResponse> DeleteAsync(long companyId);

        Task<CompanyList> GetAllAsync(IReadOnlyList<IProperty> properties = null, IReadOnlyList<IProperty> propertiesWithHistory = null, int limit = 100, long? companyOffset = null);

        Task<CompanyListResponse> GetRecentlyCreatedAsync(int count = 100, long? offset = null);

        Task<CompanyListResponse> GetRecentlyUpdatedAsync(int count = 100, long? offset = null);

        Task<SearchResponse> SearchAsync(IReadOnlyList<IProperty> properties = null, int limit = 100, long? companyOffset = null);
    }
}