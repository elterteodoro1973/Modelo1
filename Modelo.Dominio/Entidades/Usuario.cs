
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Entidades
{
    public class Usuario : EntidadeBase
    {
        public Usuario()
        {
            
        }
        
        public string NomeCompleto { get; set; } = null!;
        public string CPF { get; set; } = null!;
        
        public string Email { get; set; } = null!;
       
        public string? Senha { get; set; }
      
        public bool Inativo { get; set; }
        public bool Administrador { get; set; }
       
        public virtual ICollection<LogTransacoes>? LogsSistema { get; set; }
       
    }
}
