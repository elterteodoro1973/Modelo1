using System.ComponentModel.DataAnnotations;

namespace Modelo.Web.ViewModels.Usuarios
{
    public class EnderecoViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "CEP")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "{0} Inválido !")]
        public string CEP { get; set; } = null!;
        [Display(Name = "Logradouro")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [MaxLength(256, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string Logradouro { get; set; } = null!;
        [Display(Name = "Número")]
        public string? Numero { get; set; }
        [Display(Name = "Complemento")]
        [MaxLength(256, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string? Complemento { get; set; }
        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        public string Municipio { get; set; } = null!;
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        public string Estado { get; set; } = null!;
    }
}
