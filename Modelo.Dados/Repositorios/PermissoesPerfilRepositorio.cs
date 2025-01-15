using Modelo.Dados.Contexto;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dados.Repositorios
{
    public class PermissoesPerfilRepositorio : BaseRepositorio<PermissoesPerfis>, IPermissoesPerfilRepositorio
    {
        private readonly Contexto.DbContexto _contexto;
        public PermissoesPerfilRepositorio(Contexto.DbContexto contexto, ILogServico logServico) : base(contexto, logServico)
        {
            _contexto = contexto;
        }

        public async Task<IList<Guid>> BuscarIdsPermissoesPerfilPorPerfilId(Guid idPerfil)
        => await _contexto.PermissoesPerfis.IgnoreQueryFilters().Where(c => c.PerfilId == idPerfil).Select(c => c.Id).ToListAsync();

        public async Task<IList<LogTransacoes>> BuscarLogsPorIdsPermissoes(IList<Guid> idsPermissoes)
        => await _contexto.LogTransacoes.Where(c => idsPermissoes.Contains(c.EntidadeId)).Include(c => c.Usuario).AsNoTracking().ToListAsync();

        public async Task<IList<PermissoesPerfis>?> BuscarPermissoesPorPerfil(Guid idPerfil)
        => await _contexto.PermissoesPerfis.Where(c => !c.Excluido.Value && c.PerfilId == idPerfil). ToListAsync();
    }
}
