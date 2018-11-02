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
    public class CreateAsyncTests : AssociationTests
    {
        [Test]
        [AutoData]
        public async Task Request_is_correct(Association associationToCreate)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Put,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent)
            };

            var sut = CreateSystemUnderTest(options);

            await sut.CreateAsync(associationToCreate);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring("/crm-associations/v1/associations"));

        }

        [Test]
        public void Item_to_create_cant_be_null()
        {
            var sut = CreateSystemUnderTest();

            Assert.ThrowsAsync<ArgumentNullException>(() => sut.CreateAsync(null));
        }
    }
}