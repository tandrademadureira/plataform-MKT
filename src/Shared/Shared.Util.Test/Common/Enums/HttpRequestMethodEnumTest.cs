using NUnit.Framework;
using Shared.Util.Common.Enums;
using Shared.Util.Extension;

namespace Shared.Util.Test.Common.Enums
{
    [TestFixture]
    public class HttpRequestMethodEnumTest
    {
        [TestCase(HttpRequestMethod.Get, ExpectedResult = 1)]
        [TestCase(HttpRequestMethod.Post, ExpectedResult = 2)]
        [TestCase(HttpRequestMethod.Put, ExpectedResult = 4)]
        [TestCase(HttpRequestMethod.Delete, ExpectedResult = 8)]
        [TestCase(HttpRequestMethod.Head, ExpectedResult = 16)]
        [TestCase(HttpRequestMethod.Patch, ExpectedResult = 32)]
        [TestCase(HttpRequestMethod.Options, ExpectedResult = 64)]
        public int HttpStatusEnumValidateCode(HttpRequestMethod value)
        {
            //Arrange
            var code = 0;

            //Act
            code = (int)value;

            //Assert
            return code;
        }

        [TestCase(HttpRequestMethod.Get, ExpectedResult = "Get")]
        [TestCase(HttpRequestMethod.Post, ExpectedResult = "Post")]
        [TestCase(HttpRequestMethod.Put, ExpectedResult = "Put")]
        [TestCase(HttpRequestMethod.Delete, ExpectedResult = "Delete")]
        [TestCase(HttpRequestMethod.Head, ExpectedResult = "Head")]
        [TestCase(HttpRequestMethod.Patch, ExpectedResult = "Patch")]
        [TestCase(HttpRequestMethod.Options, ExpectedResult = "Options")]
        public string HttpStatusEnumValidateDescription(HttpRequestMethod value)
        {
            //Arrange
            var description = string.Empty;

            //Act
            description = value.GetDescription();

            //Assert
            return description;
        }
    }
}
