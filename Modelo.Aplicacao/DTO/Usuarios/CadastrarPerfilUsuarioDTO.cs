using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO.Usuarios
{
    public class CadastrarPerfilUsuarioDTO
    {
        public Guid UsuarioId { get; set; }       
        public Guid PerfilId { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
