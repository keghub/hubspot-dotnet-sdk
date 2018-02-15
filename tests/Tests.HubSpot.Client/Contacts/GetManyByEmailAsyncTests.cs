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
    public class GetManyByEmailAsyncTests : ContactTests
    {
        [Test]
        [AutoData]
        public async Task Request_is_correct(string[] emails)
        {
            var option = new HttpMessageOptions
            {
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(option);

            var response = await sut.GetManyByEmailAsync(emails);

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring("/contacts/v1/contact/emails/batch/"));

            foreach (var email in emails)
            {
                Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.Query, Contains.Substring($"email={email}"));
            }
        }

        [Test]
        public async Task Returns_empty_dictionary_if_no_id()
        {
            var sut = CreateSystemUnderTest();

            var response = await sut.GetManyByEmailAsync(Array.Empty<string>());

            Assert.That(response, Is.Empty);
        }

        [Test]
        public async Task Returns_empty_dictionary_if_null()
        {
            var sut = CreateSystemUnderTest();

            var response = await sut.GetManyByEmailAsync(null);

            Assert.That(response, Is.Empty);
        }

        [Test, TestCase]
        public void Throws_OutOfRange_if_more_than_100([Random(101, 1000, 1)] int count)
        {
            var sut = CreateSystemUnderTest();

            var emails = new string[count];

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.GetManyByEmailAsync(emails));
        }
    }
}