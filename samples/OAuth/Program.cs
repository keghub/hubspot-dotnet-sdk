using System;
using System.Linq;
using System.Threading.Tasks;
using HubSpot.Companies;
using HubSpot.Contacts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OAuth
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

            services.AddHubSpot(hs => hs
                    .UseOAuthAuthentication(configuration.GetSection("HubSpot"))
                    .UseCompanyConnector()
                    .UseContactConnector());

            services.AddLogging(b => b.AddConsole());

            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            var companyConnector = serviceProvider.GetRequiredService<IHubSpotCompanyConnector>();

            var contactConnector = serviceProvider.GetRequiredService<IHubSpotContactConnector>();

            try
            {
                var companies = await companyConnector.FindByDomainAsync("educations.com");

                foreach (var company in companies)
                {
                    var members = await contactConnector.FindByCompanyIdAsync<Contact>(company.Id);

                    foreach (var member in members)
                    {
                        Console.WriteLine($"{member.FirstName} {member.LastName} {member.Email}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex);
            }

            logger.LogInformation("OK");
        }
    }
}
