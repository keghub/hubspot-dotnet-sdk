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
    public class UpdateAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactPropertyClient sut, string propertyName, ContactProperty property)
        {
            var response = await sut.UpdateAsync(propertyName, property);

            Mock.Get(client)
                .Verify(p => p.SendAsync<ContactProperty, ContactProperty>(HttpMethod.Put, $"/properties/v1/contacts/properties/named/{propertyName}", property, null));
        }
    }
}