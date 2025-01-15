using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.DTO.Usuarios
{
    public class EnderecoCadastroUsuarioDTO
    {
        public Guid? Id { get; set; }
        public string CEP { get; set; } = null!;
        public string Logradouro { get; set; } = null!;
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string Municipio { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
