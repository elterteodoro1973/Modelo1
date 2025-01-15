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
        Task Adicionar(Perfis perfil);
        Task Editar(Perfis perfil);
        Task Excluir(Guid id);
    }
}
