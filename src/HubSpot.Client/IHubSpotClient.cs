using HubSpot.Model.Companies;
using HubSpot.Model.Contacts;
using HubSpot.Model.Deals;

namespace HubSpot
{
    public interface IHubSpotClient
    {
        IHubSpotContactClient Contacts { get; }

        IHubSpotCompanyClient Companies { get; }

        IHubSpotDealClient Deals { get; }
    }
}