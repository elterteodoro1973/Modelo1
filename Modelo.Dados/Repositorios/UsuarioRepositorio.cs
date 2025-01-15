using Modelo.Dados.Contexto;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Dominio.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dados.Repositorios
{
    public class UsuarioRepositorio : BaseRepositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly DAEEContexto _contexto;
        private readonly ILogServico _logServico;
        public UsuarioRepositorio(DAEEContexto contexto, ILogServico logServico) : base(contexto, logServico)
        {
            _contexto = contexto;
            _logServico = logServico;
        }

        public async Task<IList<Usuario>> BuscarUsuariosSemRastreio()
        => await _contexto.Usuarios.Where(c => !c.Excluido).AsNoTracking().ToListAsync();

        public async Task<bool> EmailPrincipalJaCadastrado(string email, Guid? id)
        {
            return await _contexto.Usuarios.Where(c => !c.Excluido && c.Email == email.ToUpper() && (id.HasValue ? c.Id != id : true)).AnyAsync();
        }

        public async Task<Usuario?> BuscarUsuarioPorIdSemRastreio(Guid id)
        => await _contexto.Usuarios.Where(c => !c.Excluido && c.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<bool> IdUsuarioValido(Guid id)
        => await _contexto.Usuarios.Where(c => !c.Excluido && !c.Inativo && c.Id == id).AnyAsync();

        public async Task<Usuario?> BuscarUsuarioPorId(Guid id)
        => await _contexto.Usuarios.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<Usuario?> BuscarUsuarioPorIdParaEdicaoSemRastreio(Guid id)
        {
            var usuario = await _contexto.Usuarios.Where(c => !c.Excluido && c.Id == id).AsNoTracking().FirstOrDefaultAsync();

            if (usuario == null)
                return null;

          
            return usuario;
        }

        public async Task<Usuario?> BuscarPorEmail(string email)
        => await _contexto.Usuarios.Where(c => !c.Excluido && c.Email == email.ToUpper()).AsNoTracking().FirstOrDefaultAsync();

        public async Task<bool> EmailValidoLogin(string email)
        => await _contexto.Usuarios.Where(c => !c.Excluido && !c.Inativo && c.Email == email.ToUpper().Trim()).AnyAsync();

        public async Task<bool> SenhaValidaLogin(string email, string senha)
        => await _contexto.Usuarios.Where(c => !c.Excluido && !c.Inativo && c.Email == email.ToUpper().Trim() && senha == c.Senha).AnyAsync();

        public async Task<bool> UsuarioNaoPossuiSenhaCadastrada(string email)
        => await _contexto.Usuarios.Where(c => !c.Excluido && !c.Inativo && c.Email == email.ToUpper().Trim() ).AnyAsync();

        //public Task<Usuario?> BuscarPorTokenEEmailResetSenha(Guid token, string email)
        //=> (from h in _contexto.HistoricoSenhaUsuarios
        //    join u in _contexto.Usuarios on h.UsuarioId equals u.Id
        //    where !u.Excluido && !h.Excluido && !h.Inativo && h.Token == token && u.Email == email.ToUpper().Trim()
        //    select u).FirstOrDefaultAsync();

        public async Task<bool> VerficarSeUsuarioEAdmDAEE(Guid usuarioId)
        => await _contexto.Usuarios.Where(c => !c.Excluido && !c.Inativo && c.Id == usuarioId && c.Administrador).AnyAsync();

        public async Task<IList<LogTransacoes>?> BuscarLogPorUsuarioId(Guid usuarioId)
        => await _contexto.Logs.Include(c => c.Usuario).Where(c => c.EntidadeId == usuarioId).AsNoTracking().ToListAsync();
    }
}
