using Microsoft.EntityFrameworkCore;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;

namespace Modelo.Dados.Repositorios
{
    public class ResetarSenhaRepositorio : BaseRepositorio<ResetarSenha>, IResetarSenhaRepositorio
    {
        private readonly Contexto.DbContexto _contexto;
        private readonly ILogServico _logServico;
        public ResetarSenhaRepositorio(Contexto.DbContexto contexto, ILogServico logServico) : base(contexto, logServico)
        {
            _contexto = contexto;
            _logServico = logServico;
        }

        public async Task<ResetarSenha?> BuscarResetarSenhaPorId(Guid id)
        => await _contexto.ResetarSenha.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<IList<ResetarSenha>> ListarResetarSenha()
        => await _contexto.ResetarSenha.Where(c => !c.Excluido.Value).AsNoTracking().ToListAsync();
    }
}
