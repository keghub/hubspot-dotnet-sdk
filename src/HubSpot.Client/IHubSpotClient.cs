using HubSpot.Model.Companies;
using HubSpot.Model.Contacts;
using HubSpot.Model.CRM.Associations;
using HubSpot.Model.Deals;
using HubSpot.Model.LineItems;
using HubSpot.Model.Lists;
using HubSpot.Model.Owners;
using HubSpot.Model.Pipelines;

namespace HubSpot
{
    public interface IHubSpotClient
    {
        IHubSpotContactClient Contacts { get; }

        IHubSpotCompanyClient Companies { get; }

        IHubSpotDealClient Deals { get; }

        IHubSpotPipelineClient Pipelines { get; }

        IHubSpotListClient Lists { get; }

        IHubSpotOwnerClient Owners { get; }

        IHubSpotCrmClient Crm { get; }

        IHubSpotLineItemClient LineItems { get; }
    }

    public interface IHubSpotCrmClient
    {
        IHubSpotCrmAssociationClient Associations { get; }
    }
}