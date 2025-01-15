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
        public IList<string> Emails { get; set; } = null!;
        public IList<string> Telefone { get; set; } = null!;
        public Guid CBHPrincipal { get; set; }
        public IList<EnderecoCadastroUsuarioDTO> Enderecos { get; set; } = null!;
        public IList<Guid> CBHs { get; set; } = null!;
    }
}
