using System;
using System.Threading.Tasks;
using HubSpot;
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

            var contact = await hubspot.Contacts.GetByEmailAsync(
                                                    email: "renato.golia@educations.com",
                                                    properties: new[] {Properties.FirstName, Properties.LastName, Properties.Email},
                                                    propertyMode: PropertyMode.ValueOnly,
                                                    formSubmissionMode: FormSubmissionMode.None,
                                                    showListMemberships: false
            );

            var contact2 = await hubspot.Contacts.GetByIdAsync(contact.Id);

            Console.WriteLine($"Hello {contact.Properties["firstname"].Value}! Your name is {contact2.Properties["firstname"].Value}");
        }
    }
}
