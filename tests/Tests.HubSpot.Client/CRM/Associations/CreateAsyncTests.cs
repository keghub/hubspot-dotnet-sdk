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
    public class CreateAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotCrmAssociationClient sut, Association associationToCreate)
        {
            await sut.CreateAsync(associationToCreate);

            Mock.Get(client).Verify(p => p.SendAsync(HttpMethod.Put, "/crm-associations/v1/associations", associationToCreate, null));
        }

        [Test, CustomAutoData]
        public void Item_to_create_cant_be_null(IHubSpotCrmAssociationClient sut)
        {
            Assert.That(() => sut.CreateAsync(null), Throws.ArgumentNullException);
        }
    }
}