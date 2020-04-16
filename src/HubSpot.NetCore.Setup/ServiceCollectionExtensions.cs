using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HubSpot.Authentication;
using HubSpot.Companies;
using HubSpot.Contacts;
using HubSpot.Converters;
using HubSpot.Deals;
using HubSpot.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HubSpot
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHubSpot(this IServiceCollection services, Action<HubSpotConfigurator> configuration = null)
        {
            services.AddOptions();

            services.AddSingleton<IHubSpotClient, HttpHubSpotClient>();

            var cfg = new HubSpotConfigurator();

            configuration?.Invoke(cfg);

            cfg.ApplyConfiguration(services);
        }
    }

    public static class HubSpotConfiguratorExtensions
    {
        public static void UseApiKey(this HubSpotConfigurator configurator, IConfiguration config, string name = "HUBSPOT_APIKEY")
        {
            var apiKey = config[name];

            configurator.AddConfiguration(services => services.AddSingleton<HubSpotAuthenticator>(sp => new ApiKeyHubSpotAuthenticator(apiKey)));
        }

        public static void UseOAuth(this HubSpotConfigurator configurator, IConfiguration config)
        {
            configurator.AddConfiguration(services =>
            {
                services.Configure<OAuthOptions>(config);

                services.AddSingleton<HubSpotAuthenticator, OAuthHubSpotAuthenticator>();
            });
        }

        public static void UseContactConnector(this HubSpotConfigurator configurator)
        {
            configurator.AddConfiguration(services =>
            {
                services.AddSingleton<IContactTypeManager, ContactTypeManager>();

                services.AddSingleton<IHubSpotContactConnector, HubSpotContactConnector>();
            });

            configurator.RequireTypeStore();
        }

        public static void UseCompanyConnector(this HubSpotConfigurator configurator)
        {
            configurator.AddConfiguration(services =>
            {
                services.AddSingleton<ICompanyTypeManager, CompanyTypeManager>();

                services.AddSingleton<IHubSpotCompanyConnector, HubSpotCompanyConnector>();
            });

            configurator.RequireTypeStore();
        }

        public static void UseDealConnector(this HubSpotConfigurator configurator)
        {
            configurator.AddConfiguration(services =>
            {
                services.AddSingleton<IDealTypeManager, DealTypeManager>();

                services.AddSingleton<IHubSpotDealConnector, HubSpotDealConnector>();
            });

            configurator.RequireTypeStore();
        }

        public static void RegisterDefaultConverters(this HubSpotConfigurator configurator)
        {
            configurator.RegisterConverter(new StringTypeConverter(), typeof(string));
            configurator.RegisterConverter(new LongTypeConverter(), typeof(long), typeof(long?));
            configurator.RegisterConverter(new DateTimeTypeConverter(), typeof(DateTimeOffset), typeof(DateTimeOffset?));
            configurator.RegisterConverter(new IntTypeConverter(), typeof(int), typeof(int?));
            configurator.RegisterConverter(new DecimalTypeConverter(), typeof(decimal), typeof(decimal?));
            configurator.RegisterConverter(new StringListConverter(), typeof(List<string>), typeof(IList<string>), typeof(IEnumerable<string>), typeof(IReadOnlyList<string>));
            configurator.RegisterConverter(new StringArrayConverter(), typeof(string[]));
        }

        public static void RegisterConverter<TConverter>(this HubSpotConfigurator configurator, TConverter converter, params Type[] types) where TConverter : ITypeConverter
        {
            configurator.AddConfiguration(services =>
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
        }
    }

    public class HubSpotConfigurator
    {
        private readonly List<Action<IServiceCollection>> _configurations = new List<Action<IServiceCollection>>();

        public void AddConfiguration(Action<IServiceCollection> configurationAction)
        {
            _configurations.Add(configurationAction);
        }

        public void ApplyConfiguration(IServiceCollection services)
        {
            foreach (var action in _configurations)
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

        private bool _requiresTypeStore;

        public void RequireTypeStore() => _requiresTypeStore = true;
    }
}
