using HubSpot.Authentication;
using HubSpot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Tests.Configuration
{
    [TestFixture]
    public class HubSpotClientConfiguratorExtensionsTests
    {
        [Test, CustomAutoData]
        public void UseApiKeyAuthentication_registers_needed_services(HubSpotClientConfigurator configurator, ApiKeyOptions apiKeyOptions, ServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddObject(apiKeyOptions);

            var configuration = configurationBuilder.Build();

            configurator.UseApiKeyAuthentication(configuration);

            configurator.ApplyServiceConfigurations(services);

            var serviceProvider = services.BuildServiceProvider();

            Assert.Multiple(() =>
            {
                Assert.That(() => serviceProvider.GetRequiredService<IOptions<ApiKeyOptions>>(), Throws.Nothing);

                Assert.That(() => serviceProvider.GetRequiredService<ApiKeyHubSpotAuthenticator>(), Throws.Nothing);
            });
        }

        [Test, CustomAutoData]
        public void UseOAuthAuthentication_registers_needed_services(HubSpotClientConfigurator configurator, OAuthOptions apiKeyOptions, ServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddObject(apiKeyOptions);

            var configuration = configurationBuilder.Build();

            configurator.UseOAuthAuthentication(configuration);

            configurator.ApplyServiceConfigurations(services);

            var serviceProvider = services.BuildServiceProvider();

            Assert.Multiple(() =>
            {
                Assert.That(() => serviceProvider.GetRequiredService<IOptions<OAuthOptions>>(), Throws.Nothing);

                Assert.That(() => serviceProvider.GetRequiredService<OAuthHubSpotAuthenticator>(), Throws.Nothing);
            });
        }
    }
}