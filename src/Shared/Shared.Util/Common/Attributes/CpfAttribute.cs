using Shared.Util.Common.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Shared.Util.Common.Attributes
{
    /// <summary>
    /// Check if the value is CPF valid.
    /// </summary>
    public class CpfAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the value string CPF specified is valid.
        /// <para>
        /// <note type="note">If object is null the return is true, because the field may be ***not*** required.</note>
        /// </para>
        /// </summary>
        /// <param name="value">The value of the string CPF to validate.</param>
        /// <returns>True if the specified value is valid, otherwise, false.</returns>
        /// <example>
        /// <code>
        /// public class CpfMessage
        /// {
        ///     [Cpf]
        ///     public string Cpf { get; set; }
        /// }
        /// </code>
        /// </example>
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            ErrorMessage = "Value is not CPF valid.";

            return CpfHelper.IsValidCpf(value.ToString());
        }
    }
}
