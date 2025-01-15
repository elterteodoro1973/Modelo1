using Modelo.Aplicacao.DTO;
using Modelo.Aplicacao.DTO.Perfis;
using Modelo.Aplicacao.DTO.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.Interfaces
{
    public interface IPerfilAppServico
    {
        Task Adicionar(PerfilDTO dto);
        Task Editar(PerfilDTO dto);
        Task<PerfilDTO?> BuscarPorId(Guid id);
        Task<IList<PerfilDTO?>> BuscarPerfis();
        Task<PerfilDTO> BuscarPerfilAdministrador();

        Task<IList<LogDTO>> BuscarLogPerfilPorId(Guid? idPerfil);
        Task Excluir(Guid id);

        Task<IList<LogDTO>> BuscarLogs(Guid id, string? filtro);
    }
}
