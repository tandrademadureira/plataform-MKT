using NUnit.Framework;
using Shared.Util.Common.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shared.Util.Test.Common.Attributes
{

    [TestFixture]
    public class Base64AttributeTest
    {
        [TestCase("aUJlYWNoQXBw", ExpectedResult = true)]
        [TestCase("aUJlYWNoIEFwcA==", ExpectedResult = true)]
        [TestCase("aUJlYWNoQXB", ExpectedResult = false)]
        [TestCase("Smarkets com br", ExpectedResult = false)]
        [TestCase(null, ExpectedResult = true)]
        public bool Base64AttributeValidate(string value)
        {
            //Arrange
            var validationResults = new List<ValidationResult>();
            var base64 = new Base64Message { Base64 = value };
            var validationContext = new ValidationContext(base64);

            //Act
            Validator.TryValidateObject(base64, validationContext, validationResults, true);

            //Assert
            return validationResults.Count == 0;
        }

        [TestCase("aUJlYWNoQXBw", ExpectedResult = null)]
        [TestCase("aUJlYWNoIEFwcA==", ExpectedResult = null)]
        [TestCase("aUJlYWNoQXB", ExpectedResult = "Value is not Base64 valid.")]
        [TestCase("Smarkets com br ", ExpectedResult = "Value is not Base64 valid.")]
        [TestCase(null, ExpectedResult = null)]
        public string Base64AttributeMessage(string value)
        {
            //Arrange
            var validationResults = new List<ValidationResult>();
            var base64 = new Base64Message { Base64 = value };
            var validationContext = new ValidationContext(base64);

            //Act
            Validator.TryValidateObject(base64, validationContext, validationResults, true);

            //Assert
            return validationResults.FirstOrDefault()?.ErrorMessage;
        }

        public class Base64Message
        {
            [Base64]
            public string Base64 { get; set; }
        }
    }
}
