using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO
{
    public class SelectOptionDTO
    {
        public Guid? Id { get; set; }
        public string Descricao { get; set; } = null!;
    }
}
