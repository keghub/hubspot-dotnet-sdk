using HubSpot.Model.Companies;
using HubSpot.Model.Contacts;

namespace HubSpot
{
    public interface IHubSpotClient
    {
        IHubSpotContactClient Contacts { get; }

        IHubSpotCompanyClient Companies { get; }
    }
}