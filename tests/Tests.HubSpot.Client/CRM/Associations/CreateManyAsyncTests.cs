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
    public class CreateManyAsyncTests
    {
        [Test]
        [CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotCrmAssociationClient sut, [MaxLength(100)] IReadOnlyList<Association> associationsToCreate)
        {
            await sut.CreateManyAsync(associationsToCreate);

            Mock.Get(client)
                .Verify(p => p.SendAsync(HttpMethod.Put, "/crm-associations/v1/associations/create-batch", associationsToCreate, null));

        }

        [Test, CustomAutoData]
        public void Associations_cant_be_null(IHubSpotCrmAssociationClient sut)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.CreateManyAsync(associations: null));
        }

        [Test, CustomAutoData]
        public async Task No_request_is_sent_if_associations_is_empty([Frozen] IHttpRestClient client, IHubSpotCrmAssociationClient sut)
        {
            await sut.CreateManyAsync(new Association[0]);

            Mock.Get(client)
                .Verify(p => p.SendAsync(It.IsAny<HttpMethod>(), "/crm-associations/v1/associations/create-batch", It.IsAny<Association[]>(), null), Times.Never());
        }

        [Test, CustomAutoData]
        public void Batch_size_cant_be_greater_than_100(IHubSpotCrmAssociationClient sut, [MinLength(101)] Association[] associations)
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.CreateManyAsync(associations));
        }
    }
}