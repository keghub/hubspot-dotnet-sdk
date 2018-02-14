using HubSpot.Model.Contacts.Properties;
using WorldDomination.Net.Http;

namespace Tests.Contacts.Properties
{
    public class ContactPropertyTests : HttpHubSpotClientTestBase
    {
        protected IHubSpotContactPropertyClient CreateSystemUnderTest(params HttpMessageOptions[] options) => CreateClient(options).Contacts.Properties;
    }
}