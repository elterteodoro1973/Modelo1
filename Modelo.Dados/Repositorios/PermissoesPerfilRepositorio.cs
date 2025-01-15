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
    public class PermissoesPerfilRepositorio : BaseRepositorio<PermissaoPerfil>, IPermissoesPerfilRepositorio
    {
        private readonly DAEEContexto _contexto;
        public PermissoesPerfilRepositorio(DAEEContexto contexto, ILogServico logServico) : base(contexto, logServico)
        {
            _contexto = contexto;
        }

        public async Task<IList<Guid>> BuscarIdsPermissoesPerfilPorPerfilId(Guid idPerfil)
        => await _contexto.PermissaoPerfils.IgnoreQueryFilters().Where(c => c.PerfilId == idPerfil).Select(c => c.Id).ToListAsync();

        public async Task<IList<Log>> BuscarLogsPorIdsPermissoes(IList<Guid> idsPermissoes)
        => await _contexto.Logs.Where(c => idsPermissoes.Contains(c.EntidadeId)).Include(c => c.Usuario).AsNoTracking().ToListAsync();

        public async Task<IList<PermissaoPerfil>?> BuscarPermissoesPorPerfil(Guid idPerfil)
        => await _contexto.PermissaoPerfils.Where(c => !c.Excluido && c.PerfilId == idPerfil). ToListAsync();
    }
}
