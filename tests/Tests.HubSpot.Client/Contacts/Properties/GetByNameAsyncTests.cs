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
    public class GetByNameAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactPropertyClient sut, string propertyName)
        {
            var response = await sut.GetByNameAsync(propertyName);

            Mock.Get(client)
                .Verify(p => p.SendAsync<ContactProperty>(HttpMethod.Get, $"/properties/v1/contacts/properties/named/{propertyName}", null));
        }
    }
}