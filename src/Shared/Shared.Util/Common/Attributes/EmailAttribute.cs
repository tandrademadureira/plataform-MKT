using Shared.Util.Common.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Shared.Util.Common.Attributes
{
    /// <summary>
    /// Check if the value is e-mail valid.
    /// </summary>
    public class EmailAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the value string e-mail specified is valid.
        /// <para>
        /// <note type="note">If object is null the return is true, because the field may be ***not*** required.</note>
        /// </para>
        /// </summary>
        /// <param name="value">The value of the string e-mail to validate.</param>
        /// <returns>True if the specified value is valid, otherwise, false.</returns>
        /// <example>
        /// <code>
        /// public class CpfMessage
        /// {
        ///     [Email]
        ///     public string Email { get; set; }
        /// }
        /// </code>
        /// </example>
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            ErrorMessage = "Value is not e-mail valid.";

            return EmailHelper.IsValidEmail(value.ToString());
        }
    }
}
