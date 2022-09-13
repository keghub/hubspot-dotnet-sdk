using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using EMG.Utilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace HubSpot.Authentication
{
    public class OAuthHubSpotAuthenticator : DelegatingHandler
    {
        private readonly OAuthOptions _options;

        public OAuthHubSpotAuthenticator(IOptions<OAuthOptions> options, HttpMessageHandler handler) : base(handler)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        private readonly ConcurrentDictionary<string, AuthToken> _tokens = new ConcurrentDictionary<string, AuthToken>();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestUri = request.RequestUri;

            var key = ExtractKey(requestUri);

            if (!_tokens.TryGetValue(key, out var token) || !IsTokenValid(token))
            {
                token = await GetToken(requestUri, cancellationToken);

                if (token != null)
                {
                    _tokens.AddOrUpdate(key, token, (k, old) => token);
                }
            }

            if (token != null)
            {
                request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token.Token}");
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            static string ExtractKey(Uri requestUri) => $"{requestUri.Host}:{requestUri.Port}";

            bool IsTokenValid(AuthToken token) => (token.ExpiresOn - Clock.Default.UtcNow) >= _options.ClockSkew;
        }

        private async Task<AuthToken> GetToken(Uri requestUri, CancellationToken cancellationToken)
        {
            var authUri = GetAuthUri(requestUri);

            using var request = new HttpRequestMessage(HttpMethod.Post, authUri);

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.SecretKey,
                ["redirect_uri"] = _options.RedirectUri.ToString(),
                ["refresh_token"] = _options.RefreshToken
            };

            request.Content = new FormUrlEncodedContent(form);

            using var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var jobj = JObject.Parse(content);

            var token = (string)jobj.GetValue("access_token");
            var expiresIn = (long)jobj.GetValue("expires_in");

            return new AuthToken(token, Clock.Default.UtcNow.AddSeconds(expiresIn));

            static Uri GetAuthUri(Uri requestUri)
            {
                UriBuilder builder = new UriBuilder(requestUri.Scheme, requestUri.Host, requestUri.Port)
                {
                    Path = "/oauth/v1/token"
                };

                return builder.Uri;
            }
        }

        private class AuthToken
        {
            public AuthToken(string token, DateTimeOffset expiresOn)
            {
                Token = token ?? throw new ArgumentNullException(nameof(token));
                ExpiresOn = expiresOn;
            }

            public string Token { get; set; }

            public DateTimeOffset ExpiresOn { get; set; }
        }
    }

    public class OAuthOptions
    {
        public string ClientId { get; set; }

        public Uri RedirectUri { get; set; } = new Uri("https://www.hubspot.com");

        public string RefreshToken { get; set; }

        public string SecretKey { get; set; }

        public TimeSpan ClockSkew { get; set; } = TimeSpan.Zero;
    }
}