using System;
using System.Net.Http;
using HubSpot.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Tests.Configuration
{
    [TestFixture]
    public class HubSpotClientConfiguratorTests
    {
        [Test, CustomAutoData]
        public void Registered_service_actions_are_applied(HubSpotClientConfigurator sut, Action<IServiceCollection> action, ServiceCollection services)
        {
            sut.AddServiceConfiguration(action);

            sut.ApplyServiceConfigurations(services);

            Mock.Get(action).Verify(p => p(services));
        }

        [Test, CustomAutoData]
        public void Registered_httpClientBuilder_actions_are_applied(HubSpotClientConfigurator sut, Action<IHttpClientBuilder> action, IHttpClientBuilder builder)
        {
            sut.AddHttpClientBuilderConfiguration(action);

            sut.ApplyHttpClientBuilderConfigurations(builder);

            Mock.Get(action).Verify(p => p(builder));
        }
    }
}