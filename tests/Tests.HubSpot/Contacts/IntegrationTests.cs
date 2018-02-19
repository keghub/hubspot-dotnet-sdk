using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Contacts;
using HubSpot.Converters;
using HubSpot.Internal;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Contact = HubSpot.Contacts.Contact;
using HubSpotContact = HubSpot.Model.Contacts.Contact;

namespace Tests.Contacts
{
    [TestFixture]
    public class IntegrationTests
    {
        private Mock<IHubSpotClient> mockHubSpotClient;
        private Mock<IHubSpotContactClient> mockClient;

        [SetUp]
        public void Initialize()
        {
            mockClient = new Mock<IHubSpotContactClient>();

            mockHubSpotClient = new Mock<IHubSpotClient>();
            mockHubSpotClient.SetupGet(p => p.Contacts).Returns(mockClient.Object);
        }

        private HubSpotContactConnector CreateSystemUnderTest()
        {
            var registrations = new[]
            {
                new TypeConverterRegistration { Converter = new StringTypeConverter(), Type = typeof(string) },
                new TypeConverterRegistration { Converter = new LongTypeConverter(), Type = typeof(long) },
                new TypeConverterRegistration { Converter = new LongTypeConverter(), Type = typeof(long?) },
                new TypeConverterRegistration { Converter = new DateTimeTypeConverter(), Type = typeof(DateTimeOffset) },
                new TypeConverterRegistration { Converter = new DateTimeTypeConverter(), Type = typeof(DateTimeOffset?) },
            };
            var typeStore = new TypeStore(registrations);
            var typeManager = new ContactTypeManager(typeStore);

            var connector = new HubSpotContactConnector(mockHubSpotClient.Object, typeManager, Mock.Of<ILogger<HubSpotContactConnector>>());

            return connector;
        }

        [Test, AutoData]
        public async Task Custom_contacts_can_be_retrieved_by_id(long contactId, string firstName, string lastName, string email, DateTimeOffset createdDate, string custom)
        {
            var fromApi = new HubSpotContact
            {
                Id = contactId,
                Properties = new Dictionary<string, VersionedProperty>
                {
                    ["firstname"] = new VersionedProperty { Value = firstName },
                    ["lastname"] = new VersionedProperty { Value = lastName },
                    ["email"] = new VersionedProperty { Value = email },
                    ["createdate"] = new VersionedProperty { Value = createdDate.ToUnixTimeMilliseconds().ToString("D") },
                    ["customProperty"] = new VersionedProperty { Value = custom }
                }
            };

            mockClient.Setup(p => p.GetByIdAsync(
                          contactId,
                          It.IsAny<IReadOnlyList<IProperty>>(),
                          PropertyMode.ValueOnly,
                          FormSubmissionMode.None,
                          false)
                      )
                      .ReturnsAsync(fromApi)
                      .Verifiable();

            var sut = CreateSystemUnderTest();

            var result = await sut.GetByIdAsync<TestContact>(contactId);

            mockClient.Verify(
                p => p.GetByIdAsync(
                    contactId,
                    It.Is<IReadOnlyList<IProperty>>(list => list.Select(l => l.Name).IsSupersetOf(fromApi.Properties.Keys)),
                    PropertyMode.ValueOnly,
                    FormSubmissionMode.None,
                    false),
                Times.Once
            );

            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.FirstName, Is.EqualTo(firstName));
            Assert.That(result.LastName, Is.EqualTo(lastName));
            Assert.That(result.Created, Is.EqualTo(createdDate).Within(TimeSpan.FromMilliseconds(100)));
            Assert.That(result.CustomProperty, Is.EqualTo(custom));
        }

