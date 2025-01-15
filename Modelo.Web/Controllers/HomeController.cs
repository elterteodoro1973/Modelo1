using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Modelo.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, INotificador notificador, ILogServico logServico) : base(notificador, logServico)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //return RedirectToAction("Index", "Empreendimento");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}