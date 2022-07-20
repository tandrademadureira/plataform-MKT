using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Shared.Util.Common.Attributes
{
    /// <summary>
    /// Check if the collection have any item.
    /// </summary>
    public class CollectionHaveAnyAttribute : ValidationAttribute
    {
        private bool Required { get; }

        /// <summary>
        /// Constructor for set required.
        /// </summary>
        /// <param name="required">Value true for required or false to ignore required.</param>
        public CollectionHaveAnyAttribute(bool required = false) => Required = required;

        /// <summary>
        /// Determines whether the collection have any item.
        /// <para>
        /// <note type="note">If object is null and not set required true on the constructor, the return is true, because the field may be ***not*** required.</note>
        /// </para>
        /// <para>
        /// <note type="warning">The provided object must interfaced of ***ICollection***.</note>
        /// </para>
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the specified value is valid, otherwise, false.</returns>
        /// <example>
        /// Example to default ignore required collection.
        /// <code>
        /// public class CollectionMessage
        /// {
        ///     [CollectionHaveAny]
        ///     public List<![CDATA[<string>]]> ListOfString { get; set; }
        /// }
        /// </code>
        /// Example to set collection is required.
        /// <code>
        /// public class CollectionMessage
        /// {
        ///     [CollectionHaveAny(true)]
        ///     public List<![CDATA[<string>]]> ListOfString { get; set; }
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentException">If the provided object not interfaced of <c>ICollection</c>, an exception is thrown.</exception>
        public override bool IsValid(object value)
        {
            ErrorMessage = "The collection must have any item.";

            if (value == null) return !Required;

            try { return ((ICollection)value).Count > 0; } catch (InvalidCastException) { throw new ArgumentException("The provided object must interfaced of 'ICollection', check the object type or remove the 'CollectionHaveAnyAttribute'."); }
        }
    }
}
