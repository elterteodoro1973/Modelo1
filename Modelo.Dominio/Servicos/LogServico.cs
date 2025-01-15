using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Servicos
{
    public class LogServico : ILogServico
    {
        private Guid? _idUsuario;

        public LogServico()
        {

        }

        public Guid? BuscarUsuarioId()
        => _idUsuario;

        public void InformarIdUsuario(Guid id)
        {
            _idUsuario = id;
        }

    }
}
