using System.ComponentModel.DataAnnotations;

namespace Modelo.Web.ViewModels.Usuarios
{
    public class LoginViewModel
    {
        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        public string Usuario { get; set; } = null!;
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = null!;
    }
}
