using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Servicos;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading.Tasks;

namespace Modelo.Dominio.Interfaces.Repositorios
{
    public interface IBaseRepositorio<T> where T: EntidadeBase
    {
        Task SalvarAlteracoes();
        Task IniciarTransacao();
        Task Commit();
        Task Roolback();
        Task Dispose();
        Task Adicionar(T entidade);
        Task Adicionar(T entidade, string entidadeNome);
        Task Adicionar(T entidade, string entidadeNome,Guid usuarioId);
        Task Atualizar(T entidade);
        Task Atualizar(T entidade,string entidadeNome);
        Task Atualizar(T entidade, string entidadeNome, Guid usuarioId);
        Task Excluir(T entidadeBase);
        Task<T> BuscarPorId(Guid Id, bool rastreioEntidade = true, bool incluirExcluidas = false);
        Task<IList<T>> BuscarTodos(bool rastreioEntidade = true, bool incluirExcluidas = false);
        Task<IList<LogTransacoes>?> BuscarLogsPorIds(IList<Guid> ids);

        Task<List<LogTransacoes>?> BuscarLogsPorIdsDatas(IList<Guid> ids, DateTime? DataInicial, DateTime? DataFinal);

        Task<List<LogTransacoes>?> BuscarLogsPorIdDatas(Guid id, DateTime? DataInicial, DateTime? DataFinal);
        Task<IList<LogTransacoes>?> BuscarLogsPorId(Guid id);
        Task AdicionarLista(IList<T> entidades);
        Task RemoverLista(IList<T> entidades);
        Task AtualizarLista(IList<T> entidades);
        Task<bool> VerficarSeIdEValido(Guid id);
    }
}
