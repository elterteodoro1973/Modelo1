using System.ComponentModel.DataAnnotations;

namespace Modelo.Web.ViewModels.Usuarios
{
    public class CadastrarNovaSenhaViewModel
    {
        public string? Email { get; set; }
        public Guid Token { get; set; }
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [MinLength(6, ErrorMessage = "{0} possui tamanho minimo de 6 caracteres !")]
        [MaxLength(30, ErrorMessage = "{0} Posui tamanho máximo de 30 caracteres !")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = null!;
        [Display(Name = "Confirmar Senha")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [MinLength(6, ErrorMessage = "{0} possui tamanho minimo de 6 caracteres !")]
        [MaxLength(30, ErrorMessage = "{0} Posui tamanho máximo de 30 caracteres !")]
        [DataType(DataType.Password)]
        [Compare(nameof(Senha), ErrorMessage = "Senhas diferentes !")]
        public string ConfirmarSenha { get; set; } = null!;
    }
}
