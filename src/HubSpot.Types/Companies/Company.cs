using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.Companies
{
    public class Company : IHubSpotEntity
    {
        [DefaultProperty("companyId")]
        public int Id { get; set; }

        [DefaultProperty("portalId")]
        public int PortalId { get; set; }

        [DefaultProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [CustomProperty("name")]
        public string Name { get; set; }

        [CustomProperty("createdate", IsReadOnly = true)]
        public DateTimeOffset Created { get; set; }

        [CustomProperty("domain")]
        public string Domain { get; set; }

        [CustomProperty("website")]
        public string WebSite { get; set; }

        [CustomProperty("address")]
        public string Address { get; set; }

        [CustomProperty("city")]
        public string City { get; set; }

        [CustomProperty("state")]
        public string State { get; set; }

        [CustomProperty("country")]
        public string Country { get; set; }

        IReadOnlyDictionary<string, object> IHubSpotEntity.Properties { get; set; }
    }
}
