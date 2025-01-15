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
    public class PerfilRepositorio : BaseRepositorio<Perfil>, IPerfilRepositorio
    {
        private readonly DAEEContexto _contexto;
        public PerfilRepositorio(DAEEContexto contexto, ILogServico logServico) : base(contexto, logServico)
        {
            _contexto = contexto;
        }

        public async Task<IList<Perfil>> BuscarPerfis()
        => await _contexto.Perfis.Where(c => !c.Excluido).AsNoTracking().ToListAsync();

        public async Task<Perfil?> BuscarPorIdParaEdicao(Guid id)
        => await _contexto.Perfis.Where(c => !c.Excluido && c.Id == id).Include(c => c.PermissaoPerfis).AsNoTracking().FirstOrDefaultAsync();

        public async Task<Perfil?> BuscarPerfilAdministrador()
        {
            return await _contexto.Perfis.Where(c => c.Administrador == true).FirstOrDefaultAsync();
        }
        public async Task<IList<Log>> BuscarTodosEntidadeId(Guid? entidadeId)
        {
            return await _contexto.Logs.Where(c => c.EntidadeId == entidadeId).ToListAsync();
        }

        //public async Task<bool> PerfilPossuiUsuariosAssociados(Guid id)
        //=> await _contexto.CBHUsuarios.Where(c => !c.Excluido && c.PerfilId == id).AnyAsync();

        public async Task<bool> VerificarSeIdEPerfilAdmDAEE(Guid id)
        => await _contexto.Perfis.Where(c => !c.Excluido && c.Nome.ToUpper().Trim() == "ADMINISTRADOR DAEE").AnyAsync();
    }
}
