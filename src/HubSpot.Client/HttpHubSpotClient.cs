﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model.Companies;
using HubSpot.Model.Contacts;
using HubSpot.Model.CRM.Associations;
using HubSpot.Model.Deals;
using HubSpot.Model.Lists;
using HubSpot.Model.Owners;
using HubSpot.Model.Pipelines;
using HubSpot.Utils;
using Kralizek.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotClient
    {
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

        public IHubSpotPipelineClient Pipelines => this;

    }
}