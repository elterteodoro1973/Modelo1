namespace Modelo.Web.ViewModels.Perfis
{
    public class LogPerfilViewModel
    {
        public Guid EntidadeId { get; set; }
        public DateTime Data { get; set; }
        public Guid? UsuarioId { get; set; }
        public string Dados { get; set; }

        public string Campo { get; set; }
        public string Tipo { get; set; }
        public string Valor { get; set; } = null!;
        public string Acoes { get; set; } = null!;
    }
}
