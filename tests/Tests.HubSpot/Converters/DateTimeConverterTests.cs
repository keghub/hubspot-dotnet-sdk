using HubSpot.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Converters
{
    [TestFixture]
    public class DateTimeConverterTests
    {
        [Test, CustomAutoData]
        public void TryConvertTo_returns_false_if_value_is_null(DateTimeConverter sut)
        {
            //Arrange

            //Act
            var result = sut.TryConvertTo(null, out _);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test, CustomAutoData]
        public void TryConvertTo_returns_false_if_value_is_invalid(DateTimeConverter sut, string invalidString)
        {
            //Arrange

            //Act
            var result = sut.TryConvertTo(invalidString, out _);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test, CustomAutoData]
        public void TryConvertTo_returns_true_if_value_is_datetime(DateTimeConverter sut, DateTime dateTime)
        {
            //Arrange

            //Act
            var result = sut.TryConvertTo(dateTime.ToString(), out _);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_is_default_if_value_is_null(DateTimeConverter sut)
        {
            //Arrange

            //Act
            sut.TryConvertTo(null, out var result);

            //Assert
            Assert.That(result, Is.EqualTo(default));
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_is_default_if_value_is_invalid(DateTimeConverter sut, string invalidString)
        {
            //Arrange

            //Act
            sut.TryConvertTo(invalidString, out var result);

            //Assert
            Assert.That(result, Is.EqualTo(default));
        }

        [Test, CustomAutoData]
        public void TryConvertTo_result_is_datetime_if_value_is_valid(DateTimeConverter sut, DateTime dateTime)
        {
            //Arrange

            //Act
            sut.TryConvertTo(dateTime.ToString(), out var result);

            //Assert
            Assert.That(result.ToString(), Is.EqualTo(dateTime.ToString()));
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_returns_false_if_value_is_null(DateTimeConverter sut)
        {
            //Arrange

            //Act
            var result = sut.TryConvertFrom(null, out string _);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_returns_false_if_value_is_invalid(DateTimeConverter sut, string invalidValue)
        {
            //Arrange

            //Act
            var result = sut.TryConvertFrom(invalidValue, out string _);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_returns_true_if_value_is_valid(DateTimeConverter sut, DateTime validValue)
        {
            //Arrange

            //Act
            var result = sut.TryConvertFrom(validValue, out string _);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_null_if_value_is_null(DateTimeConverter sut)
        {
            //Arrange

            //Act
            sut.TryConvertFrom(null, out string result);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_null_if_value_is_invalid(DateTimeConverter sut, string invalidValue)
        {
            //Arrange

            //Act
            sut.TryConvertFrom(invalidValue, out string result);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_result_is_equal_with_value_if_value_is_datetime(DateTimeConverter sut, DateTime dateTime)
        {
            //Arrange

            //Act
            sut.TryConvertFrom(dateTime, out string result);

            //Assert
            Assert.That(result, Is.EqualTo(dateTime.ToString()));
        }

        [Test, CustomAutoData]
        public void TryConvertFrom_valid_result_is_standard_format(DateTimeConverter sut, DateTime dateTime)
        {
            //Arrange 
            var expected = dateTime.ToString("G");

            //Act
            sut.TryConvertFrom(dateTime, out string result);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
