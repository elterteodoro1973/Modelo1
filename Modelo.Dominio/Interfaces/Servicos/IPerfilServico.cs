using Modelo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Interfaces.Servicos
{
    public interface IPerfilServico
    {
        Task Adicionar(Perfil perfil);
        Task Editar(Perfil perfil);
        Task Excluir(Guid id);
    }
}
