using System.Globalization;
using HubSpot.Converters;
using NUnit.Framework;

namespace Tests.Converters
{
    [TestFixture]
    public class DecimalTypeConverterTests
    {
        [Test, CustomAutoData]
        public void TryConvertTo_returns_true_if_value_is_null(DecimalTypeConverter sut)
        {
            var canConvert = sut.TryConvertTo(null, out _);

            Assert.IsTrue(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_is_default_if_value_is_null(DecimalTypeConverter sut)
        {
            _ = sut.TryConvertTo(null, out object result);

            Assert.That(result, Is.EqualTo(default));
        }

        
        [Test, CustomAutoData]
        public void TryConvertFrom_returns_true_if_value_is_null(DecimalTypeConverter sut)
        {
            var canConvert = sut.TryConvertFrom(null, out _);

            Assert.IsTrue(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_null_if_value_is_null(DecimalTypeConverter sut)
        {
            
            _ = sut.TryConvertFrom(null, out string result);

            Assert.IsNull(result);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_returns_false_if_value_is_not_decimal(DecimalTypeConverter sut, string value)
        {
            var canConvert = sut.TryConvertFrom(value, out _);

            Assert.IsFalse(canConvert);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_dotted_decimal_string(DecimalTypeConverter sut, decimal dec)
        {
            _ = sut.TryConvertFrom(dec, out string result);

            Assert.AreEqual(result, dec.ToString("G", CultureInfo.InvariantCulture));
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_not_containing_comma(DecimalTypeConverter sut, decimal dec)
        {
            _ = sut.TryConvertFrom(dec, out string result);

            Assert.IsFalse(result.Contains(","));
        }
    }
}