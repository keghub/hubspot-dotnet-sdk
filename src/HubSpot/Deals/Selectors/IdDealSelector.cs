using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Deals.Selectors
{
    public class IdDealSelector : IDealSelector
    {
        private readonly long _dealId;

        public IdDealSelector(long dealId)
        {
            _dealId = dealId;
        }

        public Task<Model.Deals.Deal> GetDeal(IHubSpotClient client)
        {
            return client.Deals.GetByIdAsync(_dealId, false);
        }
    }
}
