using System;
using System.ComponentModel.DataAnnotations;
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
    public class CreateManyAsyncTests : AssociationTests
    {
        [Test]
        [AutoData]
        public async Task Request_is_correct(Association[] associationsToCreate)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Put,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent)
            };

            var sut = CreateSystemUnderTest(options);

            await sut.CreateManyAsync(associationsToCreate);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring("/crm-associations/v1/associations/create-batch"));

        }

        [Test]
        public void Associations_cant_be_null()
        {
            var sut = CreateSystemUnderTest();

            Assert.ThrowsAsync<ArgumentNullException>(() => sut.CreateManyAsync(associations: null));
        }

        [Test]
        public async Task No_request_is_sent_if_associations_is_empty()
        {
            var options = new HttpMessageOptions();

            var sut = CreateSystemUnderTest(options);

            await sut.CreateManyAsync(new Association[0]);

            Assert.That(options.NumberOfTimesCalled, Is.EqualTo(0));
        }

        [Test, AutoData]
        public void Batch_size_cant_be_greater_than_100([MinLength(101)] Association[] associations)
        {
            var options = new HttpMessageOptions();

            var sut = CreateSystemUnderTest(options);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => sut.CreateManyAsync(associations));
        }
    }
}