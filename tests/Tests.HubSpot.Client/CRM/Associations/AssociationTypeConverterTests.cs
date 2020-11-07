using System;
using System.IO;
using System.Linq;
using AutoFixture.NUnit3;
using HubSpot.Model.CRM.Associations;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.CRM.Associations
{
    [TestFixture]
    public class AssociationTypeConverterTests
    {
        [Test, CustomAutoData]
        public void CanConvert_returns_true_if_AssociationType(AssociationTypeConverter sut)
        {
            Assert.That(sut.CanConvert(typeof(AssociationType)), Is.True);
        }

        [Test, CustomAutoData]
        public void CanConvert_returns_false_if_not_AssociationType(AssociationTypeConverter sut, Type type)
        {
            Assume.That(type, Is.Not.EqualTo(typeof(AssociationType)));

            Assert.That(sut.CanConvert(type), Is.False);
        }

        [Test, CustomAutoData]
        public void ReadJson_can_deserialize_an_integer_as_AssociationType(AssociationTypeConverter sut, int testValue)
        {
            var json = $"{testValue}";

            JsonReader reader = new JsonTextReader(new StringReader(json));

            var obj = sut.ReadJson(reader, typeof(AssociationType), null, JsonSerializer.CreateDefault());

            var result = obj as AssociationType;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<AssociationType>());
            Assert.That(result.Id, Is.EqualTo(testValue));
        }

        [Test, CustomAutoData]
        public void ReadJson_can_deserialize_an_array_of_integers_as_AssociationType(AssociationTypeConverter sut, int[] values)
        {
            var json = JsonConvert.SerializeObject(values);

            Console.WriteLine(json);

            JsonReader reader = new JsonTextReader(new StringReader(json));

            var result = JsonConvert.DeserializeObject(json, typeof(AssociationType[]), sut);

            Assert.That(result, Has.Some.Matches<AssociationType>(at => values.Contains(at.Id)));
        }

        [Test, CustomAutoData]
        public void WriteJson_writes_item_as_integer(AssociationTypeConverter sut, AssociationType testValue)
        {
            var stringWriter = new StringWriter();
            var writer = new JsonTextWriter(stringWriter);

            sut.WriteJson(writer, testValue, JsonSerializer.CreateDefault());

            var json = stringWriter.ToString();

            Assert.That(json, Is.EqualTo($"{testValue.Id}"));
        }
    }
}