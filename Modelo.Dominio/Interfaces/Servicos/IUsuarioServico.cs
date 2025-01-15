using Modelo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Interfaces.Servicos
{
    public interface IUsuarioServico
    {
        Task Login(string caminho, string email, string senha);
        Task Adicionar(string caminho, Usuario usuario);
        Task Editar(Usuario usuario);
        Task Excluir(Guid usuarioId);
        Task CadastrarSenha(Guid token, string email, string senha, string novaSenha);
        Task TrocarCBHUsuarioLogado(Guid usuarioId, Guid cbhIdSelecionada);
        
        Task CadastrarPerfilPermissao(Guid usuarioId, Guid cbhUsuarioId, Guid perfilId);
        void Logout();
        Task ResetarSenha(string caminho, string email);
        Task ValidarTokenEEmailResetarSenha(Guid token, string email);

    }
}
