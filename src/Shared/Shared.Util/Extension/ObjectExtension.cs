using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Util.Extension
{
    /// <summary>
    /// Extension for object.
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Get the query string with properties of the object.
        /// </summary>
        /// <param name="value">Generic object with properties to get the query string.</param>
        /// <returns>Query string</returns>
        /// <example>
        /// Foo class used in this example.
        /// <code>
        /// public class Foo
        /// {
        ///     public int IdFoo { get; set; }
        /// 
        ///     public string Name { get; set; }
        /// 
        ///     public string Code { get; set; }
        /// }
        /// </code>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     public async Task<![CDATA[<string>]]> ToQueryStringAsync()
        ///     {
        ///          var foo = new Foo
        ///          {
        ///              IdFoo = 1,
        ///              Name = "mkt App",
        ///              Code = "IBTH"
        ///          };
        /// 
        ///          return await foo.ToQueryStringAsync();
        ///     }
        /// }
        /// </code>
        /// The result in this sample is...
        /// <code>
        /// <![CDATA["IdFoo=1&Name=mkt+App&Code=IBTH"]]>
        /// </code>
        /// </example>
        public static async Task<string> ToQueryStringAsync(this object value)
        {
            if (value == null)
                return string.Empty;

            var json = JsonSerializer.Serialize(value);
            var jsonObject = JsonSerializer.Deserialize<object>(json);

            var enumerateObject = ((JsonElement)jsonObject).EnumerateObject();
            var genericDictionary = enumerateObject.ToGenericDictionary();

            if (genericDictionary == null)
                return string.Empty;

            return await new FormUrlEncodedContent(genericDictionary).ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get the query string with properties of the object.
        /// </summary>
        /// <param name="value">Generic object with properties to get the query string.</param>
        /// <returns>Query string</returns>
        /// <example>
        /// Foo class used in this example.
        /// <code>
        /// public class Foo
        /// {
        ///     public int IdFoo { get; set; }
        /// 
        ///     public string Name { get; set; }
        /// 
        ///     public string Code { get; set; }
        /// }
        /// </code>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     public string GetQueryString()
        ///     {
        ///          var foo = new Foo
        ///          {
        ///              IdFoo = 1,
        ///              Name = "mkt App",
        ///              Code = "IBTH"
        ///          };
        /// 
        ///          return foo.ToQueryString();
        ///     }
        /// }
        /// </code>
        /// The result in this sample is...
        /// <code>
        /// <![CDATA["IdFoo=1&Name=mkt+App&Code=IBTH"]]>
        /// </code>
        /// </example>
        public static string ToQueryString(this object value) => Task.Run(async () => await value.ToQueryStringAsync()).Result;

        /// <summary>
        /// Get the byte array.
        /// </summary>
        /// <param name="value">Generic object to convert in array of bytes.</param>
        /// <returns>Byte array.</returns>
        /// <example>
        /// Foo class used in this example.
        /// <code>
        /// public class Foo
        /// {
        ///     public int IdFoo { get; set; }
        /// 
        ///     public string Name { get; set; }
        /// 
        ///     public string Code { get; set; }
        /// }
        /// </code>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     public byte[] GetByteArray()
        ///     {
        ///          var foo = new Foo
        ///          {
        ///              IdFoo = 1,
        ///              Name = "mkt App",
        ///              Code = "IBTH"
        ///          };
        /// 
        ///          return foo.ToByteArray();
        ///     }
        /// }
        /// </code>
        /// </example>
        public static byte[] ToByteArray(this object value)
        {
            if (value == null)
                return null;

            return JsonSerializer.SerializeToUtf8Bytes(value);

        }

        private static IDictionary<string, string> ToGenericDictionary(this IEnumerable<JsonProperty> value)
        {
            if (value == null)
                return null;

            var genericDictionary = new Dictionary<string, string>();

            if (value.Any())
            {
                foreach (var jsonProperty in value)
                    if (jsonProperty.Value.ValueKind == JsonValueKind.Null)
                        continue;
                    else if (jsonProperty.Value.ValueKind == JsonValueKind.Object)
                        JsonPropertyObject(genericDictionary, jsonProperty);
                    else if (jsonProperty.Value.ValueKind == JsonValueKind.Array)
                        JsonPropertyArray(genericDictionary, jsonProperty);
                    else
                        genericDictionary.Add(jsonProperty.Name, jsonProperty.Value.ToString());

                return genericDictionary;
            }

            return null;
        }

        private static void JsonPropertyObject(Dictionary<string, string> genericDictionary, JsonProperty jsonProperty)
        {
            var genericDictionaryConcat = jsonProperty.Value.EnumerateObject().ToGenericDictionary();

            if (genericDictionaryConcat != null && genericDictionaryConcat.Any())
                foreach (var item in genericDictionaryConcat)
                    genericDictionary.Add($"{jsonProperty.Name}.{item.Key}", item.Value);
        }

        private static void JsonPropertyArray(Dictionary<string, string> genericDictionary, JsonProperty jsonProperty)
        {
            for (int i = 0; i < jsonProperty.Value.GetArrayLength(); i++)
            {
                var genericDictionaryConcat = ToGenericDictionary(jsonProperty.Value[i].EnumerateObject());

                if (genericDictionaryConcat != null && genericDictionaryConcat.Any())
                    foreach (var item in genericDictionaryConcat)
                        genericDictionary.Add($"{jsonProperty.Name}[{i}].{item.Key}", item.Value);
            }
        }
    }
}
