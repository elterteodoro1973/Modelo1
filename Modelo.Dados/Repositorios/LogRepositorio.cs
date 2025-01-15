using Modelo.Dados.Contexto;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dados.Repositorios
{
    public class LogRepositorio : ILogRepositorio
    {
        private readonly Contexto.DbContexto _contexto;

        public LogRepositorio(Contexto.DbContexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<IList<LogTransacoes>> BuscarPorIdEntidade(Guid idEntidade)
        => await _contexto.LogTransacoes.Where(c => c.EntidadeId == idEntidade && c.Comando != "DELETE").Include(c => c.Usuario).AsNoTracking().ToListAsync();

        public async Task<IList<LogTransacoes>?> BuscarPorIdsEntidades(IList<Guid> idsEntidades, DateTime? dataInicio, DateTime? dataFim)
        => await _contexto.LogTransacoes.Where(c => idsEntidades.Contains(c.EntidadeId) && (dataInicio.HasValue ? c.Data.Date >= dataInicio.Value.Date : true) && (dataFim.HasValue ? c.Data.Date <= dataFim.Value.Date : true)).Include(c => c.Usuario).AsNoTracking().ToListAsync();
    }
}
