using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Model.Deals
{
    public interface IHubSpotDealClient
    {
        Task<Deal> CreateAsync(IReadOnlyList<long> associatedContactIds, IReadOnlyList<long> associatedCompanyIds, IReadOnlyList<ValuedProperty> properties);

        Task<Deal> UpdateAsync(long dealId, IReadOnlyList<ValuedProperty> propertiesToUpdate);

        Task UpdateManyAsync(IReadOnlyList<ObjectPropertyList> dealsToUpdate);

        Task DeleteAsync(long dealId);

        Task<Deal> GetByIdAsync(long dealId, bool includePropertyVersions = true);

        Task AssociateContactsAsync(long dealId, IReadOnlyList<long> ids);

        Task RemoveAssociationToContactsAsync(long dealId, IReadOnlyList<long> ids);

        Task AssociateCompaniesAsync(long dealId, IReadOnlyList<long> ids);

        Task RemoveAssociationToCompaniesAsync(long dealId, IReadOnlyList<long> ids);

        Task<DealList> GetAllAsync(IReadOnlyList<IProperty> properties = null, IReadOnlyList<IProperty> propertiesWithHistory = null, bool includeAssociations = true, int limit = 100, long? offset = null);

        Task<PagedList<Deal>> GetRecentlyUpdatedAsync(DateTimeOffset? since = null, bool includePropertyVersions = true, int count = 20, long? offset = null);

        Task<PagedList<Deal>> GetRecentlyCreatedAsync(DateTimeOffset? since = null, bool includePropertyVersions = true, int count = 20, long? offset = null);

        Task<DealList> FindByContactAsync(long contactId, IReadOnlyList<IProperty> properties = null, IReadOnlyList<IProperty> propertiesWithHistory = null, bool includeAssociations = true, int limit = 100, long? offset = null);

        Task<DealList> FindByCompanyAsync(long companyId, IReadOnlyList<IProperty> properties = null, IReadOnlyList<IProperty> propertiesWithHistory = null, bool includeAssociations = true, int limit = 100, long? offset = null);
    }
}