using System;
using System.Net.Http;
using HubSpot;
using HubSpot.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Tests.Configuration
{
    [TestFixture]
    public class HubSpotClientServiceCollectionExtensionsTests
    {
        [Test, CustomAutoData]
        public void AddHubSpotClient_registers_HttpHubSpotClient(ServiceCollection services, Action<IHubSpotClientConfigurator> configuration)
        {
            services.AddHubSpotClient(configuration);

            var serviceProvider = services.BuildServiceProvider();

            Assert.That(() => serviceProvider.GetRequiredService<IHubSpotClient>(), Throws.Nothing);
        }

        [Test, CustomAutoData]
        public void AddHubSpotClient_uses_custom_configuration(ServiceCollection services, Action<IHubSpotClientConfigurator> configuration)
        {
            services.AddHubSpotClient(configuration);

            Mock.Get(configuration).Verify(p => p(It.IsAny<HubSpotClientConfigurator>()));
        }

        [Test, CustomAutoData]
        public void AddHubSpotClient_configures_default_baseAddress(ServiceCollection services)
        {
            services.AddHubSpotClient();

            var serviceProvider = services.BuildServiceProvider();

            var http = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(HubSpotClientServiceCollectionExtensions.HttpClientConfigurationName);

            Assert.That(http.BaseAddress, Is.EqualTo(HttpHubSpotClient.DefaultApiEndpoint));
        }

        [Test, CustomAutoData]
        public void CreateRawHubSpotHttpClient_creates_HttpClient_with_proper_configuration(ServiceCollection services)
        {
            services.AddHubSpotClient();

            var serviceProvider = services.BuildServiceProvider();

            var http = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateRawHubSpotHttpClient();

            Assert.That(http.BaseAddress, Is.EqualTo(HttpHubSpotClient.DefaultApiEndpoint));
        }

        [Test, CustomAutoData]
        public void GetRawHubSpotHttpClient_returns_HttpClient_with_proper_configuration(ServiceCollection services)
        {
            services.AddHubSpotClient();

            var serviceProvider = services.BuildServiceProvider();

            var http = serviceProvider.GetRawHubSpotHttpClient();

            Assert.That(http.BaseAddress, Is.EqualTo(HttpHubSpotClient.DefaultApiEndpoint));
        }
    }
}