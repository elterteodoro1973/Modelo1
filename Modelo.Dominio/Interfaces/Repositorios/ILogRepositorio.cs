using Modelo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Interfaces.Repositorios
{
    public interface ILogRepositorio
    {
        Task<IList<LogTransacoes>> BuscarPorIdEntidade(Guid idEntidade);
        Task<IList<LogTransacoes>?> BuscarPorIdsEntidades(IList<Guid> idsEntidades, DateTime? dataInicio, DateTime? dataFim);
    }
}
