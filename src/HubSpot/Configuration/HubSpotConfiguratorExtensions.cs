using System;
using System.Collections.Generic;
using HubSpot.Companies;
using HubSpot.Configuration;
using HubSpot.Contacts;
using HubSpot.Converters;
using HubSpot.Deals;
using HubSpot.Internal;
using HubSpot.LineItems;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HubSpotConfiguratorExtensions
    {
        public static IHubSpotConfigurator ConfigureHubSpotClient(this IHubSpotConfigurator configurator, Action<IHubSpotClientConfigurator> clientConfiguration)
        {
            configurator.AddClientConfiguration(clientConfiguration);

            return configurator;
        }

        public static IHubSpotConfigurator UseContactConnector(this IHubSpotConfigurator configurator)
        {
            configurator.AddServiceConfiguration(services =>
            {
                services.AddSingleton<IContactTypeManager, ContactTypeManager>();

                services.AddSingleton<IHubSpotContactConnector, HubSpotContactConnector>();
            });

            configurator.RequireTypeStore();

            return configurator;
        }

        public static IHubSpotConfigurator UseCompanyConnector(this IHubSpotConfigurator configurator)
        {
            configurator.AddServiceConfiguration(services =>
            {
                services.AddSingleton<ICompanyTypeManager, CompanyTypeManager>();

                services.AddSingleton<IHubSpotCompanyConnector, HubSpotCompanyConnector>();
            });

            configurator.RequireTypeStore();

            return configurator;
        }

        public static IHubSpotConfigurator UseDealConnector(this IHubSpotConfigurator configurator)
        {
            configurator.AddServiceConfiguration(services =>
            {
                services.AddSingleton<IDealTypeManager, DealTypeManager>();

                services.AddSingleton<IHubSpotDealConnector, HubSpotDealConnector>();
            });

            configurator.RequireTypeStore();

            return configurator;
        }

        public static IHubSpotConfigurator UseLineItemConnector(this IHubSpotConfigurator configurator)
        {
            configurator.AddServiceConfiguration(services =>
            {
                services.AddSingleton<ILineItemTypeManager, LineItemTypeManager>();
                services.AddSingleton<IHubSpotLineItemConnector, HubSpotLineItemConnector>();
            });

            configurator.RequireTypeStore();

            return configurator;
        }

        public static IHubSpotConfigurator RegisterDefaultConverters(this IHubSpotConfigurator configurator)
        {
            configurator.RegisterConverter(new StringTypeConverter(), typeof(string));
            configurator.RegisterConverter(new LongTypeConverter(), typeof(long), typeof(long?));
            configurator.RegisterConverter(new DateTimeOffSetConverter(), typeof(DateTimeOffset), typeof(DateTimeOffset?));
            configurator.RegisterConverter(new DateTimeConverter(), typeof(DateTime), typeof(DateTime?));
            configurator.RegisterConverter(new IntTypeConverter(), typeof(int), typeof(int?));
            configurator.RegisterConverter(new DecimalTypeConverter(), typeof(decimal), typeof(decimal?));
            configurator.RegisterConverter(new StringListConverter(), typeof(List<string>), typeof(IList<string>), typeof(IEnumerable<string>), typeof(IReadOnlyList<string>));
            configurator.RegisterConverter(new StringArrayConverter(), typeof(string[]));

            return configurator;
        }

        public static IHubSpotConfigurator RegisterConverter<TConverter>(this IHubSpotConfigurator configurator, TConverter converter, params Type[] types) where TConverter : ITypeConverter
        {
            configurator.AddServiceConfiguration(services =>
            {
                foreach (var type in types)
                {
                    services.AddSingleton(new TypeConverterRegistration
                    {
                        Converter = converter,
                        Type = type
                    });
                }
            });

            return configurator;
        }

        public static IHubSpotConfigurator UseOAuthAuthentication(this IHubSpotConfigurator configurator, IConfiguration configuration)
        {
            configurator.AddClientConfiguration(client => client.UseOAuthAuthentication(configuration));

            return configurator;
        }

        public static IHubSpotConfigurator UseApiKeyAuthentication(this IHubSpotConfigurator configurator, IConfiguration configuration)
        {
            configurator.AddClientConfiguration(client => client.UseApiKeyAuthentication(configuration));

            return configurator;
        }
    }
}