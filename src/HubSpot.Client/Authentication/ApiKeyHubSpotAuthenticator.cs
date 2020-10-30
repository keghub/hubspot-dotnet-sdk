using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kralizek.Extensions.Http;
using Microsoft.Extensions.Options;

namespace HubSpot.Authentication
{
    public class ApiKeyHubSpotAuthenticator : DelegatingHandler
    {
        private readonly ApiKeyOptions _options;

        public ApiKeyHubSpotAuthenticator(IOptions<ApiKeyOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uri = request.RequestUri;
            var uriBuilder = new UriBuilder(uri);

            var queryStringBuilder = HttpQueryStringBuilder.ParseQuery(uriBuilder.Query);

            queryStringBuilder.Add("hapikey", _options.ApiKey);

            uriBuilder.Query = queryStringBuilder.BuildQuery();
            request.RequestUri = uriBuilder.Uri;

            return base.SendAsync(request, cancellationToken);
        }
    }

    public class ApiKeyOptions
    {
        public string ApiKey { get; set; }
    }
}