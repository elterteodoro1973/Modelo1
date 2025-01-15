using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO.Perfis
{
    public class PerfilDTO
    {
        public Guid? Id { get; set; }
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; }
        public bool? Administrador { get; set; }
        public List<Claim>? Claims { get; set; }
    }
}
