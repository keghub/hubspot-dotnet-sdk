using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using HubSpot.Model.CRM.Associations;
using HubSpot.Model.Lists;
using Kralizek.Extensions.Http;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotCrmClient
    {
        IHubSpotCrmAssociationClient IHubSpotCrmClient.Associations => this;
    }
}