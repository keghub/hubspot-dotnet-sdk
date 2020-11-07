using Microsoft.Extensions.DependencyInjection;
using Kralizek.Extensions.Http;
using System;
using Microsoft.Extensions.Configuration;
using HubSpot.Authentication;
using Newtonsoft.Json;
using System.Net.Http;
using HubSpot.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HubSpotClientConfiguratorExtensions
    {
        public static IHubSpotClientConfigurator ConfigureHttpClientBuilder(this IHubSpotClientConfigurator configurator, Action<IHttpClientBuilder> builderConfiguration)
        {
            configurator.AddHttpClientBuilderConfiguration(builderConfiguration);

            return configurator;
        }

        public static IHubSpotClientConfigurator SetBaseAddress(this IHubSpotClientConfigurator configurator, Uri baseAddress)
        {
            configurator.AddHttpClientBuilderConfiguration(builder => builder.ConfigureHttpClient(http => http.BaseAddress = baseAddress));

            return configurator;
        }

        public static IHubSpotClientConfigurator UseOAuthAuthentication(this IHubSpotClientConfigurator configurator, IConfiguration configuration)
        {
            configurator.AddServiceConfiguration(services => services.Configure<OAuthOptions>(configuration));

            configurator.AddHttpClientBuilderConfiguration(builder => builder.AddHttpMessageHandler<OAuthHubSpotAuthenticator>());

            configurator.AddServiceConfiguration(services => services.AddTransient<OAuthHubSpotAuthenticator>());

            return configurator;
        }

        public static IHubSpotClientConfigurator UseApiKeyAuthentication(this IHubSpotClientConfigurator configurator, IConfiguration configuration)
        {
            configurator.AddServiceConfiguration(services => services.Configure<ApiKeyOptions>(configuration));

            configurator.AddHttpClientBuilderConfiguration(builder => builder.AddHttpMessageHandler<ApiKeyHubSpotAuthenticator>());

            configurator.AddServiceConfiguration(services => services.AddTransient<ApiKeyHubSpotAuthenticator>());

            return configurator;
        }

        public static IHubSpotClientConfigurator ConfigureSerialization(this IHubSpotClientConfigurator configurator, Action<JsonSerializerSettings> serializationConfiguration)
        {
            configurator.AddHttpClientBuilderConfiguration(builder => builder.ConfigureSerialization(serializationConfiguration));

            return configurator;
        }

        public static IHubSpotClientConfigurator ConfigureHttpClient(this IHubSpotClientConfigurator configurator, Action<HttpClient> httpClientConfiguration)
        {
            configurator.AddHttpClientBuilderConfiguration(builder => builder.ConfigureHttpClient(httpClientConfiguration));

            return configurator;
        }
    }
}