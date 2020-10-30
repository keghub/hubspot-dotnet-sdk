using Microsoft.Extensions.DependencyInjection;
using System;

namespace HubSpot.Configuration
{
    public interface IHubSpotClientConfigurator
    {
        void AddHttpClientBuilderConfiguration(Action<IHttpClientBuilder> configuration);

        void AddServiceConfiguration(Action<IServiceCollection> configuration);
    }
}