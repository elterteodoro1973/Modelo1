using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Modelo.Aplicacao.DTO.Perfis;
using Modelo.Aplicacao.Interfaces;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Web.Configuracoes.Claims;
using Modelo.Web.ViewModels.Perfis;
using Newtonsoft.Json;

namespace Modelo.Web.Controllers
{
    public class PerfisController : BaseController
    {
        private readonly INotificador _notificador;
        private readonly IPerfilAppServico _perfilAppServico;
        private readonly IMapper _mapper;
        private readonly IUsuarioAppServico _usuarioAppServico;
        public PerfisController(INotificador notificador, ILogServico logServico,
             IPerfilAppServico perfilAppServico,
             IUsuarioAppServico usuarioAppServico,
             IMapper mapper) : base(notificador, logServico)
        {
            _perfilAppServico = perfilAppServico;
            _notificador = notificador;
            _mapper = mapper;
            _usuarioAppServico = usuarioAppServico;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Grid", _mapper.Map<IList<PerfilViewModel>>(await _perfilAppServico.BuscarPerfis()));
            }

            return View();
        }

        public IActionResult Adicionar()
        {
            var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
            ViewBag.Categorias = claims.GroupBy(c => c.Item1).Select(g => g.First()).Select(c => c.Item1).OrderBy(c => c).ToList();
            ViewBag.ClaimModulos = claims;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody]PerfilViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
                var claimnsSelecionadas = claims.Where(c => model.Claims.Contains(c.Item3)).Select(c => c.Item4).ToList();
                var dto = _mapper.Map<PerfilDTO>(model);
                dto.Claims = claimnsSelecionadas;
                await _perfilAppServico.Adicionar(dto);

                if (!OperacaoValida())
                    return BadRequest(RecuperarListaErros());

