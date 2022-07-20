using System.Net.Mail;

namespace Shared.Util.Common.Helpers
{
    /// <summary>
    /// Common class with helper methods for e-mail.
    /// </summary>
    public class EmailHelper
    {
        /// <summary>
        /// Verify if the e-mail string is valid.
        /// </summary>
        /// <param name="email">email on string format.</param>
        /// <returns>True if this e-mail string is valid, otherwise, false.</returns>
        /// <example>
        /// <code>
        /// var emailString = "teste@mkt.com.br";
        /// var validation = EmailHelper.IsValidCnpj(emailString);
        /// </code>
        /// The result is *true*
        /// </example>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
