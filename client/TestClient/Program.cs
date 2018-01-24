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

            //var contact = await hubspot.Contacts.GetByEmailAsync(
            //                                        email: "renato.golia@educations.com",
            //                                        properties: new[] {ContactProperties.FirstName, ContactProperties.LastName, ContactProperties.Email},
            //                                        propertyMode: PropertyMode.ValueOnly,
            //                                        formSubmissionMode: FormSubmissionMode.None,
            //                                        showListMemberships: false
            //);

            //var contact2 = await hubspot.Contacts.GetByIdAsync(contact.Id);

            //Console.WriteLine($"Hello {contact.ContactProperties["firstname"].Value}! Your name is {contact2.ContactProperties["firstname"].Value}");

            //var list = await hubspot.Contacts.GetManyByIdAsync(new[] {contact.Id});

            //var list = await hubspot.Contacts.GetManyByEmailAsync(new[] {"renato.golia@educations.com", "renato.golia@studentum.se"});

            var companies = await hubspot.Companies.SearchAsync("studentum.se", new Property[] { "domain", "name", "createdate" });

            foreach (var company in companies.Companies)
            {
                var contacts = await hubspot.Companies.GetContactsInCompanyAsync(company.Id);

                Console.WriteLine($"{company.Properties["name"].Value} has{(contacts.HasMore ? " at least" : string.Empty)} {contacts.Contacts.Count} contacts");

                foreach (var contact in contacts.Contacts)
                {
                    Console.WriteLine($"{contact.Id} {contact.Properties.SingleOrDefault(p => p.Name == "firstname")?.Value} {contact.Properties.SingleOrDefault(p => p.Name == "lastname")?.Value}");
                }
            }

            Console.WriteLine("Bye");
        }
    }
}
