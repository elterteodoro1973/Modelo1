using Modelo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Interfaces.Repositorios
{
    public interface IPermissoesPerfilRepositorio : IBaseRepositorio<PermissoesPerfis>
    {
        Task<IList<PermissoesPerfis>?> BuscarPermissoesPorPerfil(Guid idPerfil);

        Task<IList<Guid>> BuscarIdsPermissoesPerfilPorPerfilId(Guid idPerfil);

        Task<IList<LogTransacoes>> BuscarLogsPorIdsPermissoes(IList<Guid> idsPermissoes);
    }
}
