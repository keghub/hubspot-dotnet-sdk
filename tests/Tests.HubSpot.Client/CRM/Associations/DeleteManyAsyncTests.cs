using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class DeleteManyAsyncTests
    {
        [Test]
        [CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotCrmAssociationClient sut, IReadOnlyList<Association> associationsToDelete)
        {
            await sut.DeleteManyAsync(associationsToDelete);

            Mock.Get(client)
                .Verify(p => p.SendAsync(HttpMethod.Put, "/crm-associations/v1/associations/delete-batch", associationsToDelete, null));
        }

        [Test, CustomAutoData]
        public void Associations_cant_be_null(IHubSpotCrmAssociationClient sut)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.DeleteManyAsync(associations: null));
        }

        [Test, CustomAutoData]
        public async Task No_request_is_sent_if_associations_is_empty([Frozen] IHttpRestClient client, IHubSpotCrmAssociationClient sut)
        {
            await sut.DeleteManyAsync(new Association[0]);

            Mock.Get(client)
                .Verify(p => p.SendAsync(It.IsAny<HttpMethod>(), "/crm-associations/v1/associations/delete-batch", It.IsAny<Association[]>(), null), Times.Never());
        }

        [Test, CustomAutoData]
        public void Batch_size_cant_be_greater_than_100(IHubSpotCrmAssociationClient sut, [MinLength(101)] Association[] associations)
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.DeleteManyAsync(associations));
        }
    }
}