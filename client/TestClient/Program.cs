using System;
using HubSpot;
using Microsoft.Extensions.Logging;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerFactory loggerFactory = new LoggerFactory();

            HubSpotAuthenticator authenticator = null;
            IHubSpotClient hubspot = new HttpHubSpotClient(authenticator, loggerFactory.CreateLogger<HttpHubSpotClient>());

            Console.WriteLine("Hello World!");
        }
    }
}
