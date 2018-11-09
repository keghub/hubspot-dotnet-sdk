using System;
using AutoFixture.NUnit3;
using HubSpot.Model.CRM.Associations;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.CRM.Associations {
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

            Console.WriteLine($"json: {json}");

            var obj = JsonConvert.DeserializeObject<Association>(json);

            Assert.That(obj.AssociationType.Id, Is.EqualTo(testAssociation.AssociationType.Id));
        }
    }
}