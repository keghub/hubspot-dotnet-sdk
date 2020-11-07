using System;
using AutoFixture.NUnit3;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Kralizek.Extensions.Http;
using NUnit.Framework;
using HttpQueryStringBuilderExtensions = HubSpot.HttpQueryStringBuilderExtensions;

// ReSharper disable InvokeAsExtensionMethod

namespace Tests
{
    [TestFixture]
    public class HttpQueryStringBuilderExtensionsTests
    {
        [Test]
        [CustomAutoData]
        public void AddProperties_adds_all_properties(string fieldName)
        {
            var builder = new HttpQueryStringBuilder();

            var properties = new IProperty[]
            {
                ContactProperties.FirstName
            };

            HttpQueryStringBuilderExtensions.AddProperties(builder, properties, fieldName);

            Assume.That(builder.HasKey(fieldName), Is.True);

            var query = builder.BuildQuery();

            Assert.That(query.Query, Contains.Substring($"{fieldName}={ContactProperties.FirstName.Name}"));
        }

        [Test, CustomAutoData]
        public void AddProperties_builder_is_required(string fieldName)
        {
            var properties = new IProperty[]
            {
                ContactProperties.FirstName
            };

            Assert.Throws<ArgumentNullException>(() => HttpQueryStringBuilderExtensions.AddProperties(null, properties, fieldName));
        }

        [Test]
        public void AddProperties_fieldName_is_required()
        {
            var builder = new HttpQueryStringBuilder();

            var properties = new IProperty[]
            {
                ContactProperties.FirstName
            };

            Assert.Throws<ArgumentNullException>(() => HttpQueryStringBuilderExtensions.AddProperties(builder, properties, null));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddShowListMemberships_adds_value(bool testValue)
        {
            var builder = new HttpQueryStringBuilder();

            HttpQueryStringBuilderExtensions.AddShowListMemberships(builder, testValue);

            Assume.That(builder.HasKey("showListMemberships"));

            var query = builder.BuildQuery();

            Assert.That(query.Query, Contains.Substring($"showListMemberships={testValue.ToString().ToLower()}"));
        }

        [Test, CustomAutoData]
        public void AddShowListMemberships_builder_is_required(bool testValue)
        {
            Assert.Throws<ArgumentNullException>(() => HttpQueryStringBuilderExtensions.AddShowListMemberships(null, testValue));
        }

        [Test]
        [TestCase(FormSubmissionMode.All, "all")]
        [TestCase(FormSubmissionMode.Newest, "newest")]
        [TestCase(FormSubmissionMode.None, "none")]
        [TestCase(FormSubmissionMode.Oldest, "oldest")]
        public void AddFormSubmissionMode_adds_value(FormSubmissionMode mode, string value)
        {
            var builder = new HttpQueryStringBuilder();

            HttpQueryStringBuilderExtensions.AddFormSubmissionMode(builder, mode);

            Assume.That(builder.HasKey("formSubmissionMode"));

            var query = builder.BuildQuery();

            Assert.That(query.Query, Contains.Substring($"formSubmissionMode={value}"));
        }

        [Test, CustomAutoData]
        public void AddFormSubmissionMode_builder_is_required(FormSubmissionMode testValue)
        {
            Assert.Throws<ArgumentNullException>(() => HttpQueryStringBuilderExtensions.AddFormSubmissionMode(null, testValue));
        }

        [Test]
        public void AddFormSubmissionMode_formSubmissionMode_must_be_a_valid_mode()
        {
            var builder = new HttpQueryStringBuilder();

            Assert.Throws<ArgumentOutOfRangeException>(() => HttpQueryStringBuilderExtensions.AddFormSubmissionMode(builder, (FormSubmissionMode)100));
        }

        [Test]
        [TestCase(PropertyMode.ValueAndHistory, "value_and_history")]
        [TestCase(PropertyMode.ValueOnly, "value_only")]
        public void AddPropertyMode_adds_value(PropertyMode mode, string value)
        {
            var builder = new HttpQueryStringBuilder();

            HttpQueryStringBuilderExtensions.AddPropertyMode(builder, mode);

            Assume.That(builder.HasKey("propertyMode"));

            var query = builder.BuildQuery();

            Assert.That(query.Query, Contains.Substring($"propertyMode={value}"));
        }

        [Test, CustomAutoData]
        public void AddPropertyMode_builder_is_required(PropertyMode testValue)
        {
            Assert.Throws<ArgumentNullException>(() => HttpQueryStringBuilderExtensions.AddPropertyMode(null, testValue));
        }
    }
}