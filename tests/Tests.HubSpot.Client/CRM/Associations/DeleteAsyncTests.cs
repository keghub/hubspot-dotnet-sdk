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
    public class DeleteAsyncTests : AssociationTests
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

            await sut.DeleteAsync(associationToCreate);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring("/crm-associations/v1/associations/delete"));

        }
    }
}