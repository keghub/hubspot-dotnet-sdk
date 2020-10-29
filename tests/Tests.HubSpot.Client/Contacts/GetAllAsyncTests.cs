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
    public class GetAllAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut)
        {
            var response = await sut.GetAllAsync();

            Mock.Get(client).Verify(p => p.SendAsync<ContactList>(HttpMethod.Get, "/contacts/v1/lists/all/contacts/all", It.IsAny<IQueryString>()));
        }
    }
}