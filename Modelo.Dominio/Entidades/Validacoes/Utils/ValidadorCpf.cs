using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Entidades.Validacoes.Utils
{
    public static class ValidadorCpf
    {
        private static string RemoveNaoNumericos(string text)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
            string ret = reg.Replace(text, string.Empty);
            return ret;
        }

        public static bool CpfValido(string cpf)
        {
            cpf = RemoveNaoNumericos(cpf);

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;
            else if (cpf == "".PadLeft(11, '0'))
                return false;
            else if (cpf == "".PadLeft(11, '1'))
                return false;
            else if (cpf == "".PadLeft(11, '2'))
                return false;
            else if (cpf == "".PadLeft(11, '3'))
                return false;
            else if (cpf == "".PadLeft(11, '4'))
                return false;
            else if (cpf == "".PadLeft(11, '5'))
                return false;
            else if (cpf == "".PadLeft(11, '6'))
                return false;
            else if (cpf == "".PadLeft(11, '7'))
                return false;
            else if (cpf == "".PadLeft(11, '8'))
                return false;
            else if (cpf == "".PadLeft(11, '9'))
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);

        }
    }

}
