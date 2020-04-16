using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Authentication;
using HubSpot.Companies;
using HubSpot.Contacts;
using HubSpot.Converters;
using HubSpot.Deals;
using HubSpot.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
// ReSharper disable InvokeAsExtensionMethod

namespace Tests {
    [TestFixture]
    public class HubSpotConfiguratorExtensionsTests
    {
        [Test, AutoData]
        public void UseApiKey_configures_a_ApiKeyHubSpotAuthenticator(HubSpotConfigurator sut, string apiKey)
        {
            var services = new ServiceCollection();

            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["HUBSPOT_APIKEY"] = apiKey
            });

            var configuration = configurationBuilder.Build();

            HubSpotConfiguratorExtensions.UseApiKey(sut, configuration);

            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var authenticator = serviceProvider.GetRequiredService<HubSpotAuthenticator>();

            Assert.That(authenticator, Is.InstanceOf<ApiKeyHubSpotAuthenticator>());
        }

        [Test, AutoData]
        public void UseOAuth_configures_a_OAuthHubSpotAuthenticator(HubSpotConfigurator sut, OAuthOptions options)
        {
            var services = new ServiceCollection();
            services.AddOptions();

            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ClientId"] = options.ClientId,
                ["SecretKey"] = options.SecretKey,
                ["RedirectUri"] = options.RedirectUri.ToString(),
                ["RefreshToken"] = options.RefreshToken,
                ["ClockSkew"] = options.ClockSkew.ToString()
            });

            var configuration = configurationBuilder.Build();

            HubSpotConfiguratorExtensions.UseOAuth(sut, configuration);

            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var authenticator = serviceProvider.GetRequiredService<HubSpotAuthenticator>();
            Assert.That(authenticator, Is.InstanceOf<OAuthHubSpotAuthenticator>());
        }

        [Test, AutoData]
        public void RegisterConverter_registers_a_converter(HubSpotConfigurator sut)
        {
            var mockConverter = new Mock<ITypeConverter>();
            var targetType = typeof(int);

            HubSpotConfiguratorExtensions.RegisterConverter(sut, mockConverter.Object, targetType);

            var services = new ServiceCollection();

            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var registration = serviceProvider.GetService<TypeConverterRegistration>();

            Assert.That(registration.Converter, Is.SameAs(mockConverter.Object));
            Assert.That(registration.Type, Is.EqualTo(targetType));
        }

        [Test]
        [InlineAutoData(typeof(string))]
        [InlineAutoData(typeof(int))]
        [InlineAutoData(typeof(int?))]
        [InlineAutoData(typeof(long))]
        [InlineAutoData(typeof(long?))]
        [InlineAutoData(typeof(decimal))]
        [InlineAutoData(typeof(decimal?))]
        [InlineAutoData(typeof(DateTimeOffset))]
        [InlineAutoData(typeof(DateTimeOffset?))]
        [InlineAutoData(typeof(List<string>))]
        [InlineAutoData(typeof(string[]))]
        [InlineAutoData(typeof(IList<string>))]
        [InlineAutoData(typeof(IEnumerable<string>))]
        [InlineAutoData(typeof(IReadOnlyList<string>))]
        public void RegisterDefaultConverters_registers_basic_type_converters(Type typeToConvert, HubSpotConfigurator sut)
        {
            HubSpotConfiguratorExtensions.RegisterDefaultConverters(sut);

            var services = new ServiceCollection();

            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var registrations = serviceProvider.GetServices<TypeConverterRegistration>();

            var supportedTypes = registrations.Select(c => c.Type);

            Assert.That(supportedTypes, Contains.Item(typeToConvert));
        }

        [Test, AutoData]
        public void UseContactConnector_registers_a_TypeManager(HubSpotConfigurator sut)
        {
            var services = new ServiceCollection();

            HubSpotConfiguratorExtensions.UseContactConnector(sut);
            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var typeManager = serviceProvider.GetRequiredService<IContactTypeManager>();

            Assert.That(typeManager, Is.Not.Null);
        }

        [Test, AutoData]
        public void UseContactConnector_registers_Connector(HubSpotConfigurator sut)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(Mock.Of<IHubSpotClient>());

            HubSpotConfiguratorExtensions.UseContactConnector(sut);
            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var connector = serviceProvider.GetRequiredService<IHubSpotContactConnector>();

            Assert.That(connector, Is.Not.Null);
        }

        [Test, AutoData]
        public void UseCompanyConnector_registers_a_TypeManager(HubSpotConfigurator sut)
        {
            var services = new ServiceCollection();

            HubSpotConfiguratorExtensions.UseCompanyConnector(sut);
            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var typeManager = serviceProvider.GetRequiredService<ICompanyTypeManager>();

            Assert.That(typeManager, Is.Not.Null);
        }

        [Test, AutoData]
        public void UseCompanyConnector_registers_Connector(HubSpotConfigurator sut)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(Mock.Of<IHubSpotClient>());

            HubSpotConfiguratorExtensions.UseCompanyConnector(sut);
            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var connector = serviceProvider.GetRequiredService<IHubSpotCompanyConnector>();

            Assert.That(connector, Is.Not.Null);
        }

        [Test, AutoData]
        public void UseDealConnector_registers_a_TypeManager(HubSpotConfigurator sut)
        {
            var services = new ServiceCollection();

            HubSpotConfiguratorExtensions.UseDealConnector(sut);
            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var typeManager = serviceProvider.GetRequiredService<IDealTypeManager>();

            Assert.That(typeManager, Is.Not.Null);
        }

        [Test, AutoData]
        public void UseDealConnector_registers_Connector(HubSpotConfigurator sut)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(Mock.Of<IHubSpotClient>());

            HubSpotConfiguratorExtensions.UseDealConnector(sut);
            sut.ApplyConfiguration(services);

            var serviceProvider = services.BuildServiceProvider();

            var connector = serviceProvider.GetRequiredService<IHubSpotDealConnector>();

            Assert.That(connector, Is.Not.Null);
        }
    }
}