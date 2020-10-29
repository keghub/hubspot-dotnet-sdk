using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using HubSpot.Converters;
using NUnit.Framework;

namespace Tests.Converters
{
    [TestFixture]
    public class StringArrayConverterTests
    {
        [Test, CustomAutoData]
        public void TryConvertTo_returns_true_if_value_is_null(StringArrayConverter sut)
        {
            var canConvert = sut.TryConvertTo(null, out object result);

            Assert.IsTrue(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_is_default_if_value_is_null(StringArrayConverter sut)
        {
            var canConvert = sut.TryConvertTo(null, out object result);

            Assert.That(result, Is.EqualTo(default));
        }

        [Test, CustomAutoData]
        public void TryConvertTo_returns_true_if_value_is_empty_string(StringArrayConverter sut)
        {
            var canConvert = sut.TryConvertTo(string.Empty, out object result);

            Assert.IsTrue(canConvert);
        }


        [Test, CustomAutoData]
        public void TryConvertTo_result_is_default_if_value_is_empty_string(StringArrayConverter sut)
        {
            var canConvert = sut.TryConvertTo(string.Empty, out object result);

            Assert.That(result, Is.EqualTo(default));
        }

        [Test, CustomAutoData]
        public void TryConvertTo_returns_true_if_value_is_whitespace(StringArrayConverter sut)
        {
            var canConvert = sut.TryConvertTo("   ", out object result);

            Assert.IsTrue(canConvert);
        }
        
        [Test, CustomAutoData]
        public void TryConvertTo_result_is_default_if_value_is_whitespace(StringArrayConverter sut)
        {
            var canConvert = sut.TryConvertTo("  ", out object result);

            Assert.That(result, Is.EqualTo(default));
        }

        [Test, CustomAutoData]
        public void TryConvertTo_returns_true_if_value_is_semicolon_separated_string(StringArrayConverter sut, string[] values)
        {
            var value = string.Join(";", values);

            var canConvert = sut.TryConvertTo(value, out object result);

            Assert.IsTrue(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_array_if_value_is_semicolon_separated_string(StringArrayConverter sut, string[] values)
        {
            var value = string.Join(";", values);

            var canConvert = sut.TryConvertTo(value, out object result);

            Assert.That(result, Is.EquivalentTo(values));
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_array_is_of_type_string_array(StringArrayConverter sut, string[] values)
        {
            var value = string.Join(";", values);

            var canConvert = sut.TryConvertTo(value, out object result);

            Assert.That(result, Is.TypeOf<string[]>());
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_array_without_whitespace_items(StringArrayConverter sut, string[] values)
        {
            var valuesWithWhitespace = values.Concat(new[] { " ", "  ", "   " });

            var value = string.Join(";", valuesWithWhitespace);

            var canConvert = sut.TryConvertTo(value, out object result);

            Assert.That(result, Is.EquivalentTo(values));
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_array_with_trimmed_items(StringArrayConverter sut, string[] values)
        {
            var valuesWithWhitespace = values.Select(s => $"  {s}  ").ToArray();

            var value = string.Join(";", valuesWithWhitespace);

            var canConvert = sut.TryConvertTo(value, out object result);

            Assert.That(result, Is.EquivalentTo(values));
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_array_without_empty_items(StringArrayConverter sut, string[] values)
        {
            var valuesWithWhitespace = values.Concat(new[] { string.Empty, "" });

            var value = string.Join(";", valuesWithWhitespace);

            var canConvert = sut.TryConvertTo(value, out object result);

            Assert.That(result, Is.EquivalentTo(values));
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_array_without_null_items(StringArrayConverter sut, string[] values)
        {
            var valuesWithWhitespace = values.Concat(new string[] { null, null });

            var value = string.Join(";", valuesWithWhitespace);

            var canConvert = sut.TryConvertTo(value, out object result);

            Assert.That(result, Is.EquivalentTo(values));
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_returns_true_if_value_is_null(StringArrayConverter sut)
        {
            var canConvert = sut.TryConvertFrom(null, out string result);

            Assert.IsTrue(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_null_if_value_is_null(StringArrayConverter sut)
        {
            var canConvert = sut.TryConvertFrom(null, out string result);

            Assert.IsNull(result);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_returns_false_if_value_is_string_list(StringArrayConverter sut, List<string> value)
        {
            var canConvert = sut.TryConvertFrom(value, out string result);

            Assert.IsFalse(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_null_if_value_is_string_list(StringArrayConverter sut, List<string> value)
        {
            var canConvert = sut.TryConvertFrom(value, out string result);

            Assert.IsNull(result);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_returns_false_if_value_is_string(StringArrayConverter sut, string value)
        {
            var canConvert = sut.TryConvertFrom(value, out string result);

            Assert.IsFalse(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_null_if_value_is_string(StringArrayConverter sut, string value)
        {
            var canConvert = sut.TryConvertFrom(value, out string result);

            Assert.IsNull(result);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_returns_false_if_value_is_object(StringArrayConverter sut, object value)
        {
            var canConvert = sut.TryConvertFrom(value, out string result);

            Assert.IsFalse(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_null_if_value_is_object(StringArrayConverter sut, object value)
        {
            var canConvert = sut.TryConvertFrom(value, out string result);

            Assert.IsNull(result);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_returns_true_if_value_is_string_array(StringArrayConverter sut, string[] value)
        {
            var canConvert = sut.TryConvertFrom(value, out string result);

            Assert.IsTrue(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_semicolon_separated_string_if_value_is_string_array(StringArrayConverter sut, string[] value)
        {
            var canConvert = sut.TryConvertFrom(value, out string result);

            Assert.AreEqual(result, string.Join(";", value));
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_semicolon_separated_string_without_empty_values(StringArrayConverter sut, string[] value)
        {
            var valuesWithWhitespace = value.Concat(new[] { string.Empty, "" }).ToArray();

            var canConvert = sut.TryConvertFrom(valuesWithWhitespace, out string result);

            Assert.AreEqual(result, string.Join(";", value));
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_semicolon_separated_string_without_whitespace_values(StringArrayConverter sut, string[] value)
        {
            var valuesWithWhitespace = value.Concat(new[] { "  ", " " }).ToArray();

            var canConvert = sut.TryConvertFrom(valuesWithWhitespace, out string result);

            Assert.AreEqual(result, string.Join(";", value));
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_semicolon_separated_string_without_null_values(StringArrayConverter sut, string[] value)
        {
            var valuesWithWhitespace = value.Concat(new string[] { null, null }).ToArray();

            var canConvert = sut.TryConvertFrom(valuesWithWhitespace, out string result);

            Assert.AreEqual(result, string.Join(";", value));
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_semicolon_separated_string_with_trimmed_values(StringArrayConverter sut, string[] value)
        {
            var valuesWithWhitespace = value.Select(s => $"  {s}  ").ToArray();

            var canConvert = sut.TryConvertFrom(valuesWithWhitespace, out string result);

            Assert.AreEqual(result, string.Join(";", value));
        }
    }
}