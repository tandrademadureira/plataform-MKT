using NUnit.Framework;
using Shared.Util.Common.Helpers;

namespace Shared.Util.Test.Common.Helpers
{
    [TestFixture]
    public class CpfHelperTest
    {
        [Test]
        public void IsValidCpf()
        {
            //Arrange
            var cpf = "46131287570";

            //Act
            var isValid = CpfHelper.IsValidCpf(cpf);

            //Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void FormatCpf()
        {
            //Arrange
            var cpf = "46131287570";

            //Act
            var formatCpf = CpfHelper.FormatCpf(cpf);

            //Assert
            Assert.That(formatCpf, Is.EqualTo("461.312.875-70"));
        }
    }
}
