using System.Text.RegularExpressions;

namespace Modelo.Web.Configuracoes.Extensoes
{
    public static class StringExtensions
    {
        public static string ApenasNumeros(this string input)
        {
            input = input.Trim();
            string pattern = @"\D";
            string digitsOnly = Regex.Replace(input, pattern, "");
            return digitsOnly;
        }

        public static string FormatarCpfCnpj(this string input)
        {
            input = input.ApenasNumeros();

            if (input.Length == 14)
                input = Convert.ToUInt64(input).ToString(@"00\.000\.000\/0000\-00");
            else if (input.Length == 11)
                input = Convert.ToUInt64(input).ToString(@"000\.000\.000\-00");

            return input;
        }
    }
}
