using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.Contacts.Properties;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;


namespace Tests.Contacts.Properties
{
    [TestFixture]
    public class CreateAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactPropertyClient sut, ContactProperty property)
        {
            var response = await sut.CreateAsync(property);

            Mock.Get(client)
                .Verify(p => p.SendAsync<ContactProperty, ContactProperty>(HttpMethod.Post, "/properties/v1/contacts/properties", property, null));
        }
    }
}