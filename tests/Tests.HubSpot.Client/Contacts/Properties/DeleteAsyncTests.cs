using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts.Properties
{
    [TestFixture]
    public class DeleteAsyncTests : ContactPropertyTests
    {
        [Test, AutoData]
        public async Task Request_is_correct(string propertyName)
        {
            var option = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Delete,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(option);

            await sut.DeleteAsync(propertyName);

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/properties/v1/contacts/properties/named/{propertyName}"));
        }
    }
}