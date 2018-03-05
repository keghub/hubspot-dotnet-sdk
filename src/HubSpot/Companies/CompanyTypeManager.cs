using System;
using System.Collections.Generic;
using HubSpot.Internal;

namespace HubSpot.Companies
{

    public interface ICompanyTypeManager : ITypeManager<Model.Companies.Company, Company> { }

    public class CompanyTypeManager : TypeManager<Model.Companies.Company, Company>, ICompanyTypeManager
    {
        public CompanyTypeManager(ITypeStore typeStore) : base(typeStore) { }

        protected override IReadOnlyList<KeyValuePair<string, string>> GetCustomProperties(Model.Companies.Company item)
        {
            throw new NotImplementedException();
        }

        protected override IReadOnlyList<KeyValuePair<string, object>> GetDefaultProperties(Model.Companies.Company item)
        {
            throw new NotImplementedException();
        }
    }
}