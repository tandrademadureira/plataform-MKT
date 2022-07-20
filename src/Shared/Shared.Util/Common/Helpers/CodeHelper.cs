using System;
using System.Security.Cryptography;

namespace Shared.Util.Common.Helpers
{
    /// <summary>
    /// Common class with helper methods for Code.
    /// </summary>
    public static class CodeHelper
    {
        /// <summary>
        /// Get one code using the algorithm randomic.
        /// </summary>
        /// <param name="length">Length of code generated.</param>
        /// <returns>Generated code.</returns>
        /// <example>
        /// <code>
        /// var var code = CodeHelper.GetCodeByRandom();
        /// </code>
        /// The result is similar to *abc123d4*.
        /// </example>
        public static string GetCodeByRandom(int length = 8)
        {
            if (length > 32)
                length = 32;

            if (length <= 0)
                length = 1;

            var validValues = "abcdefghijklmnopqrstuvxwyz0123456789";

            var randomGenerator = RandomNumberGenerator.Create();
            var result = "";

            while (result.Length < length)
            {
                var oneByte = new byte[1];
                randomGenerator.GetBytes(oneByte);
                var character = (char)oneByte[0];
                if (validValues.Contains(character))
                {
                    result += character;
                }
            }

            return result;
        }

        /// <summary>
        /// Get one code using the guid as base.
        /// </summary>
        /// <param name="length">Length of code generated.</param>
        /// <returns>Generated code.</returns>
        /// <example>
        /// <code>
        /// var var code = CodeHelper.GetCodeByGuid();
        /// </code>
        /// The result is similar to *24240d47*.
        /// </example>
        public static string GetCodeByGuid(int length = 8)
        {
            if (length > 32)
                length = 32;

            if (length <= 0)
                length = 1;

            return Guid.NewGuid().ToString("N").Substring(0, length);
        }
    }
}
