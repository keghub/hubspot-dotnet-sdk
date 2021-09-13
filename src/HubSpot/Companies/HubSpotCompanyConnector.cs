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

        public HubSpotCompanyConnector(IHubSpotClient client, ICompanyTypeManager typeManager)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _typeManager = typeManager ?? throw new ArgumentNullException(nameof(typeManager));
        }

        public async Task<TCompany> GetAsync<TCompany>(ICompanySelector selector) where TCompany : Company, new()
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var properties = _typeManager.GetCustomProperties<TCompany>(TypeManager.AllProperties).Select(p => new Property(p.FieldName)).ToArray();

            try
            {
                var hubspot = await selector.GetCompany(_client, properties).ConfigureAwait(false);
                var company = _typeManager.ConvertTo<TCompany>(hubspot);
                return company;
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        public async Task<TCompany> SaveAsync<TCompany>(TCompany company) where TCompany : Company, new()
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            var customProperties = (from property in _typeManager.GetPropertyData(company)
                                      select new ValuedPropertyV2(property.PropertyName, property.Value?.ToString()))
                                      .ToArray();

            if (customProperties.All(cp => string.IsNullOrEmpty(cp.Value)) && IsNew())
            {
                return company;
            }

            if (IsNew())
            {
                var createdCompany = await _client.Companies.CreateAsync(customProperties).ConfigureAwait(false);
                var newCompany = await _client.Companies.GetByIdAsync(createdCompany.Id).ConfigureAwait(false);
                return _typeManager.ConvertTo<TCompany>(newCompany);
            }
            
            await _client.Companies.UpdateAsync(company.Id, customProperties).ConfigureAwait(false);
            var modifiedCompany = await _client.Companies.GetByIdAsync(company.Id).ConfigureAwait(false);
            return _typeManager.ConvertTo<TCompany>(modifiedCompany);

            bool IsNew()
            {
                return company.Id == 0 && company.Created == default;

            }
        }

        public async Task<IReadOnlyList<TCompany>> FindAsync<TCompany>(ICompanyFilter filter = null) where TCompany : Company, new()
        {
            if (filter == null)
            {
                filter = FilterCompanies.All;
            }

            var properties = _typeManager.GetCustomProperties<TCompany>(TypeManager.AllProperties).Select(p => new Property(p.FieldName)).ToArray();

            var matchingCompanies = await filter.GetCompanies(_client, properties);

            return matchingCompanies.Select(_typeManager.ConvertTo<TCompany>).ToArray();

        }
    }
}