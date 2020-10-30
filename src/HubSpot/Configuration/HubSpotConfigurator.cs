using System;
using System.Collections.Generic;
using System.Linq;
using HubSpot.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace HubSpot.Configuration
{
    public class HubSpotConfigurator : IHubSpotConfigurator
    {
        private readonly IList<Action<IServiceCollection>> _serviceConfigurations = new List<Action<IServiceCollection>>();
        private readonly IList<Action<IHubSpotClientConfigurator>> _clientConfigurations = new List<Action<IHubSpotClientConfigurator>>();
        private bool _requiresTypeStore;

        public void AddClientConfiguration(Action<IHubSpotClientConfigurator> clientConfiguration)
        {
            _ = clientConfiguration ?? throw new ArgumentNullException(nameof(clientConfiguration));

            _clientConfigurations.Add(clientConfiguration);
        }

        public void AddServiceConfiguration(Action<IServiceCollection> serviceConfiguration)
        {
            _ = serviceConfiguration ?? throw new ArgumentNullException(nameof(serviceConfiguration));

            _serviceConfigurations.Add(serviceConfiguration);
        }

        public void RequireTypeStore() => _requiresTypeStore = true;

        public void ApplyServiceConfigurations(IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            foreach (var action in _serviceConfigurations)
            {
                action(services);
            }

            if (_requiresTypeStore)
            {
                services.AddSingleton<IReadOnlyList<TypeConverterRegistration>>(sp =>
                {
                    var registrars = sp.GetServices<TypeConverterRegistration>();

                    return registrars.ToArray();
                });

                services.AddSingleton<ITypeStore, TypeStore>();
            }
        }

        public void ApplyClientConfigurations(IHubSpotClientConfigurator configurator)
        {
            _ = configurator ?? throw new ArgumentNullException(nameof(configurator));

            foreach (var action in _clientConfigurations)
            {
                action(configurator);
            }
        }
    }
}
