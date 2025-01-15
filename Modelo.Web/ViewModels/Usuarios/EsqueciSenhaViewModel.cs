using System.ComponentModel.DataAnnotations;

namespace Modelo.Web.ViewModels.Usuarios
{
    public class EsqueciSenhaViewModel
    {
        [Display(Name ="E-mail")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(256, ErrorMessage = "E-mail possui tamanho máximo de 256 caracteres !")]
        [EmailAddress(ErrorMessage = "E-mail inválido !")]
        public string Email { get; set; } = null!;
    }
}
