using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Entidades
{
    public class EntidadeBase
    {
        public Guid Id { get; set; }

        public bool Excluido { get; set; }
    }
}
