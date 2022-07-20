using System.Collections.Generic;

namespace Shared.Api.HealthCheck
{
    /// <summary>
    /// Class used to load variables used for process HealthCheck. Use the Option design pattern
    /// </summary>
    public class HealthCheckOption
    {
        /// <summary>
        /// Property InternalIntegrations
        /// </summary>
        public List<Integration> InternalIntegrations { get; set; }

        /// <summary>
        /// Property ExternalIntegrations
        /// </summary>
        public List<Integration> ExternalIntegrations { get; set; }
    }
}
