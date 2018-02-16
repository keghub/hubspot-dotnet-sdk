using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot;
using HubSpot.Contacts;
using HubSpot.Converters;
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
            var loggerFactory = new LoggerFactory().AddConsole((s, l) => l >= LogLevel.Information);
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
            };

            var typeStore = new TypeStore(registrations);
            var typeManager = new ContactTypeManager(typeStore);

            var connector = new HubSpotContactConnector(hubspot.Contacts, typeManager, loggerFactory.CreateLogger<HubSpotContactConnector>());

            try
            {
                var me = await connector.GetByIdAsync(4448901);

                logger.LogInformation(me, c => $"Found {c.FirstName} {c.LastName} ({c.Email})");

                var company = await hubspot.Companies.GetByIdAsync(me.AssociatedCompanyId);

                logger.LogInformation(company, c => $"Found {c.Properties["name"].Value}");

                var contactIds = await hubspot.Companies.GetContactIdsInCompanyAsync(company.Id);

                foreach (var contactId in contactIds.ContactIds)
                {
                    var contact = await connector.GetByIdAsync(contactId);

                    Console.WriteLine($"Found {contact.Id}: {contact.FirstName} {contact.LastName} ({contact.Email})");
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