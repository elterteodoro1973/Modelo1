namespace Modelo.Web.ViewModels.Perfis
{
    public class LogPerfilJsonViewModel
    {
        public LogPerfilJsonViewModel()
        {
            Detalhes = new HashSet<LogViewModelDetalhesJson>();
        }

        public string Data { get; set; }
        public string Campo { get; set; }
        public string Acoes { get; set; }
        public string Usuario { get; set; }
        public string Dados { get; set; }

        
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public string Situacao { get; set; }
        public bool Excluido { get; set; }

        public string Permissoes { get; set; }

        public virtual LogViewModelJson Log { get; set; }
        public virtual ICollection<LogViewModelDetalhesJson>? Detalhes { get; set; }
    }
}

