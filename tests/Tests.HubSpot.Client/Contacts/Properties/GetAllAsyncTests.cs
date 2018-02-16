using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Kralizek.Extensions.Http;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts.Properties
{
    [TestFixture]
    public class GetAllAsyncTests : ContactPropertyTests
    {
        [Test]
        public async Task Request_is_correct()
        {
            var option = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Get,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.EmptyArray
                }
            };

            var sut = CreateSystemUnderTest(option);

            var response = await sut.GetAllAsync();

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring("/properties/v1/contacts/properties"));
        }
    }
}