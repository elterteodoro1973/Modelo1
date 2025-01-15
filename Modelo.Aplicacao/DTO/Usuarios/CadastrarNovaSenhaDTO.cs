using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO.Usuarios
{
    public class CadastrarNovaSenhaDTO
    {
        public string? Email { get; set; }
        public Guid Token { get; set; }
        public string Senha { get; set; } = null!;
        public string ConfirmarSenha { get; set; } = null!;
    }
}
