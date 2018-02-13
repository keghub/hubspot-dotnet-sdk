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
    public class UpdateByEmailAsyncTests : ContactTests
    {
        private IFixture fixture;

        [SetUp]
        public void Initialize()
        {
            fixture = new Fixture();

            fixture.Customizations.Add(new TypeRelay(typeof(IReadOnlyDictionary<string, VersionedProperty>), typeof(Dictionary<string, VersionedProperty>)));
        }

        [Test, AutoData]
        public async Task Request_is_correct(string email)
        {
            var contact = fixture.Create<Contact>();

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

            await sut.UpdateByEmailAsync(email, properties);

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/contacts/v1/contact/email/{email}/profile"));
        }
    }
}