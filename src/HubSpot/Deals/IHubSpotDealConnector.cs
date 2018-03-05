using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Deals
{
    public interface IHubSpotDealConnector
    {
        Task<TDeal> GetByIdAsync<TDeal>(long dealId)
            where TDeal : Deal, new();

        Task<TDeal> SaveAsync<TDeal>(TDeal deal)
            where TDeal : Deal, new();

        Task<IReadOnlyList<TDeal>> FindAsync<TDeal>(IDealFilter filter = null)
            where TDeal : Deal, new();
    }

    public static class HubSpotDealConnectorExtensions
    {
        public static Task<Deal> GetByIdAsync(this IHubSpotDealConnector connector, long dealId) => connector.GetByIdAsync<Deal>(dealId);

        public static Task<Deal> SaveAsync(this IHubSpotDealConnector connector, Deal deal) => connector.SaveAsync(deal);

        public static Task<IReadOnlyList<Deal>> FindAsync(this IHubSpotDealConnector connector, IDealFilter filter) => connector.FindAsync<Deal>(filter);
    }

}