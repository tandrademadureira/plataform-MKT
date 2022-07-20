using NUnit.Framework;
using Shared.Util.Common.Helpers;
using System.Collections.Generic;

namespace Shared.Util.Test.Common.Helpers
{
    [TestFixture]
    public class JsonHelperTest
    {
        [Test]
        public void MaskSensitiveDataValidate()
        {
            //Arrange
            var foo = new Foo { Cpf = "15486325787", Name = "Stephen William Hawking" };
            var sensitiveProperties = new List<string> { "cpf" };
            var jsonString = System.Text.Json.JsonSerializer.Serialize(foo);

            //Act
            var result = JsonHelper.MaskSensitiveData(jsonString, sensitiveProperties);
            var newFoo = System.Text.Json.JsonSerializer.Deserialize<Foo>(result);

            //Assert
            Assert.That(newFoo.Cpf, Is.EqualTo("***"));
        }

        [TestCase(@"{""Cpf"":""15486325787"", ""Name"":""Stephen William Hawking""}", ExpectedResult = true)]
        [TestCase("---", ExpectedResult = false)]
        [TestCase("Smarkets com br", ExpectedResult = false)]
        public bool JSONStringValidate(string json)
        {
            //Arrange

            //Act
            var validation = JsonHelper.IsValidJson(json);

            //Assert
            return validation;
        }

        public class Foo
        {
            public string Name { get; set; }

            public string Cpf { get; set; }
        }
    }
}
