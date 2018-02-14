using HubSpot.Model.Contacts.Properties;
using WorldDomination.Net.Http;

namespace Tests.Contacts.PropertyGroups
{
    public class ContactPropertyGroupTests : HttpHubSpotClientTestBase
    {
        protected IHubSpotContactPropertyGroupClient CreateSystemUnderTest(params HttpMessageOptions[] options) => CreateClient(options).Contacts.PropertyGroups;
    }
}