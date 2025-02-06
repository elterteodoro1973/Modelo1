using Modelo.Dominio.DTO;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Entidades.Validacoes.Usuarios;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Dominio.Notificacoes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security;

namespace Modelo.Dominio.Servicos
{
    public class UsuarioServico : BaseServico<Usuarios>, IUsuarioServico
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IResetarSenhaRepositorio _resetarSenhaRepositorio;
        private INotificador _notificador;       
        private readonly IPerfilRepositorio _perfilRepositorio;       
        private readonly EmailConfiguracao _emailConfiguracao;
        private readonly IEmailServico _emailServico;
       

        public UsuarioServico(IHttpContextAccessor httpContext, 
            IUsuarioRepositorio usuarioRepositorio,
            IResetarSenhaRepositorio resetarSenhaRepositorio,
            INotificador notificador, 
            ILogServico logServico,
            IEmailServico emailServico,
            IPerfilRepositorio perfilRepositorio,
            IOptions<EmailConfiguracao> emailConfiguracao  ) : base(notificador, logServico)
        {
            _httpContext = httpContext;
            _usuarioRepositorio = usuarioRepositorio;
            _resetarSenhaRepositorio = resetarSenhaRepositorio;
            _notificador = notificador;           
            _perfilRepositorio = perfilRepositorio;
            _emailConfiguracao = emailConfiguracao.Value;
            _emailServico = emailServico;
        }

        public async Task Adicionar(string caminho, Usuarios usuario)
        {
            await _ValidarInclusao(usuario);

            if (_notificador.TemNotificacao()) return;
            

            await _usuarioRepositorio.IniciarTransacao();

            //try
            //{
            //    await _usuarioRepositorio.Adicionar(usuario);

            //    var historico = new HistoricoSenhaUsuario
            //    {
            //        Id = Guid.NewGuid(),
            //        DataSolicitacao = DateTime.Now,
            //        DataExpiracao = DateTime.Now.AddDays(3),
            //        UsuarioId = usuario.Id,
            //        Token = Guid.NewGuid()
            //    };

            //    await _historicoSenhaUsuarioRepositorio.Adicionar(historico);

            //    var urlResetSenha = string.Concat(_httpContext.HttpContext.Request.Scheme, "://", _httpContext.HttpContext.Request.Host.Value, $"/Usuarios/CadastrarNovaSenha?token={historico.Token}&email={usuario.EmailLogin}");

            //    //string textoEmail = $"Link para cadastrar uma nova Senha: {urlResetSenha}";

            //    string textoEmail = _emailServico.GetTextoResetSenha(caminho, usuario.NomeCompleto, urlResetSenha);
            //    await _emailServico.Enviar(usuario.EmailLogin, "Cadastrar Senha!", textoEmail);


            //    await _usuarioRepositorio.SalvarAlteracoes();
            //    await _usuarioRepositorio.Commit();
            //}
            //catch (Exception ex)
            //{
            //    await _usuarioRepositorio.Roolback();
            //    _notificador.Adicionar(new Notificacao("Erro ao adicionar o Usuario !" + ex.Message));
            //}
        }

        public async Task Editar(Usuarios usuario)
        {
            await _ValidarEdicao(usuario);

            if (_notificador.TemNotificacao()) return;


            var usuarioDB = await _usuarioRepositorio.BuscarUsuarioPorId(usuario.Id);
                      

            await _usuarioRepositorio.IniciarTransacao();
            try
            {
                usuarioDB.NomeCompleto = usuario.NomeCompleto;
                usuarioDB.CPF = usuario.CPF;
                usuarioDB.Email = usuario.Email;                
                usuarioDB.Inativo = usuario.Inativo;

                await _usuarioRepositorio.Atualizar(usuarioDB);
                await _usuarioRepositorio.SalvarAlteracoes();
                await _usuarioRepositorio.Commit();
            }
            catch (Exception ex)
            {
                await _usuarioRepositorio.Roolback();
                _notificador.Adicionar(new Notificacao("Erro ao ediar o Usuario !" + ex.Message));
            }
        }

        public async Task Excluir(Guid usuarioId)
        {
            await _ValidarExclusao(usuarioId);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarPorId(usuarioId);
            await _usuarioRepositorio.IniciarTransacao();

            try
            {
                await _usuarioRepositorio.Excluir(usuarioDB);
                await _usuarioRepositorio.SalvarAlteracoes();
                await _usuarioRepositorio.Commit();
            }
            catch (Exception ex)
            {
                await _usuarioRepositorio.Roolback();
                _notificador.Adicionar(new Notificacao("Erro ao excluir o usuário !"));
            }
        }

