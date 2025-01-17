using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelo.Aplicacao.DTO.Usuarios;
using Modelo.Aplicacao.Interfaces;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Web.Configuracoes.Claims;
using Modelo.Web.ViewModels;
using Modelo.Web.ViewModels.Perfis;
using Modelo.Web.ViewModels.Usuarios;

namespace Modelo.Web.Controllers
{
    [Authorize]
    public class UsuariosController : BaseController
    {
        private readonly IUsuarioAppServico _usuarioAppServico;
        
        private readonly INotificador _notificador;
        private readonly IPerfilAppServico _perfilAppServico;
        private readonly IMapper _mapper;
        

        private readonly IWebHostEnvironment _env;

        public UsuariosController(IUsuarioAppServico usuarioAppServico, 
            IMapper mapper, 
            INotificador notificador,           
            ILogServico logServico, 
            IPerfilAppServico perfilAppServico,
            IWebHostEnvironment env ) : base(notificador, logServico)
        {
            _usuarioAppServico = usuarioAppServico;
            _notificador = notificador;
            _mapper = mapper;           
            _perfilAppServico = perfilAppServico;
            
            _env = env;
        }

        public async Task<IActionResult> Index(string? filtro, string? sort)
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var usuarios = await _usuarioAppServico.ListarUsuariosTelaInicial(filtro);
                var model = _mapper.Map<List<UsuariosViewModel>>(usuarios.AsEnumerable());
                return PartialView("Grid", model.AsEnumerable());
            }

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _usuarioAppServico.Logout();
                HttpContext.Session.Clear();
            }

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _usuarioAppServico.Logout();
            }

            return View(nameof(Login));
        }

        [AllowAnonymous]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login([Bind("Usuario, Senha")] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _usuarioAppServico.Login(_env.WebRootPath, model.Usuario, model.Senha);

            if (!OperacaoValida())
                return View(model);

            return RedirectToAction("Index", "Home");

        }

        [AllowAnonymous]
        public async Task<IActionResult> CadastrarNovaSenha(Guid? token, string? email)
        {
            if (!token.HasValue || token == Guid.Empty || string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                return BadRequest();

            await _usuarioAppServico.ValidarTokenEmailCadastrarNovaSenha(token.Value, email);

            if (!OperacaoValida())
                return View();

            var model = new CadastrarNovaSenhaViewModel { Email = email, Token = token.Value };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarNovaSenha(Guid? token, [Bind("Email, Token, Senha, ConfirmarSenha")] CadastrarNovaSenhaViewModel model)
        {
            if (!token.HasValue || token != model.Token)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            await _usuarioAppServico.CadastrarNovaSenha(_mapper.Map<CadastrarNovaSenhaDTO>(model));

            if (!OperacaoValida())
                return View(model);

            TempData["Sucesso"] = "Senha cadastrada com sucesso !";

            await _usuarioAppServico.Login(_env.WebRootPath, model.Email, model.Senha);


            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Adicionar()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar([Bind("Nome, CPF, Emails, Telefone, Enderecos, CBHs, CBHPrincipal")] CadastrarEditarUsuarioViewModel model)
        {
            

            if (!ModelState.IsValid)
                return View(model);

            var usuario = _mapper.Map<CadastrarEditarUsuarioDTO>(model);

            await _usuarioAppServico.Cadastrar(_env.WebRootPath, usuario);

            if (!OperacaoValida())
                return View(model);

            TempData["Sucesso"] = "Usuário cadastrado com sucesso !";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            var usuario = await _usuarioAppServico.BuscarUsuarioParaEditarPorId(id);

            if (usuario == null)
                return BadRequest();            

            var model = _mapper.Map<CadastrarEditarUsuarioViewModel>(usuario);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, [Bind("Id, Nome, CPF, Emails, Telefone, Enderecos, CBHs, UsuarioAtivo, CBHPrincipal")] CadastrarEditarUsuarioViewModel model)
        {
           
            

            if (id != model.Id)
                ModelState.AddModelError("", "Usuário Inválido !");

            if (!ModelState.IsValid)
                return View(model);

            var usuario = _mapper.Map<CadastrarEditarUsuarioDTO>(model);

            await _usuarioAppServico.Editar(usuario);

            if (!OperacaoValida())
                return View(model);

            TempData["Sucesso"] = "Usuário atualizado com sucesso !";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Historico(Guid? id, string? filtro)
        {
            try
            {
                if (!id.HasValue || id == Guid.Empty)
                    return BadRequest();

                var logs = await _usuarioAppServico.BuscarLogUsuario(id.Value, filtro);

                if (logs == null)
                    return BadRequest();

                var logsViewModel = _mapper.Map<IList<LogViewModel>>(logs);

                string jsonString = System.Text.Json.JsonSerializer.Serialize(logsViewModel);

                return Json(jsonString);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar o log do usuário ! {ex.Message}");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var usuario = await _usuarioAppServico.BuscarUsuarioParaEditarPorId(id);

            if (usuario == null)
                return BadRequest("Usuário não encontrado !");

            await _usuarioAppServico.Excluir(id);

            if (!OperacaoValida())
            {
                var lista = new List<string>();
                foreach (var item in _notificador.ObterNotificacoes())
                {
                    ModelState.AddModelError(string.Empty, item.Mensagem);
                    lista.Add(item.Mensagem);
                }
                return BadRequest(lista);
            }

            return Ok("Usuário Excluido com sucesso !");
        }

        public async Task<IActionResult> Permissoes(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var usuario = await _usuarioAppServico.BuscarUsuarioPorId(id);

            if (usuario == null)
                return BadRequest();

            var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
            ViewBag.Categorias = claims.GroupBy(c => c.Item1).Select(g => g.First()).Select(c => c.Item1).OrderBy(c => c).ToList();
            ViewBag.ClaimModulos = claims;

             
            ViewBag.Perfis = _mapper.Map<IList<PerfilViewModel>>(await _perfilAppServico.BuscarPerfis()).OrderBy(c => c.Nome).ToList();

            Guid? perfilId = null;
            //if (usuario.Administrador)
            //{
            //    var perfil = _mapper.Map<PerfilViewModel>(await _perfilAppServico.BuscarPerfilAdministrador());
            //    perfilId = perfil.Id;
            //}
            
            

            var model = new CadastrarPerfilUsuarioViewModel
            {
                NomeUsuario = usuario.Nome,
                Email = usuario.Email,
                UsuarioId = id,
                PerfilId = perfilId,
               

            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Permissoes(Guid id, [FromBody] CadastrarPerfilUsuarioViewModel model)
        {
            if (id == Guid.Empty || model == null || !ModelState.IsValid)
                return BadRequest();

            try
            {
                var dto = _mapper.Map<CadastrarPerfilUsuarioDTO>(model);
                var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
                dto.Claims = claims.Where(c => model.Permissoes.Contains(c.Item3)).Select(c => c.Item4).ToList();
                await _usuarioAppServico.CadastrarPermissoesEPerfilUsuario(dto);

                if (!OperacaoValida())
                    return BadRequest(RecuperarListaErros());

                return Ok("Permissões cadastradas com sucesso !");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao confirmar as alterações das permissões do usuário !" + ex.Message);
            }
        }

        [AllowAnonymous]
        public IActionResult EsqueciSenha()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EsqueciSenha([Bind("Email")] EsqueciSenhaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _usuarioAppServico.ResetarSenha(_env.WebRootPath, model.Email);

            if (!OperacaoValida())
                return View(model);


            ViewBag.ExibirMensagemSucesso = true;
            return View();
        }


        public async Task<IActionResult> BuscarPermissoesCBHUsuario(Guid usuarioId, Guid cbhId)
        {
            if (usuarioId == Guid.Empty)
                return BadRequest("Identificador do usuário inválido !");
            else if (cbhId == Guid.Empty)
                return BadRequest("Identificador de cbh inválido !");
            
            try
            {
                //var dto = await _usuarioAppServico.BuscarPermissoesUsuarioCBHPorId(cbhId, usuarioId);

                //if (dto == null)
                //    return NotFound();

                //var model = new BuscarPerfilEPermissoesCBHUsuarioViewModel
                //{
                //    PerfilId = dto.PerfilId
                //};
                var claims = ClaimsUtils.RecuperarListaTuplasModulosClaims();
                //model.Permissoes = claims.IntersectBy(dto.Permissoes.Select(c => new { c.Type, c.Value }), c => new { c.Item4.Type, c.Item4.Value }).Select(c => c.Item3).ToArray();

                //return Ok(model);
                return Ok(null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar as permissões do usuário !" + ex.Message);
            }
        }

        public async Task<IActionResult> TrocarCBHUsuarioLogado(Guid cbhId)
        {
            if (!BuscarUsuarioIdLogado().HasValue)
                return BadRequest();

            await _usuarioAppServico.TrocarCBHUsuarioLogado(BuscarUsuarioIdLogado().Value, cbhId);

            if (OperacaoValida())
                TempData["Sucesso"] = "CBH selecionada com sucesso !";

            return RedirectToAction("Index", "Home");
        }
    }
}
