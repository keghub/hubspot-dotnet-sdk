using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using HubSpot.Internal;
using NUnit.Framework;
// ReSharper disable InvokeAsExtensionMethod

namespace Tests.Internal
{
    [TestFixture]
    public class ExtensionTests
    {
        private IFixture fixture;

        [SetUp]
        public void Initialize()
        {
            fixture = new Fixture();
        }

        [Test]
        [TestCase(null)]
        [TestCase(0)]
        [TestCase(0d)]
        [TestCase(0f)]
        public void IsDefaultValue_should_return_true_when_value_is_default(object value)
        {
            Assert.That(Extensions.IsDefaultValue(value), Is.True);
        }

        [Test]
        [TestCase(typeof(string))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(double))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(string[]))]
        [TestCase(typeof(IList<string>))]
        [TestCase(typeof(IEnumerable<string>))]
        public void IsDefaultValue_should_return_false_when_value_is_valid(Type type)
        {
            var context = new SpecimenContext(fixture);
            var value = context.Resolve(type);

            Assert.That(Extensions.IsDefaultValue(value), Is.False);
        }

        [Test]
        [TestCase(typeof(string), null)]
        [TestCase(typeof(int), 0)]
        [TestCase(typeof(long), 0)]
        [TestCase(typeof(double), 0)]
        [TestCase(typeof(decimal), 0)]
        public void DefaultValue_returns_default_value_for_type(Type type, object expected)
        {
            Assert.That(Extensions.DefaultValue(type), Is.EqualTo(expected));
        }

        [Test, ExtensionsAutoData]
        public void GetValues_retrieves_the_values(IReadOnlyDictionary<string, ILookup<string, int>> sut)
        {
            var outerKey = sut.Keys.First();
            var innerKey = sut[outerKey].First().Key;

            var values = Extensions.GetValues(sut, outerKey, innerKey);

            Assert.That(values, Is.EquivalentTo(sut[outerKey][innerKey]));
        }

        [Test, ExtensionsAutoData]
        public void GetValues_returns_empty_set_if_outerKey_is_invalid(IReadOnlyDictionary<string, ILookup<string, int>> sut, string outerKey, string innerKey)
        {
            Assume.That(sut.ContainsKey(outerKey), Is.False);

            var values = Extensions.GetValues(sut, outerKey, innerKey);

            Assert.That(values, Is.Empty);
        }

        [Test, ExtensionsAutoData]
        public void GetValues_returns_empty_set_if_innerKey_is_invalid(IReadOnlyDictionary<string, ILookup<string, int>> sut, string innerKey)
        {
            var outerKey = sut.Keys.First();

            var values = Extensions.GetValues(sut, outerKey, innerKey);

            Assert.That(values, Is.Empty);
        }

        [Test, AutoData]
        public void NotIn_should_return_first_set_if_disgiunted(int[] first, int[] second)
        {
            Assert.That(Extensions.NotIn(first, second), Is.EquivalentTo(first));
        }

        [Test, AutoData]
        public void NotIn_should_return_extra_items(int[] original, int[] extra)
        {
            var added = original.Concat(extra);

            Assert.That(Extensions.NotIn(added, original), Is.EqualTo(extra));
        }

        [Test]
        public void Batch_creates_partitions_of_given_size()
        {
            var batchSize = 15;

            var items = fixture.CreateMany<int>(batchSize + 1);

            var partitions = Extensions.Batch(items, batchSize);

            Assert.That(partitions.ElementAt(0), Has.Exactly(batchSize).Items);
            Assert.That(partitions.ElementAt(1), Has.Exactly(1).Items);
        }

        [Test]
        public void Batch_creates_partitions_of_given_size_and_applies_transformation()
        {
            var batchSize = 15;

            var items = fixture.CreateMany<char>(batchSize + 1);

            var partitions = Extensions.Batch(items, batchSize, i => new string(i.ToArray()));

            Assert.That(partitions.ElementAt(0).Length, Is.EqualTo(batchSize));
            Assert.That(partitions.ElementAt(1).Length, Is.EqualTo(1));
        }
    }

    public class ExtensionsAutoDataAttribute : AutoDataAttribute
    {
        public ExtensionsAutoDataAttribute() : base(CreateFixture)
        {
            
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();
            
            fixture.Customizations.Add(new LookupBuilder());

            return fixture;
        }
    }
}
