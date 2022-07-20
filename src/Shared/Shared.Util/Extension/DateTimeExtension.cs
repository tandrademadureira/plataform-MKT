using System;

namespace Shared.Util.Extension
{
    /// <summary>
    /// Extension for DateTime.
    /// </summary>
    public static class DateTimeExtension
    {
        private static string DateTimeOffsetParseErrorString => "String was not recognized as a valid '" + nameof(DateTimeOffset) + "'.";

        private static string DateTimeParseErrorString => "String was not recognized as a valid '" + nameof(DateTime) + "'.";

        private static int[] Power10Format => new int[] { 1000000, 100000, 10000, 1000, 100, 10, 1 };

        private static double[] Power10Parse => new double[] { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000 };

        /// <summary>
        /// Get days between two dates.
        /// </summary>
        /// <param name="date">Lowest value date.</param>
        /// <param name="dateToCompare">Highest value date.</param>
        /// <returns>Total days.</returns>
        /// <example>
        /// <code>
        /// var date = DateTime.Now;
        /// var dateToCompare = date.AddDays(5);
        /// 
        /// var days = date.ToDaysBetweenDifferentDates(dateToCompare);
        /// </code>
        /// </example>
        /// <exception cref="InvalidOperationException">If the <c>dateToCompare</c> is less than <c>date</c>, an exception is thrown.</exception>
        public static int ToDaysBetweenDifferentDates(this DateTime date, DateTime dateToCompare)
        {
            ValidateBetweenDates(date, dateToCompare);

            return (dateToCompare - date).Days;
        }

        /// <summary>
        /// Get months between two dates.
        /// </summary>
        /// <param name="date">Lowest value date.</param>
        /// <param name="dateToCompare">Highest value date.</param>
        /// <returns>Total months.</returns>
        /// <example>
        /// <code>
        /// var date = DateTime.Now;
        /// var dateToCompare = date.AddMonths(5);
        /// 
        /// var months = date.ToMonthsBetweenDifferentDates(dateToCompare);
        /// </code>
        /// </example>
        /// <exception cref="InvalidOperationException">If the <c>dateToCompare</c> is less than <c>date</c>, an exception is thrown.</exception>
        public static int ToMonthsBetweenDifferentDates(this DateTime date, DateTime dateToCompare)
        {
            ValidateBetweenDates(date, dateToCompare);

            int compMonth = (dateToCompare.Month + dateToCompare.Year * 12) - (date.Month + date.Year * 12);
            double daysInEndMonth = (dateToCompare - dateToCompare.AddMonths(1)).Days;
            double months = compMonth + (date.Day - dateToCompare.Day) / daysInEndMonth;

            return Convert.ToInt32(Math.Truncate(months));
        }

        /// <summary>
        /// Get years between two dates.
        /// </summary>
        /// <param name="date">Lowest value date.</param>
        /// <param name="dateToCompare">Highest value date.</param>
        /// <returns>Total years.</returns>
        /// <example>
        /// <code>
        /// var date = DateTime.Now;
        /// var dateToCompare = date.AddMonths(5);
        /// 
        /// var years = date.ToYearsBetweenDifferentDates(dateToCompare);
        /// </code>
        /// </example>
        /// <exception cref="InvalidOperationException">If the <c>dateToCompare</c> is less than <c>date</c>, an exception is thrown.</exception>
        public static int ToYearsBetweenDifferentDates(this DateTime date, DateTime dateToCompare)
        {
            ValidateBetweenDates(date, dateToCompare);

            return (dateToCompare - date).Days / 365;
        }

        /// <summary>
        /// Get last day of month.
        /// </summary>
        /// <param name="date">Date to get last day of month.</param>
        /// <returns>Last day of month.</returns>
        /// <example>
        /// <code>
        /// var date = DateTime.Now;
        /// var lastDayOfmonth = date.ToLastDayOfMonth();
        /// </code>
        /// </example>
        public static DateTime ToLastDayOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

        /// <summary>
        /// Check if it's the last day of the month.
        /// </summary>
        /// <param name="date">Date to check.</param>
        /// <returns>Value true or false</returns>
        /// <example>
        /// <code>
        /// var date = DateTime.Now;
        /// var isLastDayOfMonth = data.IsLastDayOfMonth();
        /// </code>
        /// </example>
        public static bool IsLastDayOfMonth(this DateTime date) => date.Date == date.ToLastDayOfMonth().Date;

