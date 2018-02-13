using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model.Contacts;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts
{
    [TestFixture]
    public class GetRecentlyCreatedAsyncTests : ContactTests
    {
        [Test]
        public async Task Request_is_correct()
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Get,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = Object(new ContactList())
                }
            };

            var sut = CreateSystemUnderTest(options);

            var response = await sut.GetRecentlyCreatedAsync();

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring("/contacts/v1/lists/all/contacts/recent"));
        }
    }
}