using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO.Usuarios
{
    public class CadastrarEditarUsuarioDTO
    {
        public Guid? Id { get; set; }
        public string Nome { get; set; } = null!;
        public string CPF { get; set; } = null!;
        public bool? UsuarioAtivo { get; set; }       
       
    }
}
