using Modelo.Dominio.Entidades;

namespace Modelo.Dominio.Interfaces.Repositorios
{
    public interface IResetarSenhaRepositorio : IBaseRepositorio<ResetarSenha>
    {
        Task<ResetarSenha?> BuscarResetarSenhaPorId(Guid id);
        Task<ResetarSenha?> BuscarResetarSenhaPorToken(string token);
        Task<IList<ResetarSenha>> ListarResetarSenha();
    }
}
