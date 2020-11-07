using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HubSpot.Configuration
{
    public class HubSpotClientConfigurator : IHubSpotClientConfigurator
    {
        private readonly IList<Action<IHttpClientBuilder>> _httpClientBuilderConfigurations = new List<Action<IHttpClientBuilder>>();

        private readonly IList<Action<IServiceCollection>> _serviceCollectionConfigurations = new List<Action<IServiceCollection>>();

        public void AddHttpClientBuilderConfiguration(Action<IHttpClientBuilder> configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _httpClientBuilderConfigurations.Add(configuration);
        }

        public void AddServiceConfiguration(Action<IServiceCollection> configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _serviceCollectionConfigurations.Add(configuration);
        }

        public void ApplyHttpClientBuilderConfigurations(IHttpClientBuilder builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            foreach (var action in _httpClientBuilderConfigurations)
            {
                action(builder);
            }
        }

        public void ApplyServiceConfigurations(IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            foreach (var action in _serviceCollectionConfigurations)
            {
                action(services);
            }
        }
    }
}