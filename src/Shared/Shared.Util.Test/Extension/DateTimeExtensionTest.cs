using NUnit.Framework;
using Shared.Util.Extension;
using System;

namespace Shared.Util.Test.Extension
{
    [TestFixture]
    public class DateTimeExtensionTest
    {
        [TestCase("9/10/2012", "9/10/2012", ExpectedResult = 0)]
        [TestCase("9/10/2012", "9/11/2012", ExpectedResult = 1)]
        [TestCase("9/10/2012", "9/12/2012", ExpectedResult = 2)]
        public int ToDaysBetweenDifferentDates(DateTime date, DateTime dateToCompare)
        {
            //Arrange

            //Act
            var days = date.ToDaysBetweenDifferentDates(dateToCompare);

            //Assert
            return days;
        }

        [TestCase("9/11/2012", "9/10/2012")]
        public void ToDaysBetweenDifferentDatesException(DateTime date, DateTime dateToCompare)
        {
            //Arrange

            //Act

            //Assert
            Assert.That(() => date.ToDaysBetweenDifferentDates(dateToCompare), Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase("9/10/2012", "9/26/2012", ExpectedResult = 0)]
        [TestCase("10/01/2012", "11/30/2012", ExpectedResult = 1)]
        [TestCase("9/10/2012", "12/12/2013", ExpectedResult = 15)]
        public int ToMonthsBetweenDifferentDates(DateTime date, DateTime dateToCompare)
        {
            //Arrange

            //Act
            var months = date.ToMonthsBetweenDifferentDates(dateToCompare);

            //Assert
            return months;
        }

        [TestCase("9/11/2012", "9/10/2012")]
        public void ToMonthsBetweenDifferentDatesException(DateTime date, DateTime dateToCompare)
        {
            //Arrange

            //Act

            //Assert
            Assert.That(() => date.ToMonthsBetweenDifferentDates(dateToCompare), Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase("9/10/2012", "9/26/2012", ExpectedResult = 0)]
        [TestCase("10/01/2012", "11/30/2013", ExpectedResult = 1)]
        [TestCase("9/10/2012", "12/12/2016", ExpectedResult = 4)]
        public int ToYearsBetweenDifferentDates(DateTime date, DateTime dateToCompare)
        {
            //Arrange

            //Act
            var years = date.ToYearsBetweenDifferentDates(dateToCompare);

            //Assert
            return years;
        }

        [TestCase("9/10/2012", "9/10/2011")]
        public void ToYearsBetweenDifferentDatesException(DateTime date, DateTime dateToCompare)
        {
            //Arrange

            //Act

            //Assert
            Assert.That(() => date.ToYearsBetweenDifferentDates(dateToCompare), Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase("10/10/2012", ExpectedResult = 31)]
        [TestCase("9/10/2012", ExpectedResult = 30)]
        [TestCase("2/9/2012", ExpectedResult = 29)]
        public int ToLastDayOfMonth(DateTime date)
        {
            //Arrange

            //Act
            var lastDayofMonth = date.ToLastDayOfMonth();

            //Assert
            return lastDayofMonth.Day;
        }

        [TestCase("10/31/2012", ExpectedResult = true)]
        [TestCase("9/10/2012", ExpectedResult = false)]
        public bool IsLastDayOfMonth(DateTime date)
        {
            //Arrange

            //Act
            var isLastDayOfMonth = date.IsLastDayOfMonth();

            //Assert
            return isLastDayOfMonth;
        }

        [TestCase("10/10/2012", ExpectedResult = 1)]
        [TestCase("9/10/2012", ExpectedResult = 1)]
        [TestCase("2/9/2012", ExpectedResult = 1)]
        public int ToFirstDayOfMonth(DateTime date)
        {
            //Arrange

            //Act
            var firstDayofMonth = date.ToFirstDayOfMonth();

            //Assert
            return firstDayofMonth.Day;
        }

        [TestCase("10/1/2012", ExpectedResult = true)]
        [TestCase("9/10/2012", ExpectedResult = false)]
        public bool IsFirstDayOfMonth(DateTime date)
        {
            //Arrange

            //Act
            var isFirstDayOfMonth = date.IsFirstDayOfMonth();

            //Assert
            return isFirstDayOfMonth;
        }

        [TestCase("10/10/2012", ExpectedResult = 21)]
        [TestCase("9/10/2012", ExpectedResult = 20)]
        [TestCase("2/9/2011", ExpectedResult = 19)]
        public int ToDaysToEndOfMonth(DateTime date)
        {
            //Arrange

            //Act
            var daysUntilEndOfMonth = date.ToDaysToEndOfMonth();

            //Assert
            return daysUntilEndOfMonth;
        }

        [TestCase("10/10/2012", true, ExpectedResult = 22)]
        [TestCase("9/10/2012", true, ExpectedResult = 21)]
        [TestCase("2/9/2011", true, ExpectedResult = 20)]
        [TestCase("2/9/2011", false, ExpectedResult = 19)]
        public int ToDaysToEndOfMonthIncludeCurrentDay(DateTime date, bool includeCurrentDay)
        {
            //Arrange

            //Act
            var daysUntilEndOfMonth = date.ToDaysToEndOfMonth(includeCurrentDay);

            //Assert
            return daysUntilEndOfMonth;
        }

        [TestCase("10/10/2012", ExpectedResult = 82)]
        [TestCase("9/10/2012", ExpectedResult = 112)]
        [TestCase("2/9/2011", ExpectedResult = 325)]
        public int ToDaysToEndOfYear(DateTime date)
        {
            //Arrange

            //Act
            var daysUntilEndOfYear = date.ToDaysToEndOfYear();

            //Assert
            return daysUntilEndOfYear;
        }

        [TestCase("10/10/2012", true, ExpectedResult = 83)]
        [TestCase("9/10/2012", true, ExpectedResult = 113)]
        [TestCase("2/9/2011", true, ExpectedResult = 326)]
        [TestCase("2/9/2011", false, ExpectedResult = 325)]
        public int ToDaysToEndOfYearIncludeCurrentDay(DateTime date, bool includeCurrentDay)
        {
            //Arrange

            //Act
            var daysUntilEndOfYear = date.ToDaysToEndOfYear(includeCurrentDay);

            //Assert
            return daysUntilEndOfYear;
        }

        [TestCase("10/10/2012", ExpectedResult = "10/10/2012 23:59:59 999")]
        [TestCase("09/10/2012", ExpectedResult = "09/10/2012 23:59:59 999")]
        [TestCase("02/09/2011", ExpectedResult = "02/09/2011 23:59:59 999")]
        public string ToDateWithLastTime(DateTime date)
        {
            //Arrange

            //Act
            var dateWithLastTime = date.ToDateWithLastTime();

            //Assert
            return dateWithLastTime.ToString("MM/dd/yyyy HH:mm:ss fff");
        }

        [TestCase("03/03/1983", "03/26/2019", ExpectedResult = 36)]
        [TestCase("03/03/1983", "12/21/2012", ExpectedResult = 29)]
        [TestCase("03/03/1983", "12/27/1983", ExpectedResult = 0)]
        [TestCase("03/03/1983", "12/27/3071", ExpectedResult = 1088)]
        public int ToAge(DateTime dateBirth, DateTime dateToCalculate)
        {
            //Arrange

            //Act
            var age = dateBirth.ToAge(dateToCalculate);

            //Assert
            return age;
        }

        [TestCase("03/03/1983", "03/03/1978")]
        public void ToAgeException(DateTime dateBirth, DateTime dateToCalculate)
        {
            //Arrange

            //Act

            //Assert
            Assert.That(() => dateBirth.ToAge(dateToCalculate), Throws.TypeOf<InvalidOperationException>());
        }
    }
}
