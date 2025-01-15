using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Entidades
{
    public class LogTransacoes
    {
        public Guid Id { get; set; }
        public Guid EntidadeId { get; set; }
        public DateTime Data { get; set; }
        public Guid? UsuarioId { get; set; }
        public string? Comando { get; set; }
        public string? CampoAlterado { get; set; }
        public string? ValorAnterior { get; set; }
        public string? ValorAtual { get; set; }
        public string? Dados { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
