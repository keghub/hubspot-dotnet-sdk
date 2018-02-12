using HubSpot.Model.Companies;
using HubSpot.Model.Contacts;
using HubSpot.Model.Deals;
using HubSpot.Model.Lists;

namespace HubSpot
{
    public interface IHubSpotClient
    {
        IHubSpotContactClient Contacts { get; }

        IHubSpotCompanyClient Companies { get; }

        IHubSpotDealClient Deals { get; }

        IHubSpotListClient Lists { get; }
    }
}