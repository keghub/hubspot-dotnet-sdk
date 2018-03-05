using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubSpot.Internal;
using HubSpot.Model;
using Microsoft.Extensions.Logging;

namespace HubSpot.Deals
{
    public class HubSpotDealConnector : IHubSpotDealConnector
    {
        private readonly IHubSpotClient _client;
        private readonly IDealTypeManager _typeManager;
        private readonly ILogger<HubSpotDealConnector> _logger;

        public HubSpotDealConnector(IHubSpotClient client, IDealTypeManager typeManager, ILogger<HubSpotDealConnector> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _typeManager = typeManager ?? throw new ArgumentNullException(nameof(typeManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TDeal> GetByIdAsync<TDeal>(long dealId)
            where TDeal : Deal, new()
        {
            var hubspotContact = await _client.Deals.GetByIdAsync(dealId, includePropertyVersions: false).ConfigureAwait(false);

            var deal = _typeManager.ConvertTo<TDeal>(hubspotContact);

            return deal;
        }

        public async Task<TDeal> SaveAsync<TDeal>(TDeal deal)
            where TDeal : Deal, new()
        {
            if (deal == null)
            {
                throw new ArgumentNullException(nameof(deal));
            }

            var modifiedProperties = (from property in _typeManager.GetModifiedProperties(deal)
                                      select new ValuedProperty(property.name, property.value)).ToArray();

            var modifiedAssociations = _typeManager.GetModifiedAssociations(deal).ToNestedLookup(o => o.type, o => o.operation, o => o.id);

            if (modifiedProperties.Any() || modifiedAssociations.Any())
            {
                if (IsNew())
                {
                    var newDeal = await _client.Deals.CreateAsync(deal.AssociatedContactIds, deal.AssociatedCompanyIds, modifiedProperties).ConfigureAwait(false);

                    return _typeManager.ConvertTo<TDeal>(newDeal);
                }
                else
                {
                    await _client.Deals.UpdateAsync(deal.Id, modifiedProperties).ConfigureAwait(false);

                    await _client.Deals.AssociateContactsAsync(deal.Id, modifiedAssociations.GetValues(AssociationType.Contact, Operation.Added)).ConfigureAwait(false);
                    await _client.Deals.AssociateCompaniesAsync(deal.Id, modifiedAssociations.GetValues(AssociationType.Company, Operation.Added)).ConfigureAwait(false);

                    await _client.Deals.RemoveAssociationToContactsAsync(deal.Id, modifiedAssociations.GetValues(AssociationType.Contact, Operation.Removed)).ConfigureAwait(false);
                    await _client.Deals.RemoveAssociationToCompaniesAsync(deal.Id, modifiedAssociations.GetValues(AssociationType.Company, Operation.Removed)).ConfigureAwait(false);

                    var updatedDeal = await _client.Deals.GetByIdAsync(deal.Id, includePropertyVersions: false).ConfigureAwait(false);

                    return _typeManager.ConvertTo<TDeal>(updatedDeal);
                }
            }

            return deal;

            bool IsNew()
            {
                return deal.Id == 0 && deal.Created == default;
            }
        }

        public async Task<IReadOnlyList<TDeal>> FindAsync<TDeal>(IDealFilter filter = null)
            where TDeal : Deal, new()
        {
            filter = filter ?? FilterDeals.All;

            var properties = _typeManager.GetCustomProperties<TDeal>(TypeManager.AllProperties).Select(p => new Property(p.metadata.PropertyName)).ToArray();

            var matchingDeals = await filter.GetDeals(_client, properties);

            return matchingDeals.Select(_typeManager.ConvertTo<TDeal>).ToArray();
        }
    }
}