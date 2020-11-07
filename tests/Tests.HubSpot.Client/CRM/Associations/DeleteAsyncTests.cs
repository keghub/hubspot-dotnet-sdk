using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.CRM.Associations;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;


namespace Tests.CRM.Associations
{
    [TestFixture]
    public class DeleteAsyncTests
    {
        [Test]
        [CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotCrmAssociationClient sut, Association associationToDelete)
        {
            await sut.DeleteAsync(associationToDelete);

            Mock.Get(client)
                .Verify(p => p.SendAsync(HttpMethod.Put, "/crm-associations/v1/associations/delete", associationToDelete, null));

        }

        [Test, CustomAutoData]
        public void Item_to_delete_cant_be_null(IHubSpotCrmAssociationClient sut)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.DeleteAsync(null));
        }
    }
}