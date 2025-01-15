using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.DTO
{
    public class EmailConfiguracao
    {
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public string EnderecoSMTP { get; set; } = null!;
        public string BaseURL { get; set; } = null!;
        public int Porta { get; set; }
        public bool UsarSSL { get; set; }
    }
}
