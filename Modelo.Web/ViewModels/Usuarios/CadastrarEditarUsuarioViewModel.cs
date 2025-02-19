using Modelo.Web.Configuracoes.Validacoes;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Web.ViewModels.Usuarios
{
    public class CadastrarEditarUsuarioViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(256, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string Nome { get; set; } = null!;
        
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "{0} Inválido !")]
        [CustomValidationCPF(ErrorMessage = "CPF inválido")]
        public string CPF { get; set; } = null!;

        
        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        public Guid PerfilId { get; set; } 

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        public string Email { get; set; } = null!;

        [Display(Name = "Ativo")]
        public bool UsuarioAtivo { get; set; } = true;
    }
}
