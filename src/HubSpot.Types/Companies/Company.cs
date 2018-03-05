using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.Companies
{
    public class Company : IHubSpotEntity
    {
        IReadOnlyDictionary<string, object> IHubSpotEntity.Properties { get; set; }
    }
}
