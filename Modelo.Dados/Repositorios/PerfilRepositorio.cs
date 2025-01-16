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
    public class PerfilRepositorio : BaseRepositorio<Perfis>, IPerfilRepositorio
    {
        private readonly Contexto.DbContexto _contexto;
        public PerfilRepositorio(Contexto.DbContexto contexto, ILogServico logServico) : base(contexto, logServico)
        {
            _contexto = contexto;
        }

        public async Task<IList<Perfis>> BuscarPerfis()
        => await _contexto.Perfis.Where(c => !c.Excluido.Value).AsNoTracking().ToListAsync();

        public async Task<Perfis?> BuscarPorIdParaEdicao(Guid id)
        => await _contexto.Perfis.Where(c => !c.Excluido.Value && c.Id == id).Include(c => c.PermissoesPerfis).AsNoTracking().FirstOrDefaultAsync();

        public async Task<Perfis?> BuscarPerfilAdministrador()
        {
            return await _contexto.Perfis.Where(c => c.Administrador == true).FirstOrDefaultAsync();
        }
        public async Task<IList<LogTransacoes>> BuscarTodosEntidadeId(Guid? entidadeId)
        {
            return await _contexto.LogTransacoes.Where(c => c.EntidadeId == entidadeId).ToListAsync();
        }

        //public async Task<bool> PerfilPossuiUsuariosAssociados(Guid id)
        //=> await _contexto.CBHUsuarios.Where(c => !c.Excluido && c.PerfilId == id).AnyAsync();

        public async Task<bool> VerificarSeIdEPerfilAdmDAEE(Guid id)
        => await _contexto.Perfis.Where(c => !c.Excluido.Value && c.Nome.ToUpper().Trim() == "ADMINISTRADOR").AnyAsync();
    }
}
