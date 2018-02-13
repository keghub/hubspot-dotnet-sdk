using HubSpot.Model.Contacts;
using WorldDomination.Net.Http;

namespace Tests.Contacts
{
    public class ContactTests : HttpHubSpotClientTestBase
    {
        protected IHubSpotContactClient CreateSystemUnderTest(params HttpMessageOptions[] options) => CreateClient(options).Contacts;
    }
}