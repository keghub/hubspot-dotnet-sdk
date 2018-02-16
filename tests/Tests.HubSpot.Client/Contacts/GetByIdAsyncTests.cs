using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Kralizek.Extensions.Http;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts
{
    [TestFixture]
    public class GetByIdAsyncTests : ContactTests
    {
        [Test]
        [AutoData]
        public async Task Request_is_correct(long contactId)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Get,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(options);

            var response = await sut.GetByIdAsync(contactId);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/contacts/v1/contact/vid/{contactId}/profile"));
        }
    }
}