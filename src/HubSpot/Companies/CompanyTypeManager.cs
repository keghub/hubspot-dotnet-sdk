using System;
using System.Collections.Generic;
using System.Linq;
using HubSpot.Internal;

namespace HubSpot.Companies
{

    public interface ICompanyTypeManager : ITypeManager<Model.Companies.Company, Company> { }

    public class CompanyTypeManager : TypeManager<Model.Companies.Company, Company>, ICompanyTypeManager
    {
        public CompanyTypeManager(ITypeStore typeStore) : base(typeStore) { }

        protected override IReadOnlyList<KeyValuePair<string, string>> GetCustomProperties(Model.Companies.Company item)
        {
            var properties = from kvp in item.Properties
                             let key = kvp.Key
                             let value = kvp.Value.Value
                             select new KeyValuePair<string, string>(key, value);

            return properties.ToArray();
        }

        protected override IReadOnlyList<KeyValuePair<string, object>> GetDefaultProperties(Model.Companies.Company item)
        {
            return new[]
            {
                new KeyValuePair<string, object>("portalId", item.PortalId),
                new KeyValuePair<string, object>("companyId", item.Id),
                new KeyValuePair<string, object>("isDeleted", item.IsDeleted)
            };
        }
    }
}