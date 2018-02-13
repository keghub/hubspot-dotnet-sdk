using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts
{
    [TestFixture]
    public class GetByEmailAsyncTests : ContactTests
    {
        [Test]
        public void Email_is_required()
        {
            var option = new HttpMessageOptions();

            var sut = CreateSystemUnderTest(option);

            Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetByEmailAsync(null));
        }

        [Test]
        [AutoData]
        public async Task Request_is_correct(string email)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Get,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(options);

            var response = await sut.GetByEmailAsync(email);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/contacts/v1/contact/email/{email}/profile"));
        }
    }
}