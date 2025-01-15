using Modelo.Dominio.Entidades;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Web.ViewModels.Perfis
{
    public class PerfilPermissoesViewModel
    {    
        public string? Nome { get; set; } = null;        
        public string? Descricao { get; set; } = null;

        public bool? Excluido { get; set; } = null;
        public List<LogPerfilPermissoesViewModel> PermissaoPerfis { get; set; } = null;

    }
}
