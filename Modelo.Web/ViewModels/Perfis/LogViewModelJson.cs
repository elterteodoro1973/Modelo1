using System;
using Modelo.Dominio.Entidades;

namespace Modelo.Web.ViewModels.Perfis
{
	public class LogViewModelJson
    {
		public LogViewModelJson()
		{
            data = new HashSet<LogPerfilJsonViewModel>();
        }

        public virtual ICollection<LogPerfilJsonViewModel>? data { get; set; }
    }
}

