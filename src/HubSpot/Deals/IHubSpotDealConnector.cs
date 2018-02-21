using System.Threading.Tasks;
using HubSpot.Model.Contacts;

namespace HubSpot.Deals
{
    public interface IHubSpotDealConnector
    {
        Task<TDeal> GetByIdAsync<TDeal>(long dealId)
            where TDeal : Deal, new();

        Task<TDeal> SaveAsync<TDeal>(TDeal deal)
            where TDeal : Deal, new();
    }

    public static class HubSpotDealConnectorExtensions
    {
        public static Task<Deal> GetByIdAsync(this IHubSpotDealConnector connector, long dealId) => connector.GetByIdAsync<Deal>(dealId);

        public static Task<Deal> SaveAsync(this IHubSpotDealConnector connector, Deal deal) => connector.SaveAsync(deal);
    }
}