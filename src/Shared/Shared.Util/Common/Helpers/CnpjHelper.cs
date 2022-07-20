namespace Shared.Util.Common.Helpers
{
    /// <summary>
    /// Common class with helper methods for CNPJ.
    /// </summary>
    public static class CnpjHelper
    {
        /// <summary>
        /// Verify if the CNPJ string is valid.
        /// </summary>
        /// <param name="cnpj">CNPJ on string format.</param>
        /// <returns>True if this CNPJ string is valid, otherwise, false.</returns>
        /// <example>
        /// <code>
        /// var cnpjString = "82.876.360/0001-16";
        /// var validation = CnpjHelper.IsValidCnpj(cnpjString);
        /// </code>
        /// The result is *true*
        /// </example>
        public static bool IsValidCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cnpj.EndsWith(digito);
        }


        /// <summary>
        /// Format the CNPJ string as ##.###.###/####-###.
        /// </summary>
        /// <param name="cnpj">CNPJ on string.</param>
        /// <returns>If this CNPJ string is valid return formatted, otherwise, return current value.</returns>
        /// <example>
        /// <code>
        /// var cnpjString = "82876360000116";
        /// var validation = CnpjHelper.FormatCnpj(cnpjString);
        /// </code>
        /// The result is *82.876.360/0001-16*
        /// </example>
        public static string FormatCnpj(string cnpj)
        {
            if (!IsValidCnpj(cnpj))
                return cnpj;

            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "");

            return $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12, 2)}";
        }
    }
}
