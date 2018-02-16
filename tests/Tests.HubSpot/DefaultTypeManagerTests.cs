using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot;
using Moq;
using NUnit.Framework;
using HubSpotContact = HubSpot.Model.Contacts.Contact;

namespace Tests
{
    //[TestFixture]
    //public class DefaultTypeManagerTests
    //{
    //    private Mock<ITypeStore> mockTypeStore;

    //    [SetUp]
    //    public void Initialize()
    //    {
    //        mockTypeStore = new Mock<ITypeStore>();

    //        mockTypeStore.Setup(p => p.GetConstructor<HubSpotContact, TestContact>()).Returns(typeof(TestContact).GetConstructors().First());
    //        mockTypeStore.Setup(p => p.GetCustomProperties<HubSpotContact, TestContact>()).Returns(Array.Empty<PropertyInfo>());
    //    }

    //    public class TestContact : Contact
    //    {
    //        [CustomProperty("customProperty")]
    //        public string CustomProperty { get; set; }

    //        public TestContact(HubSpotContact entity) : base(entity) { }
    //    }

    //    [Test, AutoData]
    //    public async Task Test(HubSpotContact hubSpot)
    //    {
    //        var sut = new TypeManager<HubSpotContact, TestContact>(mockTypeStore.Object);

    //        var result = await sut.ConvertFrom<TestContact>(hubSpot);

    //        Assert.That(result.InnerEntity, Is.SameAs(hubSpot));
    //    }
    //}
}