using Shared.Util.Common.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Shared.Util.Common.Attributes
{
    /// <summary>
    /// Check if the value is CNPJ valid.
    /// </summary>
    public class CnpjAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the value string CNPJ specified is valid.
        /// <para>
        /// <note type="note">If object is null the return is true, because the field may be ***not*** required.</note>
        /// </para>
        /// </summary>
        /// <param name="value">The value of the string CNPJ to validate.</param>
        /// <returns>True if the specified value is valid, otherwise, false.</returns>
        /// <example>
        /// <code>
        /// public class CnpjMessage
        /// {
        ///     [Cnpj]
        ///     public string Cnpj { get; set; }
        /// }
        /// </code>
        /// </example>
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            ErrorMessage = "Value is not CNPJ valid.";

            return CnpjHelper.IsValidCnpj(value.ToString());
        }
    }
}
