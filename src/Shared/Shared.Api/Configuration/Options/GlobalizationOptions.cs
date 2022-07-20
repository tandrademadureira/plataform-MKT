namespace Shared.Api.Configuration.Options
{
    /// <summary>
    /// Options of Globalization with required properties.
    /// </summary>
    public class GlobalizationOptions
    {
        /// <summary>
        /// Defines wich culture is principal in a request event.
        /// </summary>
        /// <value>pt-BR</value>
        public string DefaultRequestCulture { get; set; }

        /// <summary>
        /// Array with cultures that works with application.
        /// </summary>
        /// <value>"en-US", "pt-BR"</value>
        public string[] SupportedCultures { get; set; }

        /// <summary>
        /// Array with cultures that works with application.
        /// </summary>
        /// <value>"en-US", "pt-BR"</value>
        public string[] SupportedUICultures { get; set; }

        /// <summary>
        /// Specifier of how decimal value will be showed.
        /// </summary>
        /// <value>"F" (only dot to decimal places), "P" (% at the end), "C" ($ format), "N" (uses ',' and '.' to separate) </value>
        public string FormatDecimal { get; set; }

        /// <summary>
        /// Number of decimal places.
        /// </summary>
        /// <value>2</value>
        public int DecimalPlaces { get; set; }

        /// <summary>
        /// Date Format default that will works using Service Client.
        /// </summary>
        /// <value>"dd/MM/yyyy HH:mm:ss", "MM/dd/yyyy hh:mm:ss a", "yyyy-MM-dd'T'HH:mm:ssZZZZ"</value>
        public string DateFormat { get; set; }
    }
}
