using NUnit.Framework;
using Shared.Util.Common.Constants;

namespace Shared.Util.Test.Common.Constantes
{
    [TestFixture]
    public class ConstHeadersTest
    {
        [TestCase("Authorization-Token", ExpectedResult = true)]
        [TestCase("Smarkets com br", ExpectedResult = false)]
        public bool Authorization(string value)
        {
            //Arrange
            var headerKey = string.Empty;

            //Act
            headerKey = Headers.AuthorizationToken;

            //Assert
            return headerKey == value;
        }

        [TestCase("Origin-Device", ExpectedResult = true)]
        [TestCase("Smarkets com br", ExpectedResult = false)]
        public bool OriginDevice(string value)
        {
            //Arrange
            var headerKey = string.Empty;

            //Act
            headerKey = Headers.OriginDevice;

            //Assert
            return headerKey == value;
        }

        [TestCase("Origin-Ip", ExpectedResult = true)]
        [TestCase("Smarkets com br", ExpectedResult = false)]
        public bool OriginIp(string value)
        {
            //Arrange
            var headerKey = string.Empty;

            //Act
            headerKey = Headers.OriginIp;

            //Assert
            return headerKey == value;
        }

        [TestCase("Correlation-Id", ExpectedResult = true)]
        [TestCase("Smarkets com br", ExpectedResult = false)]
        public bool CorrelationId(string value)
        {
            //Arrange
            var headerKey = string.Empty;

            //Act
            headerKey = Headers.CorrelationId;

            //Assert
            return headerKey == value;
        }
    }
}
