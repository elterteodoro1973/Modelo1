using Modelo.Dominio.Entidades;

namespace Modelo.Web.ViewModels.Perfis
{
    public class LogPerfilPermissoesViewModel
    {        
        public string? Tipo { get; set; } = null!;
        public string? Valor { get; set; } = null!;
        public bool? Excluido { get; set; } = null!;
    }
}
