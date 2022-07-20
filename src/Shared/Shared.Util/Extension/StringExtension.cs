using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Shared.Util.Extension
{
    /// <summary>
    /// Extension for string.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Try parse string date value, if not parsed the null value is returned.
        /// </summary>
        /// <param name="value">String value to try parse.</param>
        /// <returns>DateTime value or null.</returns>
        /// <example>
        /// <code>
        /// var date = "01/16/2018";
        /// var dateParse = date.TryParseDateTimeOrNullable();
        /// </code>
        /// </example>
        public static DateTime? TryParseDateTimeOrNullable(this string value) => DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime date) ? (DateTime?)date : null;

        /// <summary>
        /// Try parse string base 64 value, if not parsed the null value is returned.
        /// </summary>
        /// <param name="value">String value to try parse.</param>
        /// <returns>Byte array or null</returns>
        /// <example>
        /// <code>
        /// var base64 = "U3RvbmUgQ28=";
        /// var byteArray = base64.TryParseBase64OrNullable();
        /// </code>
        /// </example>
        public static byte[] TryParseBase64OrNullable(this string value)
        {
            var buffer = new Span<byte>(new byte[value.Length]);

            if (Convert.TryFromBase64String(value, buffer, out int bytesParsed))
                return value.ToByteArray();

            return null;
        }

        /// <summary>
        /// Try parse string enum name, if not parsed the null value is returned.
        /// </summary>
        /// <typeparam name="TEnum">Enum type provider.</typeparam>
        /// <param name="value">String value to try parse.</param>
        /// <returns>Enum or null</returns>
        /// <example>
        /// Foo enum used in this example.
        /// <code>
        /// public enum FooEnum
        /// {
        ///     [Description("Bar Description")]
        ///     Bar = 1
        /// }
        /// </code>
        /// Example for try parse to the FooEnum.
        /// <code>
        /// public class EnumTryParseSample
        /// {
        ///     public void GetEnum()
        ///     {
        ///         var nameBar = "Bar";
        ///         var enumBar = nameBar.TryParseEnumOrNullable<![CDATA[<FooEnum>]]>();
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentException">If typeof ***TEnum*** is different of Enum struct, an exception is thrown with message <c>'Type provided must be an Enum.'</c>.</exception>
        public static TEnum? TryParseEnumOrNullable<TEnum>(this string value) where TEnum : struct => Enum.TryParse(value, true, out TEnum result) ? (TEnum?)result : null;

        /// <summary>
        /// Try parse string int value, if not parsed the null value is returned.
        /// </summary>
        /// <param name="value">String value to try parse.</param>
        /// <returns>Int or null</returns>
        /// <example>
        /// <code>
        /// var number = "15";
        /// var numberInt = number.TryParseIntOrNullable();
        /// </code>
        /// </example>
        public static int? TryParseIntOrNullable(this string value) => int.TryParse(value, out int number) ? number : null;

        /// <summary>
        /// Try parse string decimal value, if not parsed the null value is returned.
        /// This approach uses the invariant culture type and the numeric string can have a decimal point.
        /// </summary>
        /// <param name="value">String value to try parse.</param>
        /// <returns>Decimal or null</returns>
        /// <example>
        /// <code>
        /// var number = "15.02";
        /// var numberDecimal = number.TryParseDecimalOrNullable();
        /// </code>
        /// </example>
        public static decimal? TryParseDecimalOrNullable(this string value) => decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal number) ? number : null;

        /// <summary>
        /// Extract a content between two delimiters.
        /// </summary>
        /// <param name="source">Origin content.</param>
        /// <param name="beginDelim">First string delimiter.</param>
        /// <param name="endDelim">Last string delimiter.</param>
        /// <param name="caseSensitive">If consider uppercase or lowercase to delimiter.</param>
        /// <param name="allowMissingEndDelimiter">If not found endDelimin, return the value, since begindelim.</param>
        /// <returns>First string between both delimiters .</returns>
        /// <example>
        /// <note type="note">This is an initial sample that will be used for the following examples.</note>
        /// <code>
        /// string appSettings = @"
        /// {
        ///  ""AppSettings"": {
        ///    ""HostingSettings"": {
        ///      ""HostAddress"": {
        ///        ""Addresses"": {
        ///          ""ApiAddress"": ""#{api_address}#""
        ///        }
        ///      }
        ///    }
        ///  }
        ///}
        /// ";
        /// </code>
        /// 
        /// **Example 1** - Defining first and last Delimiters:
        /// <code>
        /// string result = appSettings.Extract("#{", "}#");
        /// </code>
        /// Returns first value founded: *api_address*
        /// 
        /// **Example 2** - Defining first and last Delimiters, desconsidering sensitive case:
        /// <code>
        /// string result = appSettings.Extract("app", "\":");
        /// </code>
        /// Returns first value founded: *Settings*
        /// 
        /// **Example 3** - Defining first and last Delimiters, considering sensitive case:
        /// <code>
        /// string result = appSettings.Extract("app", "\":", true);
        /// </code>
        ///  Returns *null*
        /// 
        /// **Example 4** - Defining first and last Delimiters, considering sensitive case and not allow missing end delimiter:
        /// <code>
        /// string appSettings = "[ApiAddress = Smarkets.app]"
        /// 
        /// string result = appSettings.Extract("ApiAddress = ", "!", true, false);
        /// </code>
        /// Returns *null*
        /// 
        /// **Example 5** - Defining first and last Delimiters, considering sensitive case and allow missing end delimiter:
        /// <code>
        /// string appSettings = "ApiAddress=Smarkets.app"
        /// 
        /// string result = appSettings.Extract("ApiAddress=", "!", true, true);
        /// </code>
        /// Returns *Smarkets.app*
        /// 
        /// </example>
        public static string Extract(this string source, string beginDelim, string endDelim, bool caseSensitive = false, bool allowMissingEndDelimiter = false)
        {
            int posIni, posEnd;

            if (string.IsNullOrEmpty(source))
                return null;

            if (caseSensitive)
            {
                posIni = source.IndexOf(beginDelim);
                if (posIni == -1)
                    return null;

                posEnd = source.IndexOf(endDelim, posIni + beginDelim.Length);
            }
            else
            {
                string Lower = source.ToLower();
                posIni = source.IndexOf(beginDelim, 0, source.Length, StringComparison.OrdinalIgnoreCase);
                if (posIni == -1)
                    return null;

                posEnd = source.IndexOf(endDelim, posIni + beginDelim.Length, StringComparison.OrdinalIgnoreCase);
            }

            if (allowMissingEndDelimiter && posEnd == -1)
                return source.Substring(posIni + beginDelim.Length);

            if (posIni > -1 && posEnd > 1)
                return source.Substring(posIni + beginDelim.Length, posEnd - posIni - beginDelim.Length);

            return null;
        }

        /// <summary>
        /// Checks whether all characters of a string are decimal characters.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <param name="offset">Offset to start reading.</param>
        /// <param name="count">Number fo characters to check.</param>
        /// <returns>'true' if all characters are decimal characters.</returns>
        /// <example>
        /// <code>
        /// var stringValue = "2501";
        /// var offset = 2;
        /// var isDecimal = stringValue.IsDecimal(offset, 2);
        /// </code>
        /// </example>
        /// <exception cref="IndexOutOfRangeException">If ***offset*** is negative, an exception is thrown with message <c>'Offset cannot be negative.'</c>.</exception>
        /// <exception cref="IndexOutOfRangeException">If ***count*** is negative, an exception is thrown with message <c>'Count cannot be negative.'</c>.</exception>
        /// <exception cref="IndexOutOfRangeException">If ***offset*** and ***count*** combination less than string length, an exception is thrown with message <c>'Offset and count combination is out of range.'</c>.</exception>
        public static bool IsDecimal(this string value, int offset, int count)
        {
            if (value == null) return false;

            if (offset < 0)
                throw new IndexOutOfRangeException("Offset cannot be negative.");
            if (count < 0)
                throw new IndexOutOfRangeException("Count cannot be negative.");

            if (count + offset > value.Length)
                throw new IndexOutOfRangeException("Offset and count combination is out of range.");

            for (int i = 0; i < count; ++i)
            {
                char c = value[i + offset];

                bool valid = (c >= '0' && c <= '9');

                if (valid == false) return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether all characters of a string are decimal characters.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <param name="offset">Offset to start reading.</param>
        /// <returns>'true' if all characters are decimal characters.</returns>
        /// <example>
        /// <code>
        /// var stringValue = "25";
        /// var offset = 2;
        /// var isDecimal = stringValue.IsDecimal(offset);
        /// </code>
        /// </example>
        public static bool IsDecimal(this string value, int offset)
        {
            if (value == null) return false;

            return IsDecimal(value, offset, value.Length - offset);
        }

        /// <summary>
        /// Checks whether all characters of a string are decimal characters.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <returns>'true' if all characters are decimal characters.</returns>
        /// <example>
        /// <code>
        /// var stringValue = "25";
        /// var isDecimal = stringValue.IsDecimal();
        /// </code>
        /// </example>
        public static bool IsDecimal(this string value)
        {
            if (value == null) return false;

            return IsDecimal(value, 0, value.Length);
        }

        /// <summary>
        /// Get the string with no special characters.
        /// <para>If the value is empty, null, or has only spaces, the same value that was provided is returned.</para>
        /// </summary>
        /// <param name="value">Value that will be removed from the special characters.</param>
        /// <returns>Value without special characters or value provided.</returns>
        /// <example>
        /// <code>
        /// var value = "21.000-000";
        /// var valueWithoutSpecialCharacters = value.GetWithoutSpecialCharacter(); 
        /// </code>
        /// The result is *21000000*.
        /// </example>
        public static string GetWithoutSpecialCharacter(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            value = Regex.Replace(value, "(?:[^a-z0-9A-ZÁÉÍÓÚÂÊÔÀÔÃÇáéíóúâêôàõãç]|(?<=['\"])s)", string.Empty, RegexOptions.CultureInvariant | RegexOptions.Compiled);

            var result = string.Empty;

            const string vWithAccent = "ÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÒÓÔÕÖÙÚÛÜàáâãäåçèéêëìíîïòóôõöùúûü";
            const string vWithoutAccent = "AAAAAACEEEEIIIIOOOOOUUUUaaaaaaceeeeiiiiooooouuuu";

            for (var i = 1; (i <= value.Length); i++)
            {
                int vPos = (vWithAccent.IndexOf(value.Substring((i - 1), 1), 0, comparisonType: StringComparison.Ordinal) + 1);

                if ((vPos > 0))
                    result += vWithoutAccent.Substring((vPos - 1), 1);
                else
                    result += value.Substring((i - 1), 1);
            }

            return result;
        }

        /// <summary>
        /// Get words with the standardized spaces.
        /// <para>If the value is empty, null, or has only spaces, the same value that was provided is returned.</para>
        /// </summary>
        /// <param name="value">Value with the words that will be standardized.</param>
        /// <returns>Words with standard spaces.</returns>
        /// <example>
        /// <code>
        /// var value = "A    sentence        with spaces."
        /// var formatedValue = value.ToStandardSpaces();
        /// </code>
        /// The result is *A sentence with spaces.*
        /// </example>
        public static string GetStandardSpaces(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var regex = new Regex(@"\s+", RegexOptions.CultureInvariant | RegexOptions.Compiled);

            value = value.Replace("\t", " ");

            return regex.Replace(value, " ");
        }
    }
}

