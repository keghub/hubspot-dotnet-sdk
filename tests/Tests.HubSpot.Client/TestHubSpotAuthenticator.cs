using System;
using System.Net.Http;
using HubSpot;

namespace Tests
{
    public class TestHubSpotAuthenticator : HubSpotAuthenticator
    {
        public TestHubSpotAuthenticator(HttpMessageHandler innerHandler)
        {
            InnerHandler = innerHandler ?? throw new ArgumentNullException(nameof(innerHandler));
        }
    }
}