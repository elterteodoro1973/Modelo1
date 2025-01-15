using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO
{
    public class LogTransacoesDTO
    {
        public DateTime Data { get; set; }
        public string Usuario { get; set; } = null!;
        public string Campo { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string Valor { get; set; } = null!;
        public string ValorAnterior { get; set; } = null!;
        public Guid EntidadeId { get; set; }
        public string Dados { get; set; } = null!;
        public string Comando { get; set; } = null!;
        public Guid? UsuarioId { get; set; }
    }
}
