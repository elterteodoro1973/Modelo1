namespace Modelo.Aplicacao.DTO.Usuarios
{
    public class UsuariosTelaInicialDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CPF { get; set; } = null!;

        public Dominio.Entidades.Perfis? Perfil { get; set; } = null!;
    }
}
