using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HubSpot.Utils;
using Kralizek.Extensions.Http;

namespace HubSpot
{
    public abstract class HubSpotAuthenticator : DelegatingHandler
    {
        public virtual Uri ServiceUri { get; } = new Uri("https://api.hubapi.com");
    }

    public class ApiKeyHubSpotAuthenticator : HubSpotAuthenticator
    {
        private readonly string _apiKey;

        public ApiKeyHubSpotAuthenticator(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            InnerHandler = new HttpClientHandler();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uri = request.RequestUri;
            var uriBuilder = new UriBuilder(uri);

            var queryStringBuilder = HttpQueryStringBuilder.ParseQuery(uriBuilder.Query);

            queryStringBuilder.Add("hapikey", _apiKey);

            uriBuilder.Query = queryStringBuilder.BuildQuery();
            request.RequestUri = uriBuilder.Uri;

            return base.SendAsync(request, cancellationToken);
        }
    }
}