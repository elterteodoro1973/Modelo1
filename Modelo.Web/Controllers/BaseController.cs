using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Servicos;

namespace Modelo.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogServico _logServico;
        private readonly INotificador _notificador;
        public BaseController(INotificador notificador, ILogServico logServico)
        {
            _notificador = notificador;
            _logServico = logServico;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (TempData["Sucesso"] != null)
            {
                ViewBag.Sucesso = TempData["Sucesso"];
            }

            if (TempData["Erro"] != null && TempData["Erro"] is List<string>)
            {
                var listaErros = new List<string>();
                foreach (var item in TempData["Erro"] as List<string>)
                {
                    listaErros.Add(item);
                }
                ViewBag.ListaErros = listaErros;
            }

            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                InformarNomeUsuario();
                if (BuscarUsuarioIdLogado().HasValue)
                    _logServico.InformarIdUsuario(BuscarUsuarioIdLogado().Value);
            }

            base.OnActionExecuting(context);
        }

        public Guid? BuscarCBHSelecionada()
        {
            var cbhIdString = HttpContext.Session.GetString("CBHSelecionadaId");
            Guid idCBH;
            if (string.IsNullOrEmpty(cbhIdString))
                return null;
            if(Guid.TryParse(cbhIdString, out idCBH))
            {
                return idCBH;
            }
            return null;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (TempData["Sucesso"] != null)
            {
                ViewBag.Sucesso = TempData["Sucesso"];
            }

            if (TempData["Erro"] != null && TempData["Erro"] is List<string>)
            {
                var listaErros = new List<string>();
                foreach (var item in TempData["Erro"] as List<string>)
                {
                    listaErros.Add(item);
                }
                ViewBag.ListaErros = listaErros;
            }

            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                InformarNomeUsuario();
                if (BuscarUsuarioIdLogado().HasValue)
                    _logServico.InformarIdUsuario(BuscarUsuarioIdLogado().Value);
            }
            
            return base.OnActionExecutionAsync(context, next);
        }

        public Guid? BuscarUsuarioIdLogado()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                Guid idUsuario;
                if (Guid.TryParse(HttpContext.Session.GetString("UsuarioId"), out idUsuario))
                    return idUsuario;

            }
            return null;
        }
        
        protected bool OperacaoValida()
        {
            if (_notificador.TemNotificacao())
            {
                var listaErros = new List<string>();

                foreach (var item in _notificador.ObterNotificacoes())
                {
                    ModelState.AddModelError(string.Empty, item.Mensagem);
                    listaErros.Add(item.Mensagem);
                }
                ViewBag.ListaErros = listaErros;
            }
            return !_notificador.TemNotificacao();
        }

        public IList<string> RecuperarListaErros()
        {
            var listaErros = new List<string>();
            foreach (var item in ModelState.Values.SelectMany(c => c.Errors))
            {
                listaErros.Add(item.ErrorMessage);
            }
            return listaErros;
        }

        public void InformarNomeUsuario()
        {            
            var claimUsuarioId = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId");
            var claimNomeUsuario = User.Claims.FirstOrDefault(c => c.Type == "NomeUsuario");
            
            var options = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true };
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("NomeUsuario")) && claimNomeUsuario != null && claimUsuarioId != null)
            {
                HttpContext.Session.SetString("NomeUsuario", claimNomeUsuario.Value);
                HttpContext.Session.SetString("UsuarioId", claimUsuarioId.Value);
            }
        }
    }
}
