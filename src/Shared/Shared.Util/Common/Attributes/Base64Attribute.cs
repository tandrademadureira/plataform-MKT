using Shared.Util.Extension;
using System.ComponentModel.DataAnnotations;

namespace Shared.Util.Common.Attributes
{
    /// <summary>
    /// Check if the value is Base64 valid.
    /// </summary>
    public class Base64Attribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the value string Base64 specified is valid.
        /// <para>
        /// <note type="note">If object is null the return is true, because the field may be ***not*** required.</note>
        /// </para>
        /// </summary>
        /// <param name="value">The value of the string Base64 to validate.</param>
        /// <returns>True if the specified value is valid, otherwise, false.</returns>
        /// <example>
        /// <code>
        /// public class Base64Message
        /// {
        ///     [Base64]
        ///     public string Base64 { get; set; }
        /// }
        /// </code>
        /// </example>
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            ErrorMessage = "Value is not Base64 valid.";

            return value.ToString().TryParseBase64OrNullable() != null;
        }
    }
}
