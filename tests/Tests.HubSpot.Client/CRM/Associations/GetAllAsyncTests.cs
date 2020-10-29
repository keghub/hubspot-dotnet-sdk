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
    public class GetAllAsyncTests
    {
        [Test]
        [CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotCrmAssociationClient sut, long objectId, int associationTypeId)
        {
            var response = await sut.GetAllAsync(objectId, associationTypeId);

            Mock.Get(client)
                .Verify(p => p.SendAsync<AssociationIdList>(HttpMethod.Get, $"/crm-associations/v1/associations/{objectId}/HUBSPOT_DEFINED/{associationTypeId}", It.IsAny<IQueryString>()));
        }

        [Test]
        [CustomAutoData]
        public void Limit_cant_be_greater_than_100(IHubSpotCrmAssociationClient sut, long objectId, int associationTypeId, int limit)
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.GetAllAsync(objectId, associationTypeId, limit: 100 + limit));
        }

        [Test]
        [CustomAutoData]
        public async Task Limit_is_correctly_added_to_queryString([Frozen] IHttpRestClient client, IHubSpotCrmAssociationClient sut, long objectId, int associationTypeId, [System.ComponentModel.DataAnnotations.Range(1, 100)] int limit, AssociationIdList result)
        {
            Assume.That(limit <= 100);

            var response = await sut.GetAllAsync(objectId, associationTypeId, limit);

            Mock.Get(client)
                .Verify(p => p.SendAsync<AssociationIdList>(HttpMethod.Get, $"/crm-associations/v1/associations/{objectId}/HUBSPOT_DEFINED/{associationTypeId}", QueryStringMatcher.That(Contains.Substring($"limit={limit}"))));
        }

        [Test]
        [CustomAutoData]
        public async Task Offset_is_correctly_added_to_queryString([Frozen] IHttpRestClient client, IHubSpotCrmAssociationClient sut, long objectId, int associationTypeId, long offset, AssociationIdList result)
        {
            var response = await sut.GetAllAsync(objectId, associationTypeId, offset: offset);

            Mock.Get(client)
                .Verify(p => p.SendAsync<AssociationIdList>(HttpMethod.Get, $"/crm-associations/v1/associations/{objectId}/HUBSPOT_DEFINED/{associationTypeId}", QueryStringMatcher.That(Contains.Substring($"offset={offset}"))));
        }
    }
}