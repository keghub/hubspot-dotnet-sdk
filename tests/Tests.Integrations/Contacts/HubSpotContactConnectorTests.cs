using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
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
    public class HubSpotContactConnectorTests
    {
        private Mock<IHubSpotClient> mockHubSpotClient;
        private Mock<IHubSpotContactClient> mockClient;

        [SetUp]
        public void Initialize()
        {
            mockClient = new Mock<IHubSpotContactClient>(MockBehavior.Strict);

            mockHubSpotClient = new Mock<IHubSpotClient>(MockBehavior.Strict);
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
                new TypeConverterRegistration { Converter = new IntTypeConverter(), Type = typeof(int) },
                new TypeConverterRegistration { Converter = new IntTypeConverter(), Type = typeof(int?) },
                new TypeConverterRegistration { Converter = new DecimalTypeConverter(), Type = typeof(decimal) },
                new TypeConverterRegistration { Converter = new DecimalTypeConverter(), Type = typeof(decimal?) },
                new TypeConverterRegistration { Converter = new StringListConverter(), Type = typeof(List<string>) },
                new TypeConverterRegistration { Converter = new StringListConverter(), Type = typeof(IList<string>) },
                new TypeConverterRegistration { Converter = new StringListConverter(), Type = typeof(IEnumerable<string>) },
                new TypeConverterRegistration { Converter = new StringListConverter(), Type = typeof(IReadOnlyList<string>) },
                new TypeConverterRegistration { Converter = new StringArrayConverter(), Type = typeof(string[]) },
            };
            var typeStore = new TypeStore(registrations);
            var typeManager = new ContactTypeManager(typeStore);

            var connector = new HubSpotContactConnector(mockHubSpotClient.Object, typeManager);

            return connector;
        }

        [Test, ContactAutoData]
        public async Task Custom_contacts_can_be_retrieved_by_id(long contactId, string firstName, string lastName, string email, DateTimeOffset createdDate, string custom,
            List<string> stringList, string[] stringArray, IList<string> stringIList, IEnumerable<string> stringIEnumerable, IReadOnlyList<string> stringIReadOnlyList)
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
                    ["customProperty"] = new VersionedProperty { Value = custom },
                    ["stringListProperty"] = new VersionedProperty { Value = string.Join(";", stringList) },
                    ["stringArrayProperty"] = new VersionedProperty { Value = string.Join(";", stringArray) },
                    ["stringIListProperty"] = new VersionedProperty { Value = string.Join(";", stringIList) },
                    ["stringIEnumerableProperty"] = new VersionedProperty { Value = string.Join(";", stringIEnumerable) },
                    ["stringIReadOnlyListProperty"] = new VersionedProperty { Value = string.Join(";", stringIReadOnlyList) }
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
            Assert.That(result.StringListProperty, Is.EqualTo(stringList));
            Assert.That(result.StringArrayProperty, Is.EqualTo(stringArray));
            Assert.That(result.StringIListProperty, Is.EqualTo(stringIList));
            Assert.That(result.StringIEnumerableProperty, Is.EqualTo(stringIEnumerable));
            Assert.That(result.StringIReadOnlyListProperty, Is.EqualTo(stringIReadOnlyList));
        }

        [Test, ContactAutoData]
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

        [Test, ContactAutoData]
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

        [Test, ContactAutoData]
        public async Task Basic_contacts_can_be_created(TestContact contact)
        {
            var toCreate = CreateFromContact(contact);

            contact.Id = 0;
            contact.Created = default;

            mockClient.Setup(p => p.CreateAsync(It.IsAny<IReadOnlyList<ValuedProperty>>()))
                      .ReturnsAsync(toCreate);

            var sut = CreateSystemUnderTest();

            var newContact = await sut.SaveAsync(contact);

            Assert.That(newContact.FirstName, Is.EqualTo(contact.FirstName));
            Assert.That(newContact.Id, Is.EqualTo(toCreate.Id));
        }

        [Test, ContactAutoData]
        public async Task Basic_contacts_can_be_updated(TestContact input, string newFirstName)
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

            mockClient.Setup(p => p.UpdateByIdAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<ValuedProperty>>())).CompletesAsync();

            var sut = CreateSystemUnderTest();

            var contact = await sut.GetByIdAsync(input.Id);

            contact.FirstName = newFirstName;

            var updatedContact = await sut.SaveAsync(contact);

            Assert.That(updatedContact.Id, Is.EqualTo(input.Id));
        }

        [Test]
        public async Task Empty_contact_should_not_be_persisted()
        {
            var contact = new TestContact();

            var sut = CreateSystemUnderTest();

            var newContact = await sut.SaveAsync(contact);

            mockClient.Verify(p => p.CreateAsync(It.IsAny<IReadOnlyList<ValuedProperty>>()), Times.Never);

            mockClient.Verify(p => p.UpdateByIdAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<ValuedProperty>>()), Times.Never);

            Assert.That(newContact, Is.SameAs(contact));
        }

        [Test, ContactAutoData]
        public async Task Reference_property_reset_to_default_value_should_be_persisted(TestContact contact)
        {
            SetPropertyDictionary(contact);

            contact.CustomProperty = null;

            mockClient.Setup(p => p.UpdateByIdAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<ValuedProperty>>())).CompletesAsync();

            mockClient.Setup(p => p.GetByIdAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>())).ReturnsAsync(CreateFromContact(contact));

            var sut = CreateSystemUnderTest();

            var newContact = await sut.SaveAsync(contact);

            mockClient.Verify(p => p.UpdateByIdAsync(contact.Id, It.IsAny<IReadOnlyList<ValuedProperty>>()), Times.Once);

            Assert.That(newContact.CustomProperty, Is.Null);
        }

        [Test, ContactAutoData]
        public async Task Value_property_reset_to_default_value_should_be_persisted(TestContact contact)
        {
            SetPropertyDictionary(contact);

            contact.AssociatedCompanyId = 0;

            mockClient.Setup(p => p.UpdateByIdAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<ValuedProperty>>())).CompletesAsync();

            mockClient.Setup(p => p.GetByIdAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<IProperty>>(), It.IsAny<PropertyMode>(), It.IsAny<FormSubmissionMode>(), It.IsAny<bool>())).ReturnsAsync(CreateFromContact(contact));

            var sut = CreateSystemUnderTest();

            var newContact = await sut.SaveAsync(contact);

            mockClient.Verify(p => p.UpdateByIdAsync(contact.Id, It.IsAny<IReadOnlyList<ValuedProperty>>()), Times.Once);

            Assert.That(newContact.AssociatedCompanyId, Is.EqualTo(0));
        }

        private static HubSpotContact CreateFromContact(TestContact contact)
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
                    ["associatedcompanyid"] = new VersionedProperty() { Value = contact.AssociatedCompanyId.ToString("D") },
                    ["customProperty"] = new VersionedProperty() { Value = contact.CustomProperty },
                    ["stringListProperty"] = new VersionedProperty() { Value = string.Join(";", contact.StringListProperty) },
                    ["stringArrayProperty"] = new VersionedProperty() { Value = string.Join(";", contact.StringArrayProperty) },
                    ["stringIListProperty"] = new VersionedProperty() { Value = string.Join(";", contact.StringIListProperty) },
                    ["stringIEnumerableProperty"] = new VersionedProperty() { Value = string.Join(";", contact.StringIEnumerableProperty) },
                    ["stringIReadOnlyListProperty"] = new VersionedProperty() { Value = string.Join(";", contact.StringIReadOnlyListProperty) }
                }
            };

            return hubspot;
        }

        private static void SetPropertyDictionary(TestContact testContact)
        {
            ((IHubSpotEntity)testContact).Properties = new Dictionary<string, object>
            {
                ["firstname"] = testContact.FirstName,
                ["lastname"] = testContact.LastName,
                ["email"] = testContact.Email,
                ["createdate"] = testContact.Created,
                ["associatedcompanyid"] = testContact.AssociatedCompanyId,
                ["customProperty"] = testContact.CustomProperty,
                ["stringListProperty"] = testContact.StringListProperty,
                ["stringArrayProperty"] = testContact.StringArrayProperty,
                ["stringIListProperty"] = testContact.StringIListProperty,
                ["stringIEnumerableProperty"] = testContact.StringIEnumerableProperty,
                ["stringIReadOnlyListProperty"] = testContact.StringIReadOnlyListProperty
            };
        }
    }

    public class ContactAutoDataAttribute : AutoDataAttribute
    {
        public ContactAutoDataAttribute() : base(CreateFixture)
        {

        }

        public static IFixture CreateFixture()
        {
            IFixture fixture = new Fixture();

            fixture.Customize<TestContact>(hp =>
                hp.With(i => i.StringIEnumerableProperty, fixture.CreateMany<string>().ToArray().AsEnumerable()).With(
                    i => i.StringIReadOnlyListProperty,
                    (IReadOnlyList<string>) fixture.CreateMany<string>().ToArray()));

            return fixture;
        }
    }
}
