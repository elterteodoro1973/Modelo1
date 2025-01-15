using Modelo.Aplicacao.DTO;
using Modelo.Aplicacao.DTO.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.Interfaces
{
    public interface IUsuarioAppServico
    {
        Task<IList<UsuariosTelaInicialDTO>> ListarUsuariosTelaInicial(string? filtro);
        Task Logout();
        Task Login(string caminho, string email, string senha);
        Task Cadastrar(string caminho, CadastrarEditarUsuarioDTO dto);
        Task Editar(CadastrarEditarUsuarioDTO dto);
        Task CadastrarNovaSenha( CadastrarNovaSenhaDTO dto);
        Task CadastrarPermissoesEPerfilUsuario(CadastrarPerfilUsuarioDTO dto);
        Task<CadastrarEditarUsuarioDTO?> BuscarUsuarioParaEditarPorId(Guid id);
        Task<UsuariosTelaInicialDTO?> BuscarUsuarioTelaCadastrarNovaSenha(Guid idUsuario);
        Task<IList<LogTransacoesDTO>?> BuscarLogUsuario(Guid idUsuario, string? filtro);
        Task Excluir(Guid id);

        Task ResetarSenha(string caminho, string email);
        Task TrocarCBHUsuarioLogado(Guid usuarioId, Guid cbhIdSelecionada);
        Task<UsuariosTelaInicialDTO?> BuscarUsuarioPorId(Guid idUsuario);
        

        Task ValidarTokenEmailCadastrarNovaSenha(Guid token, string email);
    }
}
