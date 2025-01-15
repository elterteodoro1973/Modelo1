using Modelo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Interfaces.Repositorios
{
    public interface IPerfilRepositorio : IBaseRepositorio<Perfis>
    {
        Task<IList<Perfis>> BuscarPerfis();
        Task<Perfis?> BuscarPorIdParaEdicao(Guid id);
        Task<Perfis?> BuscarPerfilAdministrador();
        Task<IList<LogTransacoes>> BuscarTodosEntidadeId(Guid? idPerfil);
        //Task<bool> PerfilPossuiUsuariosAssociados(Guid id);
        Task<bool> VerificarSeIdEPerfilAdmDAEE(Guid id);
        
    }
}
