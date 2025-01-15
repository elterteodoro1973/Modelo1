namespace Modelo.Web.ViewModels
{
    public class HistoricoViewModel
    {
        public Guid Id { get; set; }
        public string UrlConsulta { get; set; } = null!;
        public IList<LogViewModel> Logs { get; set; } = null;
    }
}