                return Ok("Perfil Cadastrado com sucesso !");
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao adicionar o perfil !" + ex.Message);
            }
           
        }

        public async Task<IActionResult> Editar(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty)
                return BadRequest();

            var perfil = await _perfilAppServico.BuscarPorId(id.Value);

            if (perfil == null)
                return NotFound();

            var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
            ViewBag.Categorias = claims.GroupBy(c => c.Item1).Select(g => g.First()).Select(c => c.Item1).OrderBy(c => c).ToList();
            ViewBag.ClaimModulos = claims;
            var model = _mapper.Map<PerfilViewModel>(perfil);
            model.Claims = claims.IntersectBy(perfil.Claims.Select(c => new { c.Type, c.Value }), c => new { c.Item4.Type, c.Item4.Value }).Select(c => c.Item3).ToArray();
            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar([FromBody] PerfilViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
                var claimnsSelecionadas = claims.Where(c => model.Claims.Contains(c.Item3)).Select(c => c.Item4).ToList();
                var dto = _mapper.Map<PerfilDTO>(model);
                dto.Claims = claimnsSelecionadas;
                await _perfilAppServico.Editar(dto);

                if (!OperacaoValida())
                    return BadRequest(RecuperarListaErros());

                return Ok("Perfil Alterado com sucesso !");
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao alterar o perfil !" + ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Excluir(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            try
            {
                await _perfilAppServico.Excluir(id);
                if (!OperacaoValida())
                    return BadRequest(RecuperarListaErros());

                return Ok("Perfil Excluido com sucesso !");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao excluir o perfil !" + ex.Message);
            }

        }

        public async Task<IActionResult> BuscarPermissoesPerfil(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Identificador inválido !");

            try
            {
                var perfil = await _perfilAppServico.BuscarPorId(id);

                if (perfil == null)
                    return NotFound("Perfil não encontrado !");

                var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
                var permissoes = claims.IntersectBy(perfil.Claims.Select(c => new { c.Type, c.Value }), c => new { c.Item4.Type, c.Item4.Value }).Select(c => c.Item3).ToArray();
                return Ok(permissoes.ToList());


            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar as permissões do perfil !" + ex.Message);
            }
        }

        //public async Task<IActionResult> Historico(Guid id, string? filtro)
        //{
        //    if (id == Guid.Empty)
        //        return BadRequest();

        //    var perfil = await _perfilAppServico.BuscarPorId(id);

        //    if (perfil == null)
        //        return NotFound();

        //    var model = new HistoricoViewModel
        //    {
        //        Id = id,
        //        UrlConsulta = Url.Action("Historico","Perfis", new { id = id})
        //    };

        //    if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //    {
        //        model.Logs = _mapper.Map<IList<LogViewModel>>(await _perfilAppServico.BuscarLogs(id, filtro));
        //        return PartialView("_GridHistorico", model);
        //    }


        //    return View(model);
        //}


        public async Task<IActionResult> Historico(Guid? id)
        {
            try
            {
                if (!id.HasValue)
                    return NotFound();

                var Perfis = await _perfilAppServico.BuscarLogPerfilPorId(id.Value);

                if (Perfis == null)
                    return BadRequest();

                var historico = _mapper.Map<IList<LogPerfilViewModel>>(Perfis);

                List<LogPerfilJsonViewModel> list = new List<LogPerfilJsonViewModel>();

                foreach (var item in historico)
                {
                    var usuario = await _usuarioAppServico.BuscarUsuarioPorId(item.UsuarioId.Value);

                    LogViewModelDetalhesJson detalhes = JsonConvert.DeserializeObject<LogViewModelDetalhesJson>(item.Dados);


                    var pPermissaoPerfis = "";


                    try
                    {
                        PerfilPermissoesViewModel detalhes2 = JsonConvert.DeserializeObject<PerfilPermissoesViewModel>(item.Dados);

                        

                        if (detalhes2.PermissaoPerfis != null)
                        {
                            if (detalhes2.PermissaoPerfis.Count() > 0)
                            {
                                List<LogPerfilPermissoesViewModel> permissaoPerfis = detalhes2.PermissaoPerfis;

                                permissaoPerfis = permissaoPerfis.OrderBy(c=>c.Tipo).ThenBy(c=>c.Valor).ToList();
                                

                                List<LogPerfilPermissoesViewModel> distinctpermissaoPerfis = permissaoPerfis
                                  .GroupBy(p => new { p.Tipo, p.Valor,p.Excluido })
                                  .Select(g => g.First())
                                  .ToList();


                                pPermissaoPerfis = $@"Permissões:&nbsp;&nbsp;
                                              <table cellspacing='0' border='1' style='font-size: 100%;'>
	                                            <tr>
		                                            <td style='background-color: #C0C0C0;'><strong>Tipo</strong></td>
		                                            <td style='background-color: #C0C0C0;'><strong>Valor</strong></td>
		                                            <td style='background-color: #C0C0C0;'><strong>Permissão</strong></td>
	                                            </tr>";

                                foreach (LogPerfilPermissoesViewModel item2 in distinctpermissaoPerfis)
                                {
                                    pPermissaoPerfis += $@"<tr><td>{item2.Tipo}</td><td>{item2.Valor}</td><td>{(item2.Excluido == true ? "Revogada" : "Concedida")}</td></tr>";
                                }

                                pPermissaoPerfis += "</table>";
                            }
                        }

                    }
                    catch (Exception ex) { }



                    var dNome = detalhes.Nome;
                    var dDescricao = detalhes.Descricao == null ? "" : detalhes.Descricao;
                    var PerfisAtiva = detalhes.Excluido ? "Inativo" : "Ativo";

                    string timeString = item.Data.ToString();
                    DateTime dt = DateTime.ParseExact(timeString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    var cabecalhoHistorico = new List<LogPerfilJsonViewModel>
                    {
                        new LogPerfilJsonViewModel {Data = dt.ToString(),
                                                    Usuario = usuario.Nome,
                                                    Campo = detalhes.Nome,

                                                    Nome = dNome,
                                                    Descricao = dDescricao,
                                                    Situacao = PerfisAtiva ,
                                                    Permissoes = pPermissaoPerfis
                                                    },
                    };


                    list.AddRange(cabecalhoHistorico);
                }

                var log = new LogViewModelJson { data = list };

                string jsonLog = System.Text.Json.JsonSerializer.Serialize(log);

                string jsonString = System.Text.Json.JsonSerializer.Serialize(list);

                return Json(jsonString);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }



    }
}
