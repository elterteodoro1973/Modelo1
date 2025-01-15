using System.ComponentModel.DataAnnotations;

namespace Modelo.Web.ViewModels.Perfis
{
    public class PerfilViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é obrigatório !")]
        [StringLength(256, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string Nome { get; set; } = null!;
        
        [Display(Name = "Descrição")]
        [StringLength(4000, ErrorMessage = "{0} possui tamanho máximo de 256 caracteres !")]
        public string? Descricao { get; set; }

        public bool? Excluido { get; set; }
        public string[]? Claims { get; set; }

        public bool? Administrador { get; set; }
    }
}
