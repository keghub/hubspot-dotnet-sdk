using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HubSpot;
using HubSpot.Companies;
using HubSpot.Contacts;
using HubSpot.Deals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["HubSpot:ClientId"] = "<client id>",
                ["HubSpot:SecretKey"] = "<secret key>",
                ["HubSpot:RedirectUri"] = "https://www.hubspot.com",
                ["HubSpot:RefreshToken"] = "<refresh token>"
            });

            configurationBuilder.AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();

            IServiceCollection services = new ServiceCollection();

            services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));

            services.AddHubSpot(c =>
            {
                //c.UseApiKey(configuration);

                c.UseOAuth(configuration.GetSection("HubSpot"));

                c.RegisterDefaultConverters();

                c.UseCompanyConnector();

                c.UseDealConnector();

                c.UseContactConnector();
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            var logger = loggerFactory.CreateLogger<Program>();


            var companyConnector = serviceProvider.GetRequiredService<IHubSpotCompanyConnector>();

            var contactConnector = serviceProvider.GetRequiredService<IHubSpotContactConnector>();

            var dealConnector = serviceProvider.GetRequiredService<IHubSpotDealConnector>();

            try
            {
                var companies = await companyConnector.FindByDomainAsync("educations.com");

                var emg = companies.Single();

                var members = await contactConnector.FindByCompanyIdAsync<Contact>(emg.Id);

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