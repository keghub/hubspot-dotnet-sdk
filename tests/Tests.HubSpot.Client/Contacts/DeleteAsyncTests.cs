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
    public class DeleteAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, long contactId)
        {
            var actual = await sut.DeleteAsync(contactId);

            Mock.Get(client).Verify(p => p.SendAsync<DeleteContactResponse>(HttpMethod.Delete, $"/contacts/v1/contact/vid/{contactId}", null));
        }
    }
}