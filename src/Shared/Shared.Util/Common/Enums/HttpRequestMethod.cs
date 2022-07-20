using System.ComponentModel;

namespace Shared.Util.Common.Enums
{
    /// <summary>
    /// Enumerator that represent HTTP request methods.
    /// </summary>
    public enum HttpRequestMethod
    {
        /// <summary>
        /// Represent a method GET of HTTP protocol. Name **Get**, description **Get** and code **1**.
        /// </summary>
        [Description("Get")] Get = 1,
        /// <summary>
        /// Represent a method POST of HTTP protocol. Name **Post**, description **Post** and code **2**.
        /// </summary>
        [Description("Post")] Post = 2,
        /// <summary>
        /// Represent a method PUT of HTTP protocol. Name **Put**, description **Put** and code **4**.
        /// </summary>
        [Description("Put")] Put = 4,
        /// <summary>
        /// Represent a method DELETE of HTTP protocol. Name **Delete**, description **delete** and code **8**.
        /// </summary>
        [Description("Delete")] Delete = 8,
        /// <summary>
        /// Represent a method HEAD of HTTP protocol. Name **Head**, description **Head** and code **16**.
        /// </summary>
        [Description("Head")] Head = 16,
        /// <summary>
        /// Represent a method PATCH of HTTP protocol. Name **Head**, description **Head** and code **32**.
        /// </summary>
        [Description("Patch")] Patch = 32,
        /// <summary>
        /// Represent a method OPTIONS of HTTP protocol. Name **Options**, description **Options** and code **64**.
        /// </summary>
        [Description("Options")] Options = 64
    }
}
