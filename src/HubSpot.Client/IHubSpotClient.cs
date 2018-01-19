using HubSpot.Model.Contacts;

namespace HubSpot
{
    public interface IHubSpotClient
    {
        IHubSpotContactClient Contacts { get; }
    }
}