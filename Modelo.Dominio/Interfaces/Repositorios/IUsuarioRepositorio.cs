using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Servicos;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Interfaces.Repositorios
{
    public interface IUsuarioRepositorio : IBaseRepositorio<Usuarios>
    {
        Task<IList<Usuarios>> BuscarUsuariosSemRastreio();
        Task<Usuarios?> BuscarUsuarioPorIdSemRastreio(Guid id);
        Task<Usuarios?> BuscarUsuarioPorIdParaEdicaoSemRastreio(Guid id);
        Task<Usuarios?> BuscarUsuarioPorId(Guid id);
        Task<Usuarios?> BuscarPorEmail(string email);
        Task<bool> EmailValidoLogin(string email);
        Task<bool> UsuarioNaoPossuiSenhaCadastrada(string email);
        Task<bool> SenhaValidaLogin(string email, string senha);
        Task<bool> EmailPrincipalJaCadastrado(string email, Guid? id);
        Task<bool> IdUsuarioValido(Guid id);
        Task<bool> VerficarSeUsuarioEAdmDAEE(Guid usuarioId);
        //Task<Usuarios?> BuscarPorTokenEEmailResetSenha(Guid token, string email);
        Task<IList<LogTransacoes>?> BuscarLogPorUsuarioId(Guid usuarioId);
    }
}
