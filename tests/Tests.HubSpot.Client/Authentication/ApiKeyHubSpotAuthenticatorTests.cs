﻿using System;
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
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;


namespace Tests.Authentication
{
    // [TestFixture]
    // public class ApiKeyHubSpotAuthenticatorTests
    // {
    //     private HttpClient CreateClient(string apiKey, HttpMessageOptions options)
    //     {
    //         var handler = new FakeHttpMessageHandler(options);

    //         var authenticator = new ApiKeyHubSpotAuthenticator(apiKey) { InnerHandler = handler };

    //         var client = new HttpClient(authenticator);

    //         return client;
    //     }

    //     [Test]
    //     [InlineAutoData("GET")]
    //     [InlineAutoData("POST")]
    //     [InlineAutoData("DELETE")]
    //     [InlineAutoData("PUT")]
    //     public async Task ApiKey_is_attached_to_request_in_querystring(string methodName, string apiKey, Uri requestUri)
    //     {
    //         var method = new HttpMethod(methodName);

    //         var options = new HttpMessageOptions
    //         {
    //             HttpMethod = method,
    //             HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
    //         };

    //         var client = CreateClient(apiKey, options);

    //         using (var testRequest = new HttpRequestMessage(method, requestUri))
    //         {
    //             var response = await client.SendAsync(testRequest);
    //         }

    //         Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.Query, Contains.Substring($"hapikey={apiKey}"));
    //     }
    // }
}
