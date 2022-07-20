namespace Shared.Util.Common.Constants
{
    /// <summary>
    /// Constant of headers for use on APIs.
    /// </summary>
    public class Headers
    {
        /// <summary>
        /// CorrelationId of trace.
        /// </summary>
        /// <value>CorrelationId</value>
        public static string CorrelationId => "Correlation-Id";

        /// <summary>
        /// Key to authorize request on Microservices, Domais or Enterprise Service Bus
        /// </summary>
        /// <value>Authorization-Token</value>
        public static string AuthorizationToken => "Authorization-Token";

        /// <summary>
        /// Represents the IP identified on the origin.
        /// </summary>
        /// <value>Origin-Ip</value>
        public static string OriginIp => "Origin-Ip";

        /// <summary>
        /// Represents the device identified on the origin.
        /// </summary>
        /// <value>Origin-Device</value>
        public static string OriginDevice => "Origin-Device";

        /// <summary>
        /// Represents the application identified on the origin.
        /// </summary>
        /// <value>Origin-Application</value>
        public static string OriginApplication => "Origin-Application";

        /// <summary>
        /// Represents the action on SAGA Pattern.
        /// </summary>
        /// <value>Saga-Action</value>
        public static string SagaAction => "Saga-Action";

        /// <summary>
        /// Represents the Bearer token.
        /// </summary>
        /// <value>Authorization</value>
        public static string Authorization => "Authorization";

        /// <summary>
        /// Represents social login.
        /// </summary>
        /// <value>Social-Login</value>
        public static string SocialLogin => "Social-Login";
    }
}
