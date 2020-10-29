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
    public class DeleteAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactPropertyClient sut, string propertyName)
        {
            await sut.DeleteAsync(propertyName);

            Mock.Get(client)
                .Verify(p => p.SendAsync(HttpMethod.Delete, $"/properties/v1/contacts/properties/named/{propertyName}", null));
        }
    }
}