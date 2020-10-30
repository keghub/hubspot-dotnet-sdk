using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using WorldDomination.Net.Http;

namespace Tests.Authentication
{
    [TestFixture]
    public class ApiKeyHubSpotAuthenticatorTests
    {
        private HttpClient CreateClient(ApiKeyOptions options, HttpMessageOptions httpOptions)
        {
            var handler = new FakeHttpMessageHandler(httpOptions);

            var wrapper = new OptionsWrapper<ApiKeyOptions>(options);

            var authenticator = new ApiKeyHubSpotAuthenticator(wrapper) { InnerHandler = handler };

            var client = new HttpClient(authenticator);

            return client;
        }

        [Test]
        [InlineAutoData("GET")]
        [InlineAutoData("POST")]
        [InlineAutoData("DELETE")]
        [InlineAutoData("PUT")]
        public async Task ApiKey_is_attached_to_request_in_querystring(string methodName, ApiKeyOptions options, Uri requestUri)
        {
            var method = new HttpMethod(methodName);

            var httpOptions = new HttpMessageOptions
            {
                HttpMethod = method,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            };

            var client = CreateClient(options, httpOptions);

            using (var testRequest = new HttpRequestMessage(method, requestUri))
            {
                var response = await client.SendAsync(testRequest);
            }

            Assert.That(httpOptions.HttpResponseMessage.RequestMessage.RequestUri.Query, Contains.Substring($"hapikey={options.ApiKey}"));
        }
    }
}
