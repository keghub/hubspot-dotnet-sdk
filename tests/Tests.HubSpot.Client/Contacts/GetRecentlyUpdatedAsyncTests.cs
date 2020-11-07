using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.Contacts;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;


namespace Tests.Contacts
{
    [TestFixture]
    public class GetRecentlyUpdatedAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut)
        {
            var response = await sut.GetRecentlyUpdatedAsync();

            Mock.Get(client)
                .Verify(p => p.SendAsync<ContactList>(HttpMethod.Get, "/contacts/v1/lists/recently_updated/contacts/recent", It.IsAny<IQueryString>()));
        }
    }
}