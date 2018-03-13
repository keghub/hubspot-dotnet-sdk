using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using EMG.Common;
using EMG.Testing;
using HubSpot.Authentication;
using Kralizek.Extensions.Http;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Authentication {
    [TestFixture]
    public class OAuthHubSpotAuthenticatorTests
    {
        private HttpClient CreateClient(OAuthOptions options, params HttpMessageOptions[] httpOptions)
        {
            var handler = new FakeHttpMessageHandler(httpOptions);

            var wrapper = new OptionsWrapper<OAuthOptions>(options);
            var authenticator = new OAuthHubSpotAuthenticator(wrapper) { InnerHandler = handler };

            var client = new HttpClient(authenticator, false);

            return client;
        }

        [Test]
        [InlineAutoData("GET")]
        [InlineAutoData("POST")]
        [InlineAutoData("DELETE")]
        [InlineAutoData("PUT")]
        public async Task Single_request_is_authenticated_and_executed(string methodName, OAuthOptions options, Uri requestUri, string accessToken, long expiresIn, TestClock clock)
        {
            Clock.Default = clock;

            var method = new HttpMethod(methodName);

            var refreshTokenOptions = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Post,
                RequestUri = new Uri("https://api.hubapi.com/oauth/v1/token"),
                HttpContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "refresh_token",
                    ["client_id"] = options.ClientId,
                    ["client_secret"] = options.SecretKey,
                    ["redirect_uri"] = options.RedirectUri.ToString(),
                    ["refresh_token"] = options.RefreshToken
                }),
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.FromObject(new
                    {
                        access_token = accessToken,
                        expires_in = expiresIn
                    })
                }
            };

            var randomRequestOptions = new HttpMessageOptions
            {
                RequestUri = requestUri,
                HttpMethod = method,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            };

            var client = CreateClient(options, refreshTokenOptions, randomRequestOptions);

            using (var testRequest = new HttpRequestMessage(method, requestUri))
            {
                var response = await client.SendAsync(testRequest);
            }

            Assert.That(refreshTokenOptions.NumberOfTimesCalled, Is.GreaterThan(0));

            Assert.That(randomRequestOptions.HttpResponseMessage.RequestMessage.Headers.Authorization.Parameter, Is.EqualTo(accessToken));
        }

        [Test]
        [InlineAutoData("GET")]
        [InlineAutoData("POST")]
        [InlineAutoData("DELETE")]
        [InlineAutoData("PUT")]
        public async Task Multiple_requests_reuse_the_same_token(string methodName, OAuthOptions options, Uri requestUri, string accessToken, long expiresIn, TestClock clock)
        {
            Clock.Default = clock;

            var method = new HttpMethod(methodName);

            var httpOptions = new List<HttpMessageOptions>();

            var refreshTokenOptions = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Post,
                RequestUri = new Uri("https://api.hubapi.com/oauth/v1/token"),
                HttpContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "refresh_token",
                    ["client_id"] = options.ClientId,
                    ["client_secret"] = options.SecretKey,
                    ["redirect_uri"] = options.RedirectUri.ToString(),
                    ["refresh_token"] = options.RefreshToken
                }),
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.FromObject(new
                    {
                        access_token = accessToken,
                        expires_in = expiresIn
                    })
                }
            };

            httpOptions.Add(refreshTokenOptions);

            var option = new HttpMessageOptions
            {
                RequestUri = requestUri,
                HttpMethod = method,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            };

            httpOptions.Add(option);

            var client = CreateClient(options, httpOptions.ToArray());

            for (int i = 0; i < 2; i++)
            {
                var testRequest = new HttpRequestMessage(method, requestUri);
                var response = await client.SendAsync(testRequest);
            }

            Assert.That(refreshTokenOptions.NumberOfTimesCalled, Is.EqualTo(1));

        }

        [Test]
        [InlineAutoData("GET")]
        [InlineAutoData("POST")]
        [InlineAutoData("DELETE")]
        [InlineAutoData("PUT")]
        public async Task Token_is_refreshed_if_expired(string methodName, OAuthOptions options, Uri requestUri, string accessToken, long expiresIn, TestClock clock)
        {
            Clock.Default = clock;

            var method = new HttpMethod(methodName);

            var httpOptions = new List<HttpMessageOptions>();

            var refreshTokenOptions = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Post,
                RequestUri = new Uri("https://api.hubapi.com/oauth/v1/token"),
                HttpContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "refresh_token",
                    ["client_id"] = options.ClientId,
                    ["client_secret"] = options.SecretKey,
                    ["redirect_uri"] = options.RedirectUri.ToString(),
                    ["refresh_token"] = options.RefreshToken
                }),
                HttpResponseMessage = CreateNewResponse()
            };

            httpOptions.Add(refreshTokenOptions);


            var requestOption = new HttpMessageOptions
            {
                RequestUri = requestUri,
                HttpMethod = method,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            };

            httpOptions.Add(requestOption);

            var client = CreateClient(options, httpOptions.ToArray());

            await MakeARequest();

            clock.AdvanceBy(TimeSpan.FromSeconds(expiresIn + 1));

            refreshTokenOptions.HttpResponseMessage = CreateNewResponse();

            await MakeARequest();

            Assert.That(refreshTokenOptions.NumberOfTimesCalled, Is.EqualTo(2));

            HttpResponseMessage CreateNewResponse()
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.FromObject(new
                    {
                        access_token = accessToken,
                        expires_in = expiresIn
                    })
                };
            }

            async Task MakeARequest()
            {
                var request = new HttpRequestMessage(method, requestUri);
                var response = await client.SendAsync(request);
            }
        }

        [Test]
        [InlineAutoData("GET")]
        [InlineAutoData("POST")]
        [InlineAutoData("DELETE")]
        [InlineAutoData("PUT")]
        public async Task Token_is_not_attached_if_request_fails(string methodName, OAuthOptions options, Uri requestUri, string accessToken, long expiresIn, TestClock clock)
        {
            Clock.Default = clock;

            var method = new HttpMethod(methodName);

            var httpOptions = new List<HttpMessageOptions>();

            var refreshTokenOptions = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Post,
                RequestUri = new Uri("https://api.hubapi.com/oauth/v1/token"),
                HttpContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "refresh_token",
                    ["client_id"] = options.ClientId,
                    ["client_secret"] = options.SecretKey,
                    ["redirect_uri"] = options.RedirectUri.ToString(),
                    ["refresh_token"] = options.RefreshToken
                }),
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            };

            httpOptions.Add(refreshTokenOptions);


            var requestOption = new HttpMessageOptions
            {
                RequestUri = requestUri,
                HttpMethod = method,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            };

            httpOptions.Add(requestOption);

            var client = CreateClient(options, httpOptions.ToArray());

            var request = new HttpRequestMessage(method, requestUri);
            var response = await client.SendAsync(request);

            Assert.That(requestOption.HttpResponseMessage.RequestMessage.Headers.Authorization, Is.Null);

        }
    }
}