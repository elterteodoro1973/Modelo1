using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO.Usuarios
{
    public class PerfilEPermissoesCBHUsuarioDTO
    {
        public Guid? PerfilId { get; set; }
        public IList<Claim>? Permissoes { get; set; }
    }
}
