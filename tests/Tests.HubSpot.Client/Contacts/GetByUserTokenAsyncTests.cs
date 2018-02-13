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
    public class GetByUserTokenAsyncTests : ContactTests
    {
        [Test]
        [AutoData]
        public async Task Request_is_correct(string userToken)
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

            var response = await sut.GetByUserTokenAsync(userToken);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/contacts/v1/contact/utk/{userToken}/profile"));
        }

        [Test]
        public void UserToken_is_required()
        {
            var sut = CreateSystemUnderTest();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetByUserTokenAsync(null));
        }
    }
}