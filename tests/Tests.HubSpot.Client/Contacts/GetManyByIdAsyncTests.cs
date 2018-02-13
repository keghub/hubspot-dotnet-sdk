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
    public class GetManyByIdAsyncTests : ContactTests
    {
        [Test]
        [AutoData]
        public async Task Request_is_correct(long[] contactIds)
        {
            var option = new HttpMessageOptions
            {
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(option);

            var response = await sut.GetManyByIdAsync(contactIds);

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring("/contacts/v1/contact/vids/batch/"));

            foreach (var id in contactIds)
            {
                Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.Query, Contains.Substring($"vid={id}"));
            }
        }

        [Test]
        public async Task Returns_empty_dictionary_if_no_id()
        {
            var sut = CreateSystemUnderTest();

            var response = await sut.GetManyByIdAsync(Array.Empty<long>());

            Assert.That(response, Is.Empty);
        }

        [Test]
        public async Task Returns_empty_dictionary_if_null()
        {
            var sut = CreateSystemUnderTest();

            var response = await sut.GetManyByIdAsync(null);

            Assert.That(response, Is.Empty);
        }

        [Test]
        public void Throws_OutOfRange_if_more_than_100([Random(101, 1000, 1)] int count)
        {
            var sut = CreateSystemUnderTest();

            var contactIds = new long[count];

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.GetManyByIdAsync(contactIds));
        }
    }
}