using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using HubSpot;
using Newtonsoft.Json;

namespace Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(FixtureHelper.CreateFixture) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class InlineCustomAutoDataAttribute : InlineAutoDataAttribute
    {
        public InlineCustomAutoDataAttribute(params object[] arguments) : base(FixtureHelper.CreateFixture, arguments) { }
    }

    internal static class FixtureHelper
    {
        public static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization
            {
                GenerateDelegates = true,
                ConfigureMembers = true
            });

            fixture.Register(() => 
            {
                var settings = new JsonSerializerSettings();
                HttpHubSpotClient.ConfigureJsonSerializer(settings);
                return settings;
            });

            fixture.Register((HttpHubSpotClient client) => client.Companies);

            fixture.Register((HttpHubSpotClient client) => client.Contacts);

            fixture.Register((HttpHubSpotClient client) => client.Crm);

            fixture.Register((HttpHubSpotClient client) => client.Deals);

            fixture.Register((HttpHubSpotClient client) => client.Lists);

            fixture.Register((HttpHubSpotClient client) => client.Owners);

            fixture.Register((HttpHubSpotClient client) => client.Contacts.Properties);

            fixture.Register((HttpHubSpotClient client) => client.Contacts.PropertyGroups);

            fixture.Register((HttpHubSpotClient client) => client.Crm.Associations);

            return fixture;
        }
    }
}