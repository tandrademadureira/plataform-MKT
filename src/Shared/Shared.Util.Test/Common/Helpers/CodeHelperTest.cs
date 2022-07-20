using NUnit.Framework;
using Shared.Util.Common.Helpers;

namespace Shared.Util.Test.Common.Helpers
{
    [TestFixture]
    public class CodeHelperTest
    {
        [Test]
        public void GetCodeByRandom()
        {
            //Arrange

            //Act
            var code = CodeHelper.GetCodeByRandom(10);

            //Assert
            Assert.That(code.Length, Is.EqualTo(10));
        }

        [Test]
        public void GetCodeByGuid()
        {
            //Arrange

            //Act
            var code = CodeHelper.GetCodeByGuid(10);

            //Assert
            Assert.That(code.Length, Is.EqualTo(10));
        }
    }
}
