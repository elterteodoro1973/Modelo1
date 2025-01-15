namespace Modelo.Web.ViewModels.Usuarios
{
    public class UsuariosViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CPF { get; set; } = null!;
    }
}
