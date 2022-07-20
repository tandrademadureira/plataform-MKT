using NUnit.Framework;
using Shared.Util.Extension;

namespace Shared.Util.Test.Extension
{
    [TestFixture]
    public class ByteExtensionTest
    {
        [Test]
        public void ToByteArray()
        {
            //Arrange
            var foo = "Smarkets App";
            var byteArray = foo.ToByteArray();

            //Act
            var result = byteArray.ToObject<string>();

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(string.IsNullOrWhiteSpace(result), Is.False);
            Assert.That(result, Is.EqualTo("Smarkets App"));

        }
    }
}