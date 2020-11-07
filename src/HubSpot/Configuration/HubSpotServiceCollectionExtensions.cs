using System;
using HubSpot.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HubSpotServiceCollectionExtensions
    {
        public static IServiceCollection AddHubSpot(this IServiceCollection services, Action<IHubSpotConfigurator> configuration = null)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            var configurator = new HubSpotConfigurator();

            configurator.RegisterDefaultConverters();

            configuration?.Invoke(configurator);

            services.AddHubSpotClient(client => 
            {
                configurator.ApplyClientConfigurations(client);
            });

            configurator.ApplyServiceConfigurations(services);

            return services;
        }
    }
}