using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.CRM.Associations;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.CRM.Associations
{
    [TestFixture]
    public class GetAllAsyncTests : AssociationTests
    {
        [Test]
        [AutoData]
        public async Task Request_is_correct(long objectId, int associationTypeId, AssociationIdList result)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Get,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = Object(result)
                }
            };

            var sut = CreateSystemUnderTest(options);

            var response = await sut.GetAllAsync(objectId, associationTypeId);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/crm-associations/v1/associations/{objectId}/HUBSPOT_DEFINED/{associationTypeId}"));
        }

        [Test]
        [AutoData]
        public void Limit_cant_be_greater_than_100(long objectId, int associationTypeId, int limit)
        {
            var sut = CreateSystemUnderTest();

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.GetAllAsync(objectId, associationTypeId, limit: 100 + limit));

        }
    }
}