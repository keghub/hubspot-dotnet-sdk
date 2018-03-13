using System;
using System.Net.Http;
using System.Web;
using HubSpot.Utils;

namespace HubSpot
{
    public abstract class HubSpotAuthenticator : DelegatingHandler
    {
        protected HubSpotAuthenticator()
        {
            InnerHandler = new HttpClientHandler();
        }

        public virtual Uri ServiceUri { get; } = new Uri("https://api.hubapi.com");
    }
}