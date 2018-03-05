using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot;
using HubSpot.Contacts;
using HubSpot.Converters;
using HubSpot.Deals;
using HubSpot.Internal;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Microsoft.Extensions.Logging;

namespace TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loggerFactory = new LoggerFactory().AddConsole((s, l) => l >= LogLevel.Trace);
            var logger = loggerFactory.CreateLogger<Program>();

            HubSpotAuthenticator authenticator = new ApiKeyHubSpotAuthenticator(Environment.GetEnvironmentVariable("HUBSPOT_APIKEY"));
            IHubSpotClient hubspot = new HttpHubSpotClient(authenticator, loggerFactory.CreateLogger<HttpHubSpotClient>());

            var registrations = new[]
            {
                new TypeConverterRegistration { Converter = new StringTypeConverter(), Type = typeof(string) },
                new TypeConverterRegistration { Converter = new LongTypeConverter(), Type = typeof(long) },
                new TypeConverterRegistration { Converter = new LongTypeConverter(), Type = typeof(long?) },
                new TypeConverterRegistration { Converter = new DateTimeTypeConverter(), Type = typeof(DateTimeOffset) },
                new TypeConverterRegistration { Converter = new DateTimeTypeConverter(), Type = typeof(DateTimeOffset?) },
                new TypeConverterRegistration { Converter = new IntTypeConverter(), Type = typeof(int) },
                new TypeConverterRegistration { Converter = new IntTypeConverter(), Type = typeof(int?) },
                new TypeConverterRegistration { Converter = new DecimalTypeConverter(), Type = typeof(decimal) },
                new TypeConverterRegistration { Converter = new DecimalTypeConverter(), Type = typeof(decimal?) },
            };

            var typeStore = new TypeStore(registrations);
            var typeManager = new ContactTypeManager(typeStore);

            var connector = new HubSpotContactConnector(hubspot, typeManager, loggerFactory.CreateLogger<HubSpotContactConnector>());
            

            try
            {
                var me = await connector.GetByIdAsync(4448901);

                var companyMembers = await connector.FindAsync(FilterContacts.ByCompanyId(me.AssociatedCompanyId));

                foreach (var member in companyMembers)
                {
                    Console.WriteLine($"{member.Id} {member.FirstName} {member.LastName} {member.Email}");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex);
            }

            logger.LogInformation("OK");

            Console.ReadKey();
        }
    }
}