using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Deals.Selectors;
using HubSpot.Model;

namespace HubSpot.Deals
{
    public interface IDealSelector
    {
        Task<Model.Deals.Deal> GetDeal(IHubSpotClient client);
    }

    public static class SelectDeal
    {
        public static IDealSelector ById(long dealId) => new IdDealSelector(dealId);
    }

    public static class SelectDealExtensions
    {
        public static Task<TDeal> GetByIdAsync<TDeal>(this IHubSpotDealConnector connector, long dealId) where TDeal : Deal, new()
            => connector.GetAsync<TDeal>(SelectDeal.ById(dealId));

        public static Task<Deal> GetByIdAsync(this IHubSpotDealConnector connector, long dealId)
            => GetByIdAsync<Deal>(connector, dealId);
    }
}