using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Modelo.Web.ViewModels.Usuarios
{
    public class CadastrarPerfilUsuarioViewModel
    {
        public string? NomeUsuario { get; set; }
        public string? Email { get; set; }
        public Guid? UsuarioId { get; set; }
        [Display(Name = "CBH ")]  
        public Guid? CbhId { get; set; }
        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        public Guid? PerfilId { get; set; }
        public string[]? Permissoes { get; set; }
    }
}
