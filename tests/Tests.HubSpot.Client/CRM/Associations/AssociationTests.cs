using HubSpot.Model.CRM.Associations;
using WorldDomination.Net.Http;

namespace Tests.CRM.Associations
{
    public class AssociationTests : HttpHubSpotClientTestBase
    {
        protected IHubSpotCrmAssociationClient CreateSystemUnderTest(params HttpMessageOptions[] options) => CreateClient(options).Crm.Associations;
    }
}