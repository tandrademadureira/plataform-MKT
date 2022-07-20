using Shared.Util.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.Infra.Request
{
    [Serializable]
    public class BaseRequest
    {
        [JsonIgnore]
        public string CorrelationId => GetHeader(Headers.CorrelationId);

        /// <summary>
        /// Default request headers.
        /// </summary>
        /// <value>**content-type:** text/plain</value>
        [JsonIgnore]
        public IDictionary<string, string> DefaultRequestHeaders { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Add or update items in the header. 
        /// <para>If the value is null or empty, the item will not be added or updated in the header.</para>
        /// </summary>
        /// <param name="key">Key to identify value on header.</param>
        /// <param name="value">Value that represents the key on header.</param>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     public void SetHeader()
        ///     {
        ///         var request = new FooMessageRequest();
        ///         request.AddHeader(Headers.CorrelationId, Guid.NewGuid().ToString("N"));
        ///     }
        /// }
        /// </code>
        /// </example>
        public BaseRequest AddHeader(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !DefaultRequestHeaders.Contains(new KeyValuePair<string, string>(key, value)))
            {
                if (!DefaultRequestHeaders.ContainsKey(key))
                    DefaultRequestHeaders.Add(key, value);
                else
                    DefaultRequestHeaders[key] = value;
            }

            return this;
        }

        /// <summary>
        /// Get header value by the key.
        /// </summary>
        /// <param name="key">Key to identify value on header.</param>
        /// <returns>Value that represents the key on header.</returns>
        /// <example>
        /// Bar class used in this example.
        /// <code>
        /// public class Bar
        /// {
        ///     public void GetHeader()
        ///     {
        ///         var request = new FooMessageRequest();
        ///         var valueOfheader = request.GetHeader(Headers.CorrelationId);
        ///     }
        /// }
        /// </code>
        /// </example>
        protected string GetHeader(string key) => DefaultRequestHeaders.TryGetValue(key, out string outValue) ? outValue : string.Empty;


    }
}
