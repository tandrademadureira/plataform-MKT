using NUnit.Framework;
using Shared.Util.Common.Helpers;

namespace Shared.Util.Test.Common.Helpers
{
    [TestFixture]
    public class CnpjHelperTest
    {
        [Test]
        public void IsValidCnpj()
        {
            //Arrange
            var cnpj = "82876360000116";

            //Act
            var isValid = CnpjHelper.IsValidCnpj(cnpj);

            //Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void FormatCnpj()
        {
            //Arrange
            var cnpj = "82876360000116";

            //Act
            var formatCnpj = CnpjHelper.FormatCnpj(cnpj);

            //Assert
            Assert.That(formatCnpj, Is.EqualTo("82.876.360/0001-16"));
        }
    }
}