        [Test, AutoData]
        public async Task Basic_contacts_can_be_retrieved_by_id_without_specifying_type(long contactId, string firstName, string lastName, string email, DateTimeOffset createdDate)
        {
            var fromApi = new HubSpotContact
            {
                Id = contactId,
                Properties = new Dictionary<string, VersionedProperty>
                {
                    ["firstname"] = new VersionedProperty { Value = firstName },
                    ["lastname"] = new VersionedProperty { Value = lastName },
                    ["email"] = new VersionedProperty { Value = email },
                    ["createdate"] = new VersionedProperty { Value = createdDate.ToUnixTimeMilliseconds().ToString("D") }
                }
            };

            mockClient.Setup(p => p.GetByIdAsync(
                          contactId,
                          It.IsAny<IReadOnlyList<IProperty>>(),
                          PropertyMode.ValueOnly,
                          FormSubmissionMode.None,
                          false)
                      )
                      .ReturnsAsync(fromApi)
                      .Verifiable();

            var sut = CreateSystemUnderTest();

            var result = await sut.GetByIdAsync(contactId);

            mockClient.Verify(
                p => p.GetByIdAsync(
                    contactId,
                    It.Is<IReadOnlyList<IProperty>>(list => list.Select(l => l.Name).IsSupersetOf(fromApi.Properties.Keys)),
                    PropertyMode.ValueOnly,
                    FormSubmissionMode.None,
                    false),
                Times.Once
            );

            Assert.That(result, Is.InstanceOf<Contact>());

            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.FirstName, Is.EqualTo(firstName));
            Assert.That(result.LastName, Is.EqualTo(lastName));
            Assert.That(result.Created, Is.EqualTo(createdDate).Within(TimeSpan.FromMilliseconds(100)));
        }

        [Test, AutoData]
        public async Task Basic_contacts_can_be_retrieved_by_id(long contactId, string firstName, string lastName, string email, DateTimeOffset createdDate)
        {
            var fromApi = new HubSpotContact
            {
                Id = contactId,
                Properties = new Dictionary<string, VersionedProperty>
                {
                    ["firstname"] = new VersionedProperty { Value = firstName },
                    ["lastname"] = new VersionedProperty { Value = lastName },
                    ["email"] = new VersionedProperty { Value = email },
                    ["createdate"] = new VersionedProperty { Value = createdDate.ToUnixTimeMilliseconds().ToString("D") }
                }
            };

            mockClient.Setup(p => p.GetByIdAsync(
                          contactId,
                          It.IsAny<IReadOnlyList<IProperty>>(),
                          PropertyMode.ValueOnly,
                          FormSubmissionMode.None,
                          false)
                      )
                      .ReturnsAsync(fromApi)
                      .Verifiable();

            var sut = CreateSystemUnderTest();

            var result = await sut.GetByIdAsync<Contact>(contactId);

            mockClient.Verify(
                p => p.GetByIdAsync(
                    contactId,
                    It.Is<IReadOnlyList<IProperty>>(list => list.Select(l => l.Name).IsSupersetOf(fromApi.Properties.Keys)),
                    PropertyMode.ValueOnly,
                    FormSubmissionMode.None,
                    false),
                Times.Once
            );

            Assert.That(result, Is.InstanceOf<Contact>());

            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.FirstName, Is.EqualTo(firstName));
            Assert.That(result.LastName, Is.EqualTo(lastName));
            Assert.That(result.Created, Is.EqualTo(createdDate).Within(TimeSpan.FromMilliseconds(100)));
        }

        [Test, AutoData]
        public async Task Basic_contacts_can_be_created(Contact contact)
        {
            var toCreate = CreateFromContact(contact);

            contact.Id = 0;

            mockClient.Setup(p => p.CreateAsync(It.IsAny<IReadOnlyList<ValuedProperty>>()))
                      .ReturnsAsync(toCreate);

            var sut = CreateSystemUnderTest();

            var newContact = await sut.SaveAsync(contact);

            Assert.That(newContact.FirstName, Is.EqualTo(contact.FirstName));
            Assert.That(newContact.Id, Is.EqualTo(toCreate.Id));
        }

        [Test, AutoData]
        public async Task Basic_contacts_can_be_updated(Contact input, string newFirstName)
        {
            Assume.That(newFirstName, Is.Not.EqualTo(input.FirstName));

            var fromApi = CreateFromContact(input);

            mockClient.Setup(p => p.GetByIdAsync(
                          input.Id,
                          It.IsAny<IReadOnlyList<IProperty>>(),
                          PropertyMode.ValueOnly,
                          FormSubmissionMode.None,
                          false)
                      )
                      .ReturnsAsync(fromApi);

            var sut = CreateSystemUnderTest();

            var contact = await sut.GetByIdAsync(input.Id);

            contact.FirstName = newFirstName;

            var updatedContact = await sut.SaveAsync(contact);

            Assert.That(updatedContact.Id, Is.EqualTo(input.Id));
        }

        private static HubSpotContact CreateFromContact(Contact contact)
        {
            var hubspot = new HubSpotContact
            {
                Id = contact.Id,
                Properties = new Dictionary<string, VersionedProperty>
                {
                    ["firstname"] = new VersionedProperty { Value = contact.FirstName },
                    ["lastname"] = new VersionedProperty { Value = contact.LastName },
                    ["email"] = new VersionedProperty { Value = contact.Email },
                    ["createdate"] = new VersionedProperty { Value = contact.Created.ToUnixTimeMilliseconds().ToString("D") },
                    ["associatedcompanyid"] = new VersionedProperty() { Value = contact.AssociatedCompanyId.ToString("D") }
                }
            };

            return hubspot;
        }
    }
}