using System;
using HubSpot;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
// ReSharper disable InvokeAsExtensionMethod

namespace Tests
{
    [TestFixture]
    public class ServiceCollectionExtensionsTests
    {
        [Test]
        public void AddHubSpot_configures_HttpHubSpotClient()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(Mock.Of<HubSpotAuthenticator>());

            ServiceCollectionExtensions.AddHubSpot(services);

            var serviceProvider = services.BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<IHubSpotClient>();

            Assert.That(client, Is.InstanceOf<HttpHubSpotClient>());
        }

        [Test]
        public void AddHubSpot_uses_configurator_delegate()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(Mock.Of<HubSpotAuthenticator>());

            var mockDelegate = new Mock<Action<HubSpotConfigurator>>();

            ServiceCollectionExtensions.AddHubSpot(services, mockDelegate.Object);

            mockDelegate.Verify(p => p(It.IsAny<HubSpotConfigurator>()), Times.Once);
        }
    }
}