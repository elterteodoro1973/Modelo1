using Modelo.Dominio.Entidades;

namespace Modelo.Dominio.Interfaces.Servicos
{
    public interface IResetarSenhaServico
    {    
        Task Adicionar(ResetarSenha resetarSenha);        
        Task Excluir(Guid usuarioId);
    }
}
