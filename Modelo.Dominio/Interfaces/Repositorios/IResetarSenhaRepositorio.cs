using Modelo.Dominio.Entidades;

namespace Modelo.Dominio.Interfaces.Repositorios
{
    public interface IResetarSenhaRepositorio : IBaseRepositorio<ResetarSenha>
    {
        Task<ResetarSenha?> BuscarResetarSenhaPorId(Guid id);
        Task<IList<ResetarSenha>> ListarResetarSenha();
    }
}
