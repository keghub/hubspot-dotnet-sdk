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
    public class MergeAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, long primaryContactId, long secondaryContactId)
        {
            await sut.MergeAsync(primaryContactId, secondaryContactId);

            Mock.Get(client).Verify(p => p.SendAsync<object>(HttpMethod.Post, $"/contacts/v1/contact/merge-vids/{primaryContactId}/", It.IsAny<object>(), null));
        }
    }
}