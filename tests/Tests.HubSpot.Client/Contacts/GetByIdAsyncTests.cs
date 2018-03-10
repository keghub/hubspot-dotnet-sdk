using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Model;
using HubSpot.Model.Contacts;
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
        public void NotFoundException_is_thrown_if_404(long contactId)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Get,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = JsonContent.EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(options);

            Assert.ThrowsAsync<NotFoundException>(() => sut.GetByIdAsync(contactId));
        }

        [Test]
        [AutoData]
        public async Task Request_absolutePath_is_correct(long contactId)
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

        [Test, AutoData]
        public async Task Properties_are_added_to_queryString(long contactId, Property[] properties) 
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

            var response = await sut.GetByIdAsync(contactId, properties);

            foreach (var property in properties)
            {
                Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.Query, Contains.Substring($"property={property.Name}"));
            }
        }

        [Test]
        [InlineAutoData(PropertyMode.ValueAndHistory, "value_and_history")]
        [InlineAutoData(PropertyMode.ValueOnly, "value_only")]
        public async Task PropertyMode_is_added_to_queryString(PropertyMode propertyMode, string queryStringValue, long contactId)
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

            var response = await sut.GetByIdAsync(contactId, propertyMode: propertyMode);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.Query, Contains.Substring($"propertyMode={queryStringValue}"));
        }

        [Test]
        [InlineAutoData(FormSubmissionMode.All, "all")]
        [InlineAutoData(FormSubmissionMode.Newest, "newest")]
        [InlineAutoData(FormSubmissionMode.Oldest, "oldest")]
        [InlineAutoData(FormSubmissionMode.None, "none")]
        public async Task FormSubmissionMode_is_added_to_queryString(FormSubmissionMode formSubmissionMode, string queryStringValue, long contactId)
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

            var response = await sut.GetByIdAsync(contactId, formSubmissionMode: formSubmissionMode);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.Query, Contains.Substring($"formSubmissionMode={queryStringValue}"));
        }

        [Test]
        [InlineAutoData(true, "true")]
        [InlineAutoData(false, "false")]
        public async Task ShowListMemberships_is_added_to_queryString(bool showListMemberships, string queryStringValue, long contactId)
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

            var response = await sut.GetByIdAsync(contactId, showListMemberships: showListMemberships);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.Query, Contains.Substring($"showListMemberships={queryStringValue}"));
        }
    }
}