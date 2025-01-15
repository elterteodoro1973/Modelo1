using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Entidades.Validacoes.Utils
{
    public static class ValidadorCnpj
    {
        private static string RemoveNaoNumericos(string text)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
            string ret = reg.Replace(text, string.Empty);
            return ret;
        }
        public static bool CnpjValido(string cnpj)
        {
            cnpj = RemoveNaoNumericos(cnpj);
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            else if (cnpj == "".PadLeft(14, '0'))
                return false;
            else if (cnpj == "".PadLeft(14, '1'))
                return false;
            else if (cnpj == "".PadLeft(14, '2'))
                return false;
            else if (cnpj == "".PadLeft(14, '3'))
                return false;
            else if (cnpj == "".PadLeft(14, '4'))
                return false;
            else if (cnpj == "".PadLeft(14, '5'))
                return false;
            else if (cnpj == "".PadLeft(14, '6'))
                return false;
            else if (cnpj == "".PadLeft(14, '7'))
                return false;
            else if (cnpj == "".PadLeft(14, '8'))
                return false;
            else if (cnpj == "".PadLeft(14, '9'))
                return false;


            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
}
