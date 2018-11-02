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
    public class DeleteManyAsyncTests : AssociationTests
    {
        [Test]
        [AutoData]
        public async Task Request_is_correct(Association[] associationsToDelete)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Put,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent)
            };

            var sut = CreateSystemUnderTest(options);

            await sut.DeleteManyAsync(associationsToDelete);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring("/crm-associations/v1/associations/delete-batch"));

        }

        [Test]
        public void Associations_cant_be_null()
        {
            var sut = CreateSystemUnderTest();

            Assert.ThrowsAsync<ArgumentNullException>(() => sut.DeleteManyAsync(associations: null));
        }

        [Test]
        public async Task No_request_is_sent_if_associations_is_empty()
        {
            var options = new HttpMessageOptions();

            var sut = CreateSystemUnderTest(options);

            await sut.DeleteManyAsync(new Association[0]);

            Assert.That(options.NumberOfTimesCalled, Is.EqualTo(0));
        }
    }
}