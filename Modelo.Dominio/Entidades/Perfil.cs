using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Entidades
{
    public class Perfil : EntidadeBase
    {
        public Perfil()
        {
            PermissaoPerfis = new HashSet<PermissaoPerfil>();            
        }
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; }
        public bool? Administrador { get; set; }
        public virtual ICollection<PermissaoPerfil> PermissaoPerfis { get; set; }
        
    }
}
