using System.ComponentModel.DataAnnotations;

namespace Modelo.Dominio.Entidades
{
    public class EntidadeBase
    {
        [Key]
        public Guid Id { get; set; }
        public bool? Excluido { get; set; } = false;
    }
}
