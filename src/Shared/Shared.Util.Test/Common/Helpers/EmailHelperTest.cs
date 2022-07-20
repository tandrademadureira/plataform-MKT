using NUnit.Framework;
using Shared.Util.Common.Helpers;

namespace Shared.Util.Test.Common.Helpers
{
    [TestFixture]
    public class EmailHelperTest
    {
        [Test]
        public void IsValidEmail()
        {
            //Arrange
            var email = "teste@smarkets.com.br";

            //Act
            var isValid = EmailHelper.IsValidEmail(email);

            //Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void IsNotValidEmail()
        {
            //Arrange
            var email = "t@este@smarkets.com.br";

            //Act
            var isValid = EmailHelper.IsValidEmail(email);

            //Assert
            Assert.That(isValid, Is.False);
        }
    }
}
