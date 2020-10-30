using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model.Companies;
using HubSpot.Model.Contacts;
using HubSpot.Model.CRM.Associations;
using HubSpot.Model.Deals;
using HubSpot.Model.Lists;
using HubSpot.Model.Owners;
using HubSpot.Utils;
using Kralizek.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotClient
    {
        // TODO: Remove this code!
        // private readonly ILogger<HttpHubSpotClient> _logger;

        // public HttpHubSpotClient(HubSpotAuthenticator authenticator, ILogger<HttpHubSpotClient> logger) : base(CreateClient(authenticator), SerializerSettings, logger)
        // {
        //     if (authenticator == null) throw new ArgumentNullException(nameof(authenticator));
        //     _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        // }

        // private static HttpClient CreateClient(HubSpotAuthenticator authenticator)
        // {
        //     return new HttpClient(authenticator) { BaseAddress = authenticator.ServiceUri };
        // }

        public static readonly Uri DefaultApiEndpoint = new Uri("https://api.hubapi.com");

        public static void ConfigureJsonSerializer(JsonSerializerSettings settings)
        {
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.Converters = new JsonConverter[]
            {
                new UnixEpochConverter(),
                new StringEnumConverter()
            };
        }

        private readonly ILogger<HttpHubSpotClient> _logger;
        private readonly IHttpRestClient _client;

        public HttpHubSpotClient(IHttpRestClient client, ILogger<HttpHubSpotClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IHubSpotContactClient Contacts => this;

        public IHubSpotCompanyClient Companies => this;

        public IHubSpotDealClient Deals => this;

        public IHubSpotListClient Lists => this;

        public IHubSpotOwnerClient Owners => this;

        public IHubSpotCrmClient Crm => this;

        protected Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string path, TRequest content, IQueryString query = null)
        {
            _logger.LogDebug($"Invoking {nameof(HttpRestClient.SendAsync)}");

            return _client.SendAsync<TRequest, TResponse>(method, path, content, query);
        }

        protected Task<TResponse> SendAsync<TResponse>(HttpMethod method, string path, IQueryString query = null)
        {
            _logger.LogDebug($"Invoking {nameof(HttpRestClient.SendAsync)}");

            return _client.SendAsync<TResponse>(method, path, query);
        }

        protected Task SendAsync<TRequest>(HttpMethod method, string path, TRequest content, IQueryString query = null)
        {
            _logger.LogDebug($"Invoking {nameof(HttpRestClient.SendAsync)}");

            return _client.SendAsync<TRequest>(method, path, content, query);
        }

        protected Task SendAsync(HttpMethod method, string path, IQueryString query = null)
        {
            _logger.LogDebug($"Invoking {nameof(HttpRestClient.SendAsync)}");

            return _client.SendAsync(method, path, query);
        }
    }
}