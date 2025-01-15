namespace Modelo.Web.ViewModels
{
    public class LogViewModel
    {
        public DateTime Data { get; set; }
        public string Usuario { get; set; } = null!;
        public string Campo { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string Valor { get; set; } = null!;
        public string ValorAnterior { get; set; } = null!;
        public string Dados { get; set; } = null!;
        public string DataFormatada { get; set; }
        public Guid? UsuarioId { get; set; }
    }
}
