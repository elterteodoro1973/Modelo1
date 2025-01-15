using Modelo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Infraestrutura.Crosscutting.Identity.Interfaces
{
    public interface IUsuarioServico
    {
        Task Adicionar(Usuario usuario, IList<EnderecoUsuario> enderecos, IList<TelefoneUsuario> telefones, IList<EmailUsuario> email, IList<Guid> CBHs);
        Task<Usuario?> Login(string email, string senha);
        Task Logout();
        Task Excluir(Guid id);
        Task Atualizar(Guid id, Usuario usuario, IList<EnderecoUsuario> enderecos, IList<TelefoneUsuario> telefones, IList<EmailUsuario> email, IList<Guid> CBHs);
        Task CadastrarSenha(Guid idUsuario, string senha, string confirmarSenha);
        Task TrocarCBHUsuarioLogado(Guid idUsuario, Guid cbhIdSelecionada);
    }
}