        /// <summary>
        /// Get first day of month.
        /// </summary>
        /// <param name="date">Date to get first day of month.</param>
        /// <returns>First day of month.</returns>
        /// <example>
        /// <code>
        /// var date = DateTime.Now;
        /// var firstDayOfmonth = date.ToFirstDayOfMonth();
        /// </code>
        /// </example>
        public static DateTime ToFirstDayOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

        /// <summary>
        /// Check if it's the first day of the month.
        /// </summary>
        /// <param name="date">Date to check.</param>
        /// <returns>Value true or false</returns>
        /// <example>
        /// <code>
        /// var date = DateTime.Now;
        /// var isFirstDayOfMonth = data.IsFirstDayOfMonth();
        /// </code>
        /// </example>
        public static bool IsFirstDayOfMonth(this DateTime date) => date.Date == date.ToFirstDayOfMonth().Date;

        /// <summary>
        /// Get total days until the end of the month.
        /// </summary>
        /// <param name="date">Date to get total days.</param>
        /// <param name="includeCurrentDay">Include current day into total.</param>
        /// <returns>Total days.</returns>
        /// <example>
        /// Total days until the end of the month and ***excluding*** current day.
        /// <code>
        /// var date = DateTime.Now;
        /// var daysUntilEndOfMonth = date.ToDaysToEndOfMonth();
        /// </code>
        /// Total days until the end of the month and ***including*** current day.
        /// <code>
        /// var date = DateTime.Now;
        /// var includeCurrentDay = true;
        /// 
        /// var daysUntilEndOfMonth = date.ToDaysToEndOfMonth(includeCurrentDay);
        /// </code>
        /// </example>
        public static int ToDaysToEndOfMonth(this DateTime date, bool includeCurrentDay = false) => Convert.ToInt32(date.ToLastDayOfMonth().Subtract(date).TotalDays) + (includeCurrentDay ? 1 : 0);

        /// <summary>
        /// Get total days until the end of the year.
        /// </summary>
        /// <param name="date">Date to get total days.</param>
        /// <param name="includeCurrentDay">Include current day into total.</param>
        /// <returns>Total days.</returns>
        /// <example>
        /// Total days until the end of the year and ***excluding*** current day.
        /// <code>
        /// var date = DateTime.Now;
        /// var daysUntilEndOfYear = date.ToDaysToEndOfYear();
        /// </code>
        /// Total days until the end of the year and ***including*** current day.
        /// <code>
        /// var date = DateTime.Now;
        /// var includeCurrentDay = true;
        /// 
        /// var daysUntilEndOfYear = date.ToDaysToEndOfYear(includeCurrentDay);
        /// </code>
        /// </example>
        public static int ToDaysToEndOfYear(this DateTime date, bool includeCurrentDay = false) => Convert.ToInt32(new DateTime(date.Year, 12, 31).Subtract(date).TotalDays) + (includeCurrentDay ? 1 : 0);

        /// <summary>
        /// Get date with last time. Time format 23:59:59 999.
        /// </summary>
        /// <param name="date">Date to get last time.</param>
        /// <returns>Date with last time</returns>
        /// <example>
        /// <code>
        /// var date = DateTime.Now;
        /// 
        /// var dateWithLastTime = data.ToDateWithLastTime();
        /// </code>
        /// </example>
        public static DateTime ToDateWithLastTime(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);

        /// <summary>
        /// Get age
        /// </summary>
        /// <param name="dateBirth">Date of birth</param>
        /// <param name="dateToCalculate">Date to calculate age. If the date is not entered, the current date will be used.</param>
        /// <returns>Age</returns>
        /// <example>
        /// Calcule age and ***pass date to calculate*** .
        /// <code>
        /// var dateOfBirth = new DateTime(1983, 3, 3);
        /// var dateToCalculate = new DateTime(2012, 12, 21);
        /// 
        /// var age = dateOfBirth.ToAge(dateToCalculate);
        /// </code>
        /// Calcule age and ***not pass date to calculate*** .
        /// <code>
        /// var dateOfBirth = new DateTime(1983, 3, 3);
        /// 
        /// var age = dateOfBirth.ToAge();
        /// </code>
        /// </example>
        /// <exception cref="InvalidOperationException">If the <c>dateToCalculate</c> is less than <c>dateBirth</c>, an exception is thrown.</exception>

