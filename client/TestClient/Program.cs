using System;
using System.Linq;
using System.Threading.Tasks;
using HubSpot;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Microsoft.Extensions.Logging;

namespace TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loggerFactory = new LoggerFactory().AddConsole((s, l) => true);

            HubSpotAuthenticator authenticator = new ApiKeyHubSpotAuthenticator(Environment.GetEnvironmentVariable("HUBSPOT_APIKEY"));
            IHubSpotClient hubspot = new HttpHubSpotClient(authenticator, loggerFactory.CreateLogger<HttpHubSpotClient>());
            try
            {
                var lists = await hubspot.Lists.GetAllAsync();

                foreach (var list in lists.Lists)
                {
                    Console.WriteLine($"{list.ListId} {list.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }

            Console.WriteLine("Bye");
        }
    }
}
