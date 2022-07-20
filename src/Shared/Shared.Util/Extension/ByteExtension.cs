using System;
using System.Text.Json;

namespace Shared.Util.Extension
{
    /// <summary>
    /// Extension for byte.
    /// </summary>
    public static class ByteExtension
    {
        /// <summary>
        /// Get object from an byte array.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="value">Byte array to convert in object.</param>
        /// <returns>Object or null.</returns>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     public string GetObjectString()
        ///     {
        ///          var foo = "mkt.App";
        ///          var byteArray = foo.ToByteArray();
        /// 
        ///          return byteArray.ToObject<![CDATA[<string>]]>();
        ///     }
        /// }
        /// </code>
        /// </example>
        public static T ToObject<T>(this byte[] value)
        {
            if (value == null)
                return default;

            try { return JsonSerializer.Deserialize<T>(value); }
            catch (InvalidCastException) { return default; }
        }
    }

}