        public static int ToAge(this DateTime dateBirth, DateTime? dateToCalculate = null)
        {
            if (dateToCalculate.HasValue)
            {
                ValidateBetweenDates(dateBirth, dateToCalculate.Value);
                dateToCalculate = dateToCalculate.Value.Date;
            }
            else
                dateToCalculate = DateTime.Today;

            var age = dateToCalculate.Value.Year - dateBirth.Year;

            if (dateBirth.Date > dateToCalculate.Value.AddYears(-age))
                age--;

            return age;
        }

        /// <summary>
        /// Formats a 'DateTime' object into its string representation.
        /// </summary>
        /// <param name="value">'DateTime' object to be formatted.</param>
        /// <param name="decimalPlaces">Number of digits to the right of the decimal point.</param>
        /// <returns>String representation of the 'DateTime' object.</returns>

        public static string Format(this DateTime value, int decimalPlaces = 3)
        {
            if (decimalPlaces < 0)
                throw new ArgumentException(string.Format("Invalid value for '" + nameof(decimalPlaces) + "': minimum is {0}, got {1}.", 0, decimalPlaces));
            if (decimalPlaces > 7)
                throw new ArgumentException(string.Format("Invalid value for '" + nameof(decimalPlaces) + "': maximum is {0}, got {1}.", 7, decimalPlaces));

            const int zeroValue = '0';
            bool hasDecimalPlaces = decimalPlaces > 0;

            int resultLength = 19;
            if (hasDecimalPlaces)
            {
                resultLength += 1 + decimalPlaces;
            }
            string result = new('-', resultLength);

            int year = value.Year;
            int month = value.Month;
            int day = value.Day;
            int hour = value.Hour;
            int minute = value.Minute;
            int second = value.Second;

            unsafe
            {
                fixed (char* unsafeResult = result)
                {
                    unsafeResult[0] = (char)(zeroValue + year / 1000);
                    unsafeResult[1] = (char)(zeroValue + (year % 1000) / 100);
                    unsafeResult[2] = (char)(zeroValue + (year % 100) / 10);
                    unsafeResult[3] = (char)(zeroValue + year % 10);

                    unsafeResult[5] = (char)(zeroValue + month / 10);
                    unsafeResult[6] = (char)(zeroValue + month % 10);

                    unsafeResult[8] = (char)(zeroValue + day / 10);
                    unsafeResult[9] = (char)(zeroValue + day % 10);
                    unsafeResult[10] = 'T';

                    unsafeResult[11] = (char)(zeroValue + hour / 10);
                    unsafeResult[12] = (char)(zeroValue + hour % 10);
                    unsafeResult[13] = ':';

                    unsafeResult[14] = (char)(zeroValue + minute / 10);
                    unsafeResult[15] = (char)(zeroValue + minute % 10);
                    unsafeResult[16] = ':';

                    unsafeResult[17] = (char)(zeroValue + second / 10);
                    unsafeResult[18] = (char)(zeroValue + second % 10);

                    if (hasDecimalPlaces)
                    {
                        TimeSpan fractionTime = value.TimeOfDay - TimeSpan.FromSeconds(3600 * hour + 60 * minute + second);
                        int fraction = (int)(fractionTime.TotalMilliseconds * 10000);

                        unsafeResult[19] = '.';
                        unsafeResult[20] = (char)(zeroValue + (fraction / 1000000));
                        for (int i = 1; i < decimalPlaces; ++i)
                        {
                            unsafeResult[20 + i] = (char)(zeroValue + (fraction / Power10Format[i]) % 10);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Formats a 'DateTimeOffset' object into its string representation.
        /// </summary>
        /// <param name="value">'DateTimeOffset' object to be formatted.</param>
        /// <param name="decimalPlaces">Number of digits to the right of the decimal point.</param>
        /// <returns>String representation of the 'DateTimeOffset' object.</returns>
        public static string FormatWithOffset(this DateTimeOffset value, int decimalPlaces = 3)
        {
            if (decimalPlaces < 0)
                throw new ArgumentException(string.Format("Invalid value for '" + nameof(decimalPlaces) + "': minimum is {0}, got {1}.", 0, decimalPlaces));
            if (decimalPlaces > 7)
                throw new ArgumentException(string.Format("Invalid value for '" + nameof(decimalPlaces) + "': maximum is {0}, got {1}.", 7, decimalPlaces));

            const int zeroValue = '0';
            bool hasDecimalPlaces = decimalPlaces > 0;

            int resultLength = 25;
            if (hasDecimalPlaces)
            {
                resultLength += 1 + decimalPlaces;
            }
            string result = new('-', resultLength);

            int year = value.Year;
            int month = value.Month;
            int day = value.Day;
            int hour = value.Hour;
            int minute = value.Minute;
            int second = value.Second;

            unsafe
            {
                fixed (char* unsafeResult = result)
                {
                    unsafeResult[0] = (char)(zeroValue + year / 1000);
                    unsafeResult[1] = (char)(zeroValue + (year % 1000) / 100);
                    unsafeResult[2] = (char)(zeroValue + (year % 100) / 10);
                    unsafeResult[3] = (char)(zeroValue + year % 10);

                    unsafeResult[5] = (char)(zeroValue + month / 10);
                    unsafeResult[6] = (char)(zeroValue + month % 10);

                    unsafeResult[8] = (char)(zeroValue + day / 10);
                    unsafeResult[9] = (char)(zeroValue + day % 10);
                    unsafeResult[10] = 'T';

                    unsafeResult[11] = (char)(zeroValue + hour / 10);
                    unsafeResult[12] = (char)(zeroValue + hour % 10);
                    unsafeResult[13] = ':';

                    unsafeResult[14] = (char)(zeroValue + minute / 10);
                    unsafeResult[15] = (char)(zeroValue + minute % 10);
                    unsafeResult[16] = ':';

                    unsafeResult[17] = (char)(zeroValue + second / 10);
                    unsafeResult[18] = (char)(zeroValue + second % 10);

                    int raster = 19;

                    if (hasDecimalPlaces)
                    {
                        TimeSpan fractionTime = value.TimeOfDay - TimeSpan.FromSeconds(3600 * hour + 60 * minute + second);
                        int fraction = (int)(fractionTime.TotalMilliseconds * 10000);

                        unsafeResult[19] = '.';
                        unsafeResult[20] = (char)(zeroValue + (fraction / 1000000));
                        for (raster = 21; raster < decimalPlaces + 20; ++raster)
                        {
                            unsafeResult[raster] = (char)(zeroValue + (fraction / Power10Format[raster - 20]) % 10);
                        }
                    }

                    int offsetTotalMinutes = (int)value.Offset.TotalMinutes;
                    if (offsetTotalMinutes < 0)
                    {
                        offsetTotalMinutes = -offsetTotalMinutes;
                    }
                    else
                    {
                        unsafeResult[raster] = '+';
                    }
                    unsafeResult[++raster] = (char)(zeroValue + offsetTotalMinutes / 600);
                    unsafeResult[++raster] = (char)(zeroValue + (offsetTotalMinutes / 60) % 10);
                    unsafeResult[++raster] = ':';
                    unsafeResult[++raster] = (char)(zeroValue + (offsetTotalMinutes % 60) / 10);
                    unsafeResult[++raster] = (char)(zeroValue + offsetTotalMinutes % 10);
                }
            }

            return result;
        }

        /// <summary>
        /// Formats a 'DateTime' object into its string representation.
        /// </summary>
        /// <param name="value">Value for 'this'.</param>
        /// <param name="decimalPlaces">Number of digits to the right of the decimal point.</param>
        /// <returns>String representation of the 'DateTime' object.</returns>

        public static string FormatIso(this DateTime value, int decimalPlaces = 3) => Format(value, decimalPlaces);

        /// <summary>
        /// Formats a 'DateTimeOffset' object into its string representation.
        /// </summary>
        /// <param name="value">Value for 'this'.</param>
        /// <param name="decimalPlaces">Number of digits to the right of the decimal point.</param>
        /// <returns>String representation of the 'DateTimeOffset' object.</returns>
        public static string FormatIso(this DateTimeOffset value, int decimalPlaces = 3) => FormatWithOffset(value, decimalPlaces);

        private static void ValidateBetweenDates(DateTime date, DateTime dateToCompare)
        {
            if (date > dateToCompare)
                throw new InvalidOperationException("The 'dateToCompare' should be greater than or equal to 'date'.");
        }
    }
}
