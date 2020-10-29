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
    public class SearchAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, string query)
        {
            var response = await sut.SearchAsync(query);

            Mock.Get(client)
                .Verify(p => p.SendAsync<SearchResponse>(HttpMethod.Get, "/contacts/v1/search/query", It.IsAny<IQueryString>()));
        }

        [Test, CustomAutoData]
        public async Task Query_is_attached_to_request([Frozen] IHttpRestClient client, IHubSpotContactClient sut, string query)
        {
            var response = await sut.SearchAsync(query);

            Mock.Get(client)
                .Verify(p => p.SendAsync<SearchResponse>(HttpMethod.Get, "/contacts/v1/search/query", QueryStringMatcher.That(Does.Contain($"q={query}"))));
        }
    }
}