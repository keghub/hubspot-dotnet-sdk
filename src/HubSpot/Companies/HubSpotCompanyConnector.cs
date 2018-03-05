using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubSpot.Internal;
using HubSpot.Model;
using Microsoft.Extensions.Logging;

namespace HubSpot.Companies
{
    public class HubSpotCompanyConnector : IHubSpotCompanyConnector
    {
        private readonly IHubSpotClient _client;
        private readonly ICompanyTypeManager _typeManager;
        private readonly ILogger<HubSpotCompanyConnector> _logger;

        public HubSpotCompanyConnector(IHubSpotClient client, ICompanyTypeManager typeManager, ILogger<HubSpotCompanyConnector> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _typeManager = typeManager ?? throw new ArgumentNullException(nameof(typeManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TCompany> GetAsync<TCompany>(ICompanySelector selector) where TCompany : Company, new()
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var properties = _typeManager.GetCustomProperties<TCompany>(TypeManager.AllProperties).Select(p => new Property(p.metadata.PropertyName)).ToArray();

            var hubspot = await selector.GetCompany(_client, properties).ConfigureAwait(false);

            var company = _typeManager.ConvertTo<TCompany>(hubspot);

            return company;
        }

        public async Task<TCompany> SaveAsync<TCompany>(TCompany company) where TCompany : Company, new()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IReadOnlyList<TCompany>> FindAsync<TCompany>(ICompanyFilter filter = null) where TCompany : Company, new()
        {
            if (filter == null)
            {
                filter = FilterCompanies.All;
            }

            var properties = _typeManager.GetCustomProperties<TCompany>(TypeManager.AllProperties).Select(p => new Property(p.metadata.PropertyName)).ToArray();

            var matchingCompanies = await filter.GetCompanies(_client, properties);

            return matchingCompanies.Select(_typeManager.ConvertTo<TCompany>).ToArray();

        }
    }
}