        public async Task Login(string caminho,  string email, string senha)
        {
            await _ValidarLogin(email, senha);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarPorEmail(email);

            if (!await _usuarioRepositorio.SenhaValidaLogin(email, GetSha256Hash(senha)))
            {
                _notificador.Adicionar(new Notificacao("Senha inválida !"));
                return;
            }
            else
            {
                //await _cobrancaServico.GerarCobrancaAnualPorCbh(usuarioDB.CBHPrincipalId.Value, 2023);
                await Login(usuarioDB);
            }
        }
        private async Task Login(Usuarios usuarioDB)
        {
            if (_httpContext.HttpContext.User.Identity != null && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                await _httpContext.HttpContext.SignOutAsync();
                _httpContext.HttpContext.Session.Clear();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("UsuarioId", usuarioDB.Id.ToString()));
            identity.AddClaim(new Claim("NomeUsuario", usuarioDB.NomeCompleto));


            if (usuarioDB.Perfil.Administrador.Value)
                identity.AddClaim(new Claim(ClaimTypes.Role, "ADMINISTRADOR"));

            var claimPrincipal = new ClaimsPrincipal(identity);
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties
            {
                IsPersistent = false,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
            });
        }

        private async Task _ValidarInclusao(Usuarios usuario)
        {
            if (!ExecutarValidacao<CadastrarEditarUsuarioValidacao, Usuarios>(new CadastrarEditarUsuarioValidacao(), usuario)) return;

            if (_notificador.TemNotificacao()) return;
                        
            if (await _usuarioRepositorio.EmailPrincipalJaCadastrado(usuario.Email, null))
                _notificador.Adicionar(new Notificacao("E-mail principal já cadastrado !"));
        }

        private async Task _ValidarEdicao(Usuarios usuario)
        {
            //if (!ExecutarValidacao<CadastrarEditarUsuarioValidacao, Usuarios>(new CadastrarEditarUsuarioValidacao(true), usuario)) return;
              
            if (_notificador.TemNotificacao()) return;


            if (await _usuarioRepositorio.EmailPrincipalJaCadastrado(usuario.Email, usuario.Id))
                _notificador.Adicionar(new Notificacao("E-mail principal já cadastrado !"));
        }

        private async Task _ValidarExclusao(Guid usuarioId)
        {
            if (usuarioId == Guid.Empty)
            {
                _notificador.Adicionar(new Notificacao("Identificador de Usuário obrigatório !"));
            }
            else if (!await _usuarioRepositorio.VerficarSeIdEValido(usuarioId))
            {
                _notificador.Adicionar(new Notificacao("Identificador de Usuário inválido !"));
            }
        }

        private string GetSha256Hash(string input)
        {
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = SHA256.HashData(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        public void Logout()
        {
            if (_httpContext.HttpContext.User.Identity != null && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _httpContext.HttpContext.SignOutAsync();
                _httpContext.HttpContext.Session.Clear();
            }
        }

        private async Task _ValidarLogin(string email, string senha)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                _notificador.Adicionar(new Notificacao("E-mail é obrigatório !"));

            if (string.IsNullOrEmpty(senha) || string.IsNullOrWhiteSpace(senha))
                _notificador.Adicionar(new Notificacao("Senha é obrigatório !"));

            if (_notificador.TemNotificacao()) return;

            if (!await _usuarioRepositorio.EmailValidoLogin(email))
            {
                _notificador.Adicionar(new Notificacao("E-mail é inválido !"));
                return;
            }

            var usuario = await _usuarioRepositorio.BuscarPorEmail(email);

            

        }

        public async Task CadastrarSenha(string token, string email, string senha, string confirmarSenha)
        {
            await _ValidarCadastroNovaSenha(token, email, senha, confirmarSenha);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarPorEmail( email);           
            await _usuarioRepositorio.IniciarTransacao();

            var resetarSenhaDB = await _resetarSenhaRepositorio.BuscarResetarSenhaPorToken(token);
            await _resetarSenhaRepositorio.IniciarTransacao();

            try
            {                
                usuarioDB.Senha = GetSha256Hash(senha);                
                await _usuarioRepositorio.Atualizar(usuarioDB);
                await _usuarioRepositorio.SalvarAlteracoes();
                await _usuarioRepositorio.Commit();

                resetarSenhaDB.Efetivado = true;
                await _resetarSenhaRepositorio.Atualizar(resetarSenhaDB);
                await _resetarSenhaRepositorio.SalvarAlteracoes();
                await _resetarSenhaRepositorio.Commit();
            }
            catch (Exception ex)
            {
                await _usuarioRepositorio.Roolback();
                await _resetarSenhaRepositorio.Roolback();
                _notificador.Adicionar(new Notificacao("Erro ao cadastrar a nova senha do usuário !" + ex.Message));
            }

            await Login(usuarioDB);
        }

        private async Task _ValidarCadastroNovaSenha(string token, string email, string senha, string confirmarSenha)
        {
            await ValidarTokenResetarSenha(token);

            if (string.IsNullOrEmpty(senha) || string.IsNullOrWhiteSpace(senha))
                _notificador.Adicionar(new Notificacao("Senha é obrigatório !"));
            else if (senha.Length > 30)
                _notificador.Adicionar(new Notificacao("Senha possui tamanho máximo de 50 caracteres !"));

            if (_notificador.TemNotificacao()) return;

            if (string.IsNullOrEmpty(confirmarSenha) || string.IsNullOrWhiteSpace(confirmarSenha))
                _notificador.Adicionar(new Notificacao("Confirmar senha é obrigatório !"));
            else if (confirmarSenha.Length > 30)
                _notificador.Adicionar(new Notificacao("Confirmar senha possui tamanho máximo de 50 caracteres !"));

            if (_notificador.TemNotificacao()) return;

            if (senha != confirmarSenha)
                _notificador.Adicionar(new Notificacao("Senhas diferentes !"));
        }

        public async Task TrocarCBHUsuarioLogado(Guid usuarioId, Guid cbhIdSelecionada)
        {
            await _ValidarTrocaCBHUsuarioLogado(usuarioId, cbhIdSelecionada);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarUsuarioPorIdSemRastreio(usuarioId);

            if (_httpContext.HttpContext.User.Identity != null && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                await _httpContext.HttpContext.SignOutAsync();
                _httpContext.HttpContext.Session.Clear();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("UsuarioId", usuarioDB.Id.ToString()));
            identity.AddClaim(new Claim("NomeUsuario", usuarioDB.NomeCompleto));


            var claimPrincipal = new ClaimsPrincipal(identity);
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties
            {
                IsPersistent = false,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
            });

        }

        private async Task _ValidarTrocaCBHUsuarioLogado(Guid usuarioId, Guid cbhIdSelecionada)
        {
            if (usuarioId == Guid.Empty)
                _notificador.Adicionar(new Notificacao("Identificador de usuário é obrigatório !"));

            if (cbhIdSelecionada == Guid.Empty)
                _notificador.Adicionar(new Notificacao("Identificador de CBH é obrigatório !"));

            if (_notificador.TemNotificacao()) return;

            if (!await _usuarioRepositorio.IdUsuarioValido(usuarioId))
                _notificador.Adicionar(new Notificacao("Identificador de usuário inválido !"));

            

            if (_notificador.TemNotificacao()) return;

            

        }

        public async Task CadastrarPerfilPermissao(Guid usuarioId,  Guid perfilId)
        {
            //await _ValidarCadastroPerfilPermissaoUsuarioCBH(usuarioId, cbhId, perfilId, permissoes);

            if (_notificador.TemNotificacao()) return;

            var perfil = await _perfilRepositorio.BuscarPorId(perfilId);
            Usuarios? usuarioDB = await _usuarioRepositorio.BuscarPorId(usuarioId);



            //if ( perfil.Nome.ToUpper().Trim() == "ADMINISTRADOR")
            //{
            //    usuarioDB.Administrador = true;

            //}

            await _usuarioRepositorio.IniciarTransacao();

            try
            {
                if (usuarioDB != null)
                    await _usuarioRepositorio.Atualizar(usuarioDB);

                await _usuarioRepositorio.SalvarAlteracoes();
                await _usuarioRepositorio.Commit();

            }
            catch (Exception ex)
            {
                await _usuarioRepositorio.Roolback();
                _notificador.Adicionar(new Notificacao("Erro ao alterar as permissões do usuário !"));
            }

        }

        //private async Task _ValidarCadastroPerfilPermissaoUsuarioCBH(Guid usuarioId, Guid cbhId, Guid perfilId, IList<PermissoesCbhUsuario> permissoes)
        //{

        //    var usuario = await _usuarioRepositorio.BuscarPorId(usuarioId, false);
        //    if (usuarioId == Guid.Empty)
        //        _notificador.Adicionar(new Notificacao("Identificador do usuário obrigatório !"));
        //    else if (!await _usuarioRepositorio.IdUsuarioValido(usuarioId))
        //        _notificador.Adicionar(new Notificacao("Identificador do usuário inválido !"));

        //    if (_notificador.TemNotificacao()) return;

        //    if (cbhId == Guid.Empty && !await _perfilRepositorio.VerificarSeIdEPerfilAdmDAEE(perfilId))
        //        _notificador.Adicionar(new Notificacao("Identificador de CBH usuário é obrigatório quando o perfil não for Administrador!"));
           
            

        //    if (_notificador.TemNotificacao()) return;

        //    if (perfilId == Guid.Empty)
        //        _notificador.Adicionar(new Notificacao("Identificador do Perfil é obrigatório !"));
        //    else if (!await _perfilRepositorio.VerficarSeIdEValido(perfilId))
        //        _notificador.Adicionar(new Notificacao("Identificador do Perfil inválido !"));

        //}

        public async Task ResetarSenha(string caminho, string email)
        {
            await _ValidarReseteSenha(email);

            if (_notificador.TemNotificacao()) return;

            var usuarioDB = await _usuarioRepositorio.BuscarPorEmail(email);
            
            await _resetarSenhaRepositorio.IniciarTransacao();
            try
            {
                var token = Guid.NewGuid();

                ResetarSenha resetarSenhaDB = new ResetarSenha()
                {
                    Id = Guid.NewGuid(),
                    Token = GerarHash512(token.ToString()).Replace("/", "b").Replace("=", "a").Replace("+", "C"),
                    UsuarioId = usuarioDB.Id,
                    DataSolicitacao = DateTime.Now,
                    DataExpiracao = DateTime.Now.AddHours(4),
                    Excluido = false
                };
                await _resetarSenhaRepositorio.Adicionar(resetarSenhaDB,"ResetarSenha",usuarioDB.Id);

                var urlResetSenha = string.Concat(_httpContext.HttpContext.Request.Scheme, "://", _httpContext.HttpContext.Request.Host.Value, $"/Usuarios/CadastrarNovaSenha?token={resetarSenhaDB.Token}");
                string textoEmail = _emailServico.GetTextoResetSenha(caminho, usuarioDB.NomeCompleto, urlResetSenha);
                await _emailServico.Enviar(email, "Resetar Senha!", textoEmail);

                await _resetarSenhaRepositorio.SalvarAlteracoes();
                await _resetarSenhaRepositorio.Commit();
            }
            catch (Exception ex)
            {                
                await _resetarSenhaRepositorio.Roolback();
                _notificador.Adicionar(new Notificacao("Erro ao resetar a senha do usuário: " + ex.Message));
            }
            return;
        }        
        
        public static string GerarHash512(string chave)
        {
            byte[] saltBytes = null;

            int minSaltSize = 4;
            int maxSaltSize = 8;

            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            saltBytes = new byte[saltSize];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(chave);
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = new SHA512Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            return hashValue;
        }

        private async Task _ValidarReseteSenha(string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                _notificador.Adicionar(new Notificacao("E-mail é obrigatório !"));
            else if (!await _usuarioRepositorio.EmailValidoLogin(email))
                _notificador.Adicionar(new Notificacao("E-mail inválido !"));
        }        

        public async Task ValidarTokenResetarSenha(string token)
        {
            if (token == string.Empty)
            {
                _notificador.Adicionar(new Notificacao("Token é obrigatório !"));
                return;
            }

            var resetarSenha = await _resetarSenhaRepositorio.BuscarResetarSenhaPorToken(token);

            if (resetarSenha == null)
            {
                _notificador.Adicionar(new Notificacao("Token inválido!"));
                return;
            }
            else if (resetarSenha.DataExpiracao < DateTime.Now)
            {
                _notificador.Adicionar(new Notificacao("Link de Acesso vencido !"));
                return;
            }
            else if (resetarSenha.Efetivado.Value )
            {
                _notificador.Adicionar(new Notificacao("Token já validado!"));
                return;
            }
        }
    }
}
