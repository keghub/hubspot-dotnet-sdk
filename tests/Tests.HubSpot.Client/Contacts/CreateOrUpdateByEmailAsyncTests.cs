using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts
{
    [TestFixture]
    public class CreateOrUpdateByEmailAsyncTests : ContactTests
    {
        [Test, AutoData]
        public async Task Request_is_correct(string email, Contact contact)
        {
            var properties = (from p in contact.Properties
                              select new ValuedProperty(p.Key, p.Value.Value)).ToArray();

            var option = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Post,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = Object(contact)
                }
            };

            var sut = CreateSystemUnderTest(option);

            await sut.CreateOrUpdateByEmailAsync(email, properties);

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/contacts/v1/contact/createOrUpdate/email/{email}"));
        }
    }
}