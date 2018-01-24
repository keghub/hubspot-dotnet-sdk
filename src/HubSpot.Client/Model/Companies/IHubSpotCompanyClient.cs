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

        Task<PagedList<Company>> GetRecentlyCreatedAsync(int count = 100, long? offset = null);

        Task<PagedList<Company>> GetRecentlyUpdatedAsync(int count = 100, long? offset = null);

        Task<SearchResponse> SearchAsync(string domain, IReadOnlyList<IProperty> properties = null, int limit = 100, long? companyOffset = null);

        Task<ContactList> GetContactsInCompanyAsync(long companyId, int count = 100, long? companyOffset = null);

        Task<ContactIdList> GetContactIdsInCompanyAsync(long companyId, int count = 100, long? companyOffset = null);

        Task<Company> AddContactToCompanyAsync(long companyId, long contactId);

        Task RemoveContactFromCompanyAsync(long companyId, long contactId);
    }
}