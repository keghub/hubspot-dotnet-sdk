using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HubSpot;
using HubSpot.Authentication;
using HubSpot.Companies;
using HubSpot.Contacts;
using HubSpot.Converters;
using HubSpot.Deals;
using HubSpot.Internal;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loggerFactory = new LoggerFactory().AddConsole((s, l) => l >= LogLevel.Trace);
            var logger = loggerFactory.CreateLogger<Program>();

            LoggingHandler logging = new LoggingHandler(loggerFactory.CreateLogger<LoggingHandler>());
            HubSpotAuthenticator authenticator = GetAuthenticator();// { InnerHandler = logging };
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

            var companyTypeManager = new CompanyTypeManager(typeStore);

            var companyConnector = new HubSpotCompanyConnector(hubspot, companyTypeManager, loggerFactory.CreateLogger<HubSpotCompanyConnector>());

            var contactTypeManager = new ContactTypeManager(typeStore);

            var contactConnector = new HubSpotContactConnector(hubspot, contactTypeManager, loggerFactory.CreateLogger<HubSpotContactConnector>());

            var dealTypeManager = new DealTypeManager(typeStore);

            var dealConnector = new HubSpotDealConnector(hubspot, dealTypeManager, loggerFactory.CreateLogger<HubSpotDealConnector>());


            try
            {
                var companies = await companyConnector.FindByDomainAsync("educations.com");

                var emg = companies.Single();

                emg.Name = $"Educations Media Group";

                var members = await contactConnector.FindByCompanyIdAsync<HubSpot.Contacts.Contact>(emg.Id);

                foreach (var member in members)
                {
                    Console.WriteLine($"{member.FirstName} {member.LastName} {member.Email}");
                }

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex);
            }

            logger.LogInformation("OK");
         }

        private static HubSpotAuthenticator GetAuthenticator()
        {
            var options = new OptionsWrapper<OAuthOptions>(new OAuthOptions
            {
                ClientId = "clientId",
                SecretKey = "secretKey",
                RedirectUri = new Uri("https://www.hubspot.com"),
                RefreshToken = "refreshToken"
            });

            return new OAuthHubSpotAuthenticator(options);

            //return new ApiKeyHubSpotAuthenticator(Environment.GetEnvironmentVariable("HUBSPOT_APIKEY"));
        }
    }

    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                _logger.LogTrace(requestBody);
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.Content != null)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogTrace(responseBody);
            }

            return response;
        }
    }
}