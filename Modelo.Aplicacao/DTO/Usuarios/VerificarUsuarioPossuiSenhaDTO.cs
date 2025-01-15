using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO.Usuarios
{
    public class VerificarUsuarioPossuiSenhaDTO
    {
        public bool UsuarioInvalido { get; set; } = true;
        public bool SenhaCadastrada { get; set; } = false;
    }
}
