using System;
using Microsoft.Extensions.DependencyInjection;

namespace HubSpot.Configuration
{
    public interface IHubSpotConfigurator
    {
        void AddServiceConfiguration(Action<IServiceCollection> serviceConfiguration);

        void AddClientConfiguration(Action<IHubSpotClientConfigurator> clientConfiguration);

        void RequireTypeStore();
    }
}
