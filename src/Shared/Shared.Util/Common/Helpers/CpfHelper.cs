namespace Shared.Util.Common.Helpers
{
    /// <summary>
    /// Common class with helper methods for CPF.
    /// </summary>
    public static class CpfHelper
    {
        /// <summary>
        /// Verify if the CPF string is valid.
        /// </summary>
        /// <param name="cpf">CPF on string format.</param>
        /// <returns>True if this CPF string is valid, otherwise, false.</returns>
        /// <example>
        /// <code>
        /// var cpfString = "461.312.875-70";
        /// var validation = CpfHelper.IsValidCpf(cpfString);
        /// </code>
        /// The result is *true*
        /// </example>
        public static bool IsValidCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        /// <summary>
        /// Format the CPF string as ###.###.###-##.
        /// </summary>
        /// <param name="cpf">CPF on string.</param>
        /// <returns>If this CPF string is valid return formatted, otherwise, return current value.</returns>
        /// <example>
        /// <code>
        /// var cpfString = "46131287570";
        /// var formatCpf = CpfHelper.FormatCpf(cpfString);
        /// </code>
        /// The result is *461.312.875-70*
        /// </example>
        public static string FormatCpf(string cpf)
        {
            if (!IsValidCpf(cpf))
                return cpf;

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
        }
    }
}
