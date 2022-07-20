using NUnit.Framework;
using Shared.Util.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shared.Util.Test.Common.Attributes
{
    [TestFixture]
    public class CollectionHaveAnyAttributeTest
    {
        [TestCase(false, "Smarkets", ExpectedResult = true)]
        [TestCase(false, "Smarkets", "Smarkets com br", ExpectedResult = true)]
        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public bool CollectionHaveAnyAttributeValidate(bool setNull, params string[] values)
        {
            //Arrange
            var validationResults = new List<ValidationResult>();
            var collection = new CollectionMessage { ListOfString = setNull ? null : values?.ToList() };
            var validationContext = new ValidationContext(collection);

            //Act
            Validator.TryValidateObject(collection, validationContext, validationResults, true);

            //Assert
            return validationResults.Count == 0;
        }

        [TestCase(false, "Smarkets", ExpectedResult = null)]
        [TestCase(false, "Smarkets", "Smarkets com br", ExpectedResult = null)]
        [TestCase(true, ExpectedResult = null)]
        [TestCase(false, ExpectedResult = "The collection must have any item.")]
        public string CollectionHaveAnyAttributeMessage(bool setNull, params string[] values)
        {
            //Arrange
            var validationResults = new List<ValidationResult>();
            var collection = new CollectionMessage { ListOfString = setNull ? null : values?.ToList() };
            var validationContext = new ValidationContext(collection);

            //Act
            Validator.TryValidateObject(collection, validationContext, validationResults, true);

            //Assert
            return validationResults.FirstOrDefault()?.ErrorMessage;
        }

        [TestCase(false, "Smarkets", ExpectedResult = true)]
        [TestCase(false, "Smarkets", "Smarkets com br", ExpectedResult = true)]
        [TestCase(true, ExpectedResult = false)]
        [TestCase(false, ExpectedResult = false)]
        public bool CollectionHaveAnyAttributeRequiredValidate(bool setNull, params string[] values)
        {
            //Arrange
            var validationResults = new List<ValidationResult>();
            var collection = new CollectionRequiredMessage { ListOfString = setNull ? null : values?.ToList() };
            var validationContext = new ValidationContext(collection);

            //Act
            Validator.TryValidateObject(collection, validationContext, validationResults, true);

            //Assert
            return validationResults.Count == 0;
        }

        [TestCase(false, "Smarkets", ExpectedResult = null)]
        [TestCase(false, "Smarkets", "Smarkets com br", ExpectedResult = null)]
        [TestCase(true, ExpectedResult = "The collection must have any item.")]
        [TestCase(false, ExpectedResult = "The collection must have any item.")]
        public string CollectionHaveAnyAttributeRequiredMessage(bool setNull, params string[] values)
        {
            //Arrange
            var validationResults = new List<ValidationResult>();
            var collection = new CollectionRequiredMessage { ListOfString = setNull ? null : values?.ToList() };
            var validationContext = new ValidationContext(collection);

            //Act
            Validator.TryValidateObject(collection, validationContext, validationResults, true);

            //Assert
            return validationResults.FirstOrDefault()?.ErrorMessage;
        }

        [Test]
        public void CollectionHaveAnyAttributeException()
        {
            //Arrange
            var validationResults = new List<ValidationResult>();
            var collection = new CollectionMessageException { Value = "Smarkets" };
            var validationContext = new ValidationContext(collection);

            //Act
            var ex = Assert.Throws<ArgumentException>(() => Validator.TryValidateObject(collection, validationContext, validationResults, true));

            //Assert
            Assert.That(ex.Message, Is.EqualTo("The provided object must interfaced of 'ICollection', check the object type or remove the 'CollectionHaveAnyAttribute'."));
        }

        public class CollectionMessage
        {
            [CollectionHaveAny]
            public List<string> ListOfString { get; set; }
        }

        public class CollectionRequiredMessage
        {
            [CollectionHaveAny(true)]
            public List<string> ListOfString { get; set; }
        }

        public class CollectionMessageException
        {
            [CollectionHaveAny()]
            public string Value { get; set; }
        }
    }
}
