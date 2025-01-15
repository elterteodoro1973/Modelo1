using Modelo.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Interfaces.Servicos
{
    public interface ILogServico
    {
        void InformarIdUsuario(Guid id);

        Guid? BuscarUsuarioId();
        
    }
}
