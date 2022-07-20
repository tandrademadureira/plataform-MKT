using NUnit.Framework;
using Shared.Util.Extension;

namespace Shared.Util.Test.Extension
{
    [TestFixture]
    public class EnumExtensionTest
    {
        [TestCase(FooEnum.Bar, ExpectedResult = "Bar")]
        public string GetDescription(FooEnum value)
        {
            //Arrange

            //Act
            var description = value.GetDescription();

            //Assert
            return description;
        }

        [TestCase(FooEnum.Sample, ExpectedResult = null)]
        public string GetDescriptionNull(FooEnum value)
        {
            //Arrange

            //Act
            var description = value.GetDescription();

            //Assert
            return description;
        }


        [TestCase(FooEnum.Bar, ExpectedResult = "Bar")]
        [TestCase(FooEnum.Sample, ExpectedResult = "Sample")]
        public string GetName(FooEnum value)
        {
            //Arrange

            //Act
            var name = value.GetName<FooEnum>();

            //Assert
            return name;
        }

        public enum FooEnum
        {
            [System.ComponentModel.Description("Bar")]
            Bar = 1,
            Sample = 2
        }
    }
}
