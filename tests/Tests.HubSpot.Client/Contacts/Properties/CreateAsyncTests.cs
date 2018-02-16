using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.Contacts.Properties;
using Kralizek.Extensions.Http;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts.Properties
{
    [TestFixture]
    public class CreateAsyncTests : ContactPropertyTests
    {
        [Test, AutoData]
        public async Task Request_is_correct(ContactProperty property)
        {
            var option = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Post,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(option);

            var response = await sut.CreateAsync(property);

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring("/properties/v1/contacts/properties"));
        }
    }
}