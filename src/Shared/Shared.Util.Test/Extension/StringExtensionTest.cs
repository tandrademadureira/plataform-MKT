using NUnit.Framework;
using Shared.Util.Extension;
using System;
using System.Globalization;

namespace Shared.Util.Test.Extension
{
    [TestFixture]
    public class StringExtensionTest
    {
        [TestCase("10/10/2012", ExpectedResult = "10/10/2012")]
        [TestCase("10/09/2012", ExpectedResult = "10/09/2012")]
        [TestCase("09/02/2011", ExpectedResult = "09/02/2011")]
        public string TryParseDateTime(string value)
        {
            //Arrange

            //Act
            var dateTime = value.TryParseDateTimeOrNullable();

            //Assert
            return dateTime.Value.ToString("MM/dd/yyyy", new CultureInfo("en-US")); ;
        }

        [TestCase("101010", ExpectedResult = null)]
        [TestCase("SmarketsApp", ExpectedResult = null)]
        public DateTime? TryParseDateTimeNullable(string value)
        {
            //Arrange

            //Act
            var dateTime = value.TryParseDateTimeOrNullable();

            //Assert
            return dateTime;
        }

        [TestCase("aUJlYWNoQXBw", ExpectedResult = new byte[] { 34, 97, 85, 74, 108, 89, 87, 78, 111, 81, 88, 66, 119, 34 })]
        [TestCase("aUJlYWNoIEFwcA==", ExpectedResult = new byte[] { 34, 97, 85, 74, 108, 89, 87, 78, 111, 73, 69, 70, 119, 99, 65, 61, 61, 34 })]
        public byte[] TryParseBase64(string value)
        {
            //Arrange

            //Act
            var byteArray = value.TryParseBase64OrNullable();

            //Assert
            return byteArray;
        }

        [TestCase("aUJlYWNoQX", ExpectedResult = null)]
        [TestCase("aUJlYWNoIEFwcA", ExpectedResult = null)]
        [TestCase("SmarketsApp", ExpectedResult = null)]
        [TestCase("Smarkets App", ExpectedResult = null)]
        public byte[] TryParseBase64Nullable(string value)
        {
            //Arrange

            //Act
            var byteArray = value.TryParseBase64OrNullable();

            //Assert
            return byteArray;
        }

        [TestCase("Bar", ExpectedResult = FooEnum.Bar)]
        [TestCase("Sample", ExpectedResult = FooEnum.Sample)]
        public FooEnum? TryParseEnum(string value)
        {
            //Arrange

            //Act
            var enumValue = value.TryParseEnumOrNullable<FooEnum>();

            //Assert
            return enumValue;
        }

        [TestCase("SmarketsApp", ExpectedResult = null)]
        [TestCase("Smarkets App", ExpectedResult = null)]
        [TestCase("Bar Sample", ExpectedResult = null)]
        public FooEnum? TryParseEnumNullable(string value)
        {
            //Arrange

            //Act
            var enumValue = value.TryParseEnumOrNullable<FooEnum>();

            //Assert
            return enumValue;
        }

        [TestCase("15", ExpectedResult = 15)]
        [TestCase("10", ExpectedResult = 10)]
        [TestCase("1289", ExpectedResult = 1289)]
        public int? TryParseInt(string value)
        {
            //Arrange

            //Act
            var number = value.TryParseIntOrNullable();

            //Assert
            return number;
        }

        [TestCase("SmarketsApp", ExpectedResult = null)]
        [TestCase("Smarkets App", ExpectedResult = null)]
        [TestCase("Smarkets 10", ExpectedResult = null)]
        [TestCase("1 2 3 4", ExpectedResult = null)]
        public int? TryParseIntNullable(string value)
        {
            //Arrange

            //Act
            var number = value.TryParseIntOrNullable();

            //Assert
            return number;
        }

        [TestCase("15.2", ExpectedResult = 15.2)]
        [TestCase("10.045", ExpectedResult = 10.045)]
        [TestCase("1289.88", ExpectedResult = 1289.88)]
        [TestCase("1289", ExpectedResult = 1289)]
        public decimal? TryParseDecimal(string value)
        {
            //Arrange

            //Act
            var number = value.TryParseDecimalOrNullable();

            //Assert
            return number;
        }

        [TestCase("SmarketsApp", ExpectedResult = null)]
        [TestCase("Smarkets App", ExpectedResult = null)]
        [TestCase("Smarkets 10", ExpectedResult = null)]
        [TestCase("1,5 2,8 3,0 4,2", ExpectedResult = null)]
        public decimal? TryParseDecimalNullable(string value)
        {
            //Arrange

            //Act
            var number = value.TryParseDecimalOrNullable();

            //Assert
            return number;
        }

        [TestCase("Smark-C0", ExpectedResult = "SmarkC0")]
        [TestCase("SmarkÁpp", ExpectedResult = "SmarkApp")]
        [TestCase("Framework Smarkets App 2.0", ExpectedResult = "FrameworkSmarketsApp20")]
        public string GetWithoutSpecialCharacter(string value)
        {
            //Arrange

            //Act
            var valueWithoutSpecialCharacters = value.GetWithoutSpecialCharacter();

            //Assert
            return valueWithoutSpecialCharacters;
        }

        [TestCase("iBea     App", ExpectedResult = "iBea App")]
        [TestCase("A    sentence        with spaces.", ExpectedResult = "A sentence with spaces.")]
        public string GetStandardSpaces(string value)
        {
            //Arrange

            //Act
            var formatedValue = value.GetStandardSpaces();

            //Assert
            return formatedValue;
        }

        const string AppSettings = @"
         {
          ""AppSettings"": {
            ""HostingSettings"": {
              ""HostAddress"": {
                ""Addresses"": {
                  ""ApiAddress"": ""#{api_address}#""
                }
              }
            }
          }
        }
         ";
        [TestCase(AppSettings, "#{", "}#", ExpectedResult = "api_address")]
        [TestCase(AppSettings, "app", "\":", ExpectedResult = "Settings")]
        [TestCase(AppSettings, "app", "\":", true, ExpectedResult = null)]
        [TestCase(AppSettings, "ApiAddress", "!", true, false, ExpectedResult = null)]
        public string GetExtractedValue(string value, string beginDelim, string endDelim, bool caseSensitive = false, bool allowMissingEndDelimiter = false)
        {
            return value.Extract(beginDelim, endDelim, caseSensitive, allowMissingEndDelimiter);
        }

        public enum FooEnum
        {
            [System.ComponentModel.Description("Bar")]
            Bar = 1,
            Sample = 2
        }
    }
}
