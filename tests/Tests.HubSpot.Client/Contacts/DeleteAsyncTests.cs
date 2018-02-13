using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.Contacts;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts
{
    [TestFixture]
    public class DeleteAsyncTests : ContactTests
    {
        [Test, AutoData]
        public async Task Request_is_correct(long contactId)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Delete,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = Object(new ContactList())
                }
            };

            var sut = CreateSystemUnderTest(options);

            var response = await sut.DeleteAsync(contactId);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/contacts/v1/contact/vid/{contactId}"));
        }
    }

    [TestFixture]
    public class MergeAsyncTests : ContactTests
    {
        [Test, AutoData]
        public async Task Request_is_correct(long primaryContactId, long secondaryContactId)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Post,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = Object(new ContactList())
                }
            };

            var sut = CreateSystemUnderTest(options);

            await sut.MergeAsync(primaryContactId, secondaryContactId);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/contacts/v1/contact/merge-vids/{primaryContactId}/"));
        }
    }
}