using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Entidades
{
    public class PermissaoPerfil : EntidadeBase
    {
        public Guid PerfilId { get; set; }
        public string Tipo { get; set; } = null!;
        public string Valor { get; set; } = null!;

        public virtual Perfil Perfil { get; set; } = null!;
    }
}
