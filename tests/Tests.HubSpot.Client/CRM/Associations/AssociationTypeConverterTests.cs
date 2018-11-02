using System;
using System.IO;
using AutoFixture.NUnit3;
using HubSpot.Model.CRM.Associations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tests.CRM.Associations
{
    [TestFixture]
    public class AssociationSerializationTests
    {
        [Test]
        [AutoData]
        public void Association_is_correctly_serialized(Association testAssociation)
        {
            var json = JsonConvert.SerializeObject(testAssociation);

            Assert.That(json, Contains.Substring($"\"definitionId\":{testAssociation.AssociationType.Id}"));
        }

        [Test]
        [AutoData]
        public void Association_is_correctly_deserialized(Association testAssociation)
        {
            var json = $@"{{""fromObjectId"":{testAssociation.FromId},""toObjectId"":{testAssociation.ToId},""definitionId"":{testAssociation.AssociationType.Id},""category"":""HUBSPOT_DEFINED""}}";

            var obj = JsonConvert.DeserializeObject<Association>(json);

            Assert.That(obj.AssociationType.Id, Is.EqualTo(testAssociation.AssociationType.Id));
        }
    }

    [TestFixture]
    public class AssociationTypeConverterTests
    {
        [Test, AutoData]
        public void CanConvert_returns_true_if_AssociationType(AssociationTypeConverter sut)
        {
            Assert.That(sut.CanConvert(typeof(AssociationType)), Is.True);
        }

        [Test, AutoData]
        public void CanConvert_returns_false_if_not_AssociationType(AssociationTypeConverter sut, Type type)
        {
            Assume.That(type, Is.Not.EqualTo(typeof(AssociationType)));

            Assert.That(sut.CanConvert(type), Is.False);
        }

        [Test, AutoData]
        public void ReadJson_can_deserialize_an_integer_as_AssociationType(AssociationTypeConverter sut, int testValue)
        {
            JsonReader reader = new JTokenReader(JToken.Parse($"{testValue}"));

            var result = sut.ReadJson(reader, typeof(AssociationType), null, JsonSerializer.CreateDefault()) as AssociationType;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf<AssociationType>());
                Assert.That(result.Id,Is.EqualTo(testValue));
            });
        }

        [Test, AutoData]
        public void ReadJson_ignores_if_wrong_type_is_requested(AssociationTypeConverter sut, int testValue, Type type)
        {
            JsonReader reader = new JTokenReader(JToken.Parse($"{testValue}"));

            var result = sut.ReadJson(reader, type, null, JsonSerializer.CreateDefault()) as AssociationType;

            Assert.That(result, Is.Null);
        }

        [Test, AutoData]
        public void WriteJson_writes_item_as_integer(AssociationTypeConverter sut, AssociationType testValue)
        {
            var stringWriter = new StringWriter();
            var writer = new JsonTextWriter(stringWriter);

            sut.WriteJson(writer, testValue, JsonSerializer.CreateDefault());

            var json = stringWriter.ToString();

            Assert.That(json, Is.EqualTo($"{testValue.Id}"));
        }

        [Test]
        [InlineAutoData(123)]
        [InlineAutoData("hello-world")]
        public void WriteJson_ignores_item_if_not_AssociationType(object testValue, AssociationTypeConverter sut)
        {
            var stringWriter = new StringWriter();
            var writer = new JsonTextWriter(stringWriter);

            sut.WriteJson(writer, testValue, JsonSerializer.CreateDefault());

            var json = stringWriter.ToString();

            Assert.That(json, Is.Empty);

        }
    }
}