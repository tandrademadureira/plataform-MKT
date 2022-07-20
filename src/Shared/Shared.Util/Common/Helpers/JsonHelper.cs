using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Shared.Util.Common.Helpers
{
    /// <summary>
    /// Common class with helper methods for JSON.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Mask the sensitive data into a JSON.
        /// <para>
        /// A recursive query is executed inside the JSON string to fetch the sensitive properties that are within the list passed by parameter. After you find the properties, their values ​​are replaced by ***.
        /// <note type="note">If the JSON string is null, empty, or not a valid JSON object, the same JSON string is returned without masking the sensitive data.</note>
        /// <note type="warning">The value can only be replaced by *** if the field is of type string, otherwise an exception will be thrown.</note>
        /// </para>
        /// </summary>
        /// <param name="json">JSON on string format.</param>
        /// <param name="sensitiveProperties">List of string with the properties names.</param>
        /// <returns>JSON on string format with sensitive data masked.</returns>
        /// <example>
        /// Foo class used in this example.
        /// <code>
        /// public class Foo
        /// {
        ///     public string Name { get; set; }
        /// 
        ///     public string Cpf { get; set; }
        /// }
        /// </code>
        /// Bar class to use <see cref="JsonHelper"/> sample.
        /// <code>
        /// public class Bar
        /// {
        ///     public void MaskData()
        ///     {
        ///         var foo = new Foo { Cpf = "15486325787", Name = "Stephen William Hawking" };
        ///         var sensitiveProperties = new List<![CDATA[<string>]]> { "cpf" };
        ///         var jsonString = JsonConvert.SerializeObject(foo);
        /// 
        ///         var result = JsonHelper.MaskSensitiveData(jsonString, sensitiveProperties);
        ///     }
        /// }
        /// </code>
        /// Result value is...
        /// <code>
        /// {
        ///    "Cpf":"***",
        ///    "Name":"Stephen William Hawking"
        /// }
        /// </code>
        /// </example>
        public static string MaskSensitiveData(string json, List<string> sensitiveProperties)
        {
            if (!IsValidJson(json))
                return json;

            object deserializedValue = JsonSerializer.Deserialize<object>(json);

            if (!(deserializedValue is JsonElement))
                return json;

            return RecursiveSensitiveDataMask(((JsonElement)deserializedValue).EnumerateObject(), sensitiveProperties);
        }

        /// <summary>
        /// Verify if the JSON string is valid.
        /// </summary>
        /// <param name="json">JSON on string format.</param>
        /// <returns>True if this JSON string is valid, otherwise, false.</returns>
        /// <example>
        /// <code>
        /// var jsonString = @"{""Cpf"":""15486325787"", ""Name"":""Stephen William Hawking""}";
        /// 
        /// var validation = JsonHelper.IsValidJson(jsonString);
        /// </code>
        /// </example>
        public static bool IsValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json.StartsWith("---") || !((json.StartsWith("{") && json.EndsWith("}")) || (json.StartsWith("[") && json.EndsWith("]"))))
                return false;

            try
            {
                json = json.Trim();
                var obj = JsonDocument.Parse(json);
                return true;
            }
            catch { return false; }

        }

        private static string RecursiveSensitiveDataMask(IEnumerable<JsonProperty> enumerateObject, IEnumerable<string> sensitiveProperties)
        {
            if (!enumerateObject.Any())
                return string.Empty;

            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteStartObject();

                foreach (var jsonProperty in enumerateObject)
                {
                    if (jsonProperty.Value.ValueKind == JsonValueKind.Null)
                        continue;

                    if (jsonProperty.Value.ValueKind == JsonValueKind.Object)
                    {
                        writer.WritePropertyName(jsonProperty.Name);
                        RecursiveSensitiveDataMask(writer, jsonProperty.Value.EnumerateObject(), sensitiveProperties);
                    }
                    else
                    {
                        if (sensitiveProperties.Any(it => it.ToLower() == jsonProperty.Name.ToLower()))
                            writer.WriteString(jsonProperty.Name, "***");
                        else
                            jsonProperty.WriteTo(writer);
                    }
                }

                writer.WriteEndObject();
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        private static void RecursiveSensitiveDataMask(Utf8JsonWriter writer, IEnumerable<JsonProperty> enumerateObject, IEnumerable<string> sensitiveProperties)
        {
            if (!enumerateObject.Any())
                return;

            writer.WriteStartObject();

            foreach (var jsonProperty in enumerateObject)
            {
                if (jsonProperty.Value.ValueKind == JsonValueKind.Null)
                    continue;

                if (jsonProperty.Value.ValueKind == JsonValueKind.Object)
                {
                    writer.WritePropertyName(jsonProperty.Name);
                    RecursiveSensitiveDataMask(writer, jsonProperty.Value.EnumerateObject(), sensitiveProperties);
                }
                else
                {
                    if (sensitiveProperties.Any(it => it.ToLower() == jsonProperty.Name.ToLower()))
                        writer.WriteString(jsonProperty.Name, "***");
                    else
                        jsonProperty.WriteTo(writer);
                }
            }

            writer.WriteEndObject();
        }
    }
}
