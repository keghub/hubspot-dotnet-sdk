using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Deals.Filters;
using HubSpot.Model;

namespace HubSpot.Deals
{
    public interface IDealFilter
    {
        Task<IReadOnlyList<Model.Deals.Deal>> GetDeals(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery);
    }

    public static class FilterDeals
    {
        public static readonly IDealFilter All = new AllDealFilter();

        public static IDealFilter ByCompanyId(long companyId) => new AssociatedToCompanyDealFilter(companyId);

        public static IDealFilter ByContactId(long contactId) => new AssociatedToContactDealFilter(contactId);

        public static readonly IDealFilter RecentlyCreated = new RecentlyCreatedDealFilter();

        public static readonly IDealFilter RecentlyModified = new RecentlyUpdatedDealFilter();
    }
}