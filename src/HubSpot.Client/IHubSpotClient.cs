using HubSpot.Model.Companies;
using HubSpot.Model.Contacts;
using HubSpot.Model.Deals;
using HubSpot.Model.Lists;
using HubSpot.Model.Owners;

namespace HubSpot
{
    public interface IHubSpotClient
    {
        IHubSpotContactClient Contacts { get; }

        IHubSpotCompanyClient Companies { get; }

        IHubSpotDealClient Deals { get; }

        IHubSpotListClient Lists { get; }

        IHubSpotOwnerClient Owners { get; }
    }
}