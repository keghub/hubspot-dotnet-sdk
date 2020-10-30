using System;
using System.Linq;
using System.Threading.Tasks;
using HubSpot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RawOAuth
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddObject(new 
            {
                HubSpot = new 
                {
                    ClientId = "<client id>",
                    SecretKey = "<secret key>",
                    RedirectUri = "https://www.hubspot.com",
                    RefreshToken = "<refresh token>"
                }
            });

            configurationBuilder.AddUserSecrets<Program>();

            configurationBuilder.AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddHubSpotClient(client => 
            {
                client.UseOAuthAuthentication(configuration.GetSection("HubSpot"));
            });

            services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<IHubSpotClient>();

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                var companies = await client.Companies.SearchAsync("educations.com");

                var thisCompany = companies.Companies.First();

                logger.LogInformation($"Company ID: {thisCompany.Id}");

                var contacts = await client.Companies.GetContactIdsInCompanyAsync(thisCompany.Id);

                logger.LogInformation($"Found {contacts.ContactIds.Count} contacts");
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }           
        }
    }
}
