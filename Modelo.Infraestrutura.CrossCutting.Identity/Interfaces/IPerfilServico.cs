using Modelo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Infraestrutura.Crosscutting.Identity.Interfaces
{
    public interface IPerfilServico 
    {
        Task Adicionar(Perfil perfil, IList<Claim> permissoes);
        Task AdicionarPerfilUsuario(Guid perfilId, Guid usuarioId, Guid cbhId);
    }
}
