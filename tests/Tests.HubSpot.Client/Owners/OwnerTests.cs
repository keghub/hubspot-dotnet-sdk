using System;
using System.Collections.Generic;
using System.Text;
using HubSpot.Model.Owners;
using WorldDomination.Net.Http;

namespace Tests.Owners
{
    public class OwnerTests : HttpHubSpotClientTestBase
    {
        protected IHubSpotOwnerClient CreateSystemUnderTest(params HttpMessageOptions[] options) => CreateClient(options).Owners;
    }
}
