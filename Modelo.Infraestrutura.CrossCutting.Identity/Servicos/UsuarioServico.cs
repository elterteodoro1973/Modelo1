using Modelo.Dominio.Entidades;
using Modelo.Dominio.Entidades.Validacoes.Usuarios;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Dominio.Notificacoes;
using Modelo.Dominio.Servicos;
using Modelo.Infraestrutura.Crosscutting.Identity.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Modelo.Infraestrutura.Crosscutting.Identity.Servicos
{
    public class UsuarioServico : BaseServicoIdentity, IUsuarioServico
    {
        private readonly UserManager<Usuario> _gerenciadorUsuarios;
        private readonly SignInManager<Usuario> _gerenciadorLogin;
        private readonly INotificador _notificador;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ICBHUsuarioRepositorio _cbhUsuarioRepositorio;
        private readonly ICBHRepositorio _cbhRepositorio;
        private readonly ITelefoneUsuarioRepositorio _telefoneRepositorio;
        private readonly IEmailUsuarioRepositorio _emailUsuarioRepositorio;
        private readonly IEnderecoUsuarioRepositorio _enderecoUsuarioRepositorio;
        private readonly ILogServico _logServico;
        private readonly ILogRepositorio _logRepositorio;
        private readonly IHttpContextAccessor _httpContext;

        public UsuarioServico(INotificador notificador, UserManager<Usuario> gerenciadorUsuarios,
            IUsuarioRepositorio usuarioRepositorio, ICBHUsuarioRepositorio cbhUsuarioRepositorio,
            SignInManager<Usuario> gerenciadorLogin, ITelefoneUsuarioRepositorio telefoneRepositorio,
            IEmailUsuarioRepositorio emailUsuarioRepositorio, IEnderecoUsuarioRepositorio enderecoUsuarioRepositorio,
            ILogServico logServico, ILogRepositorio logRepositorio, ICBHRepositorio cbhRepositorio, IHttpContextAccessor httpContext) : base(notificador)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _notificador = notificador;
            _gerenciadorUsuarios = gerenciadorUsuarios;
            _gerenciadorLogin = gerenciadorLogin;
            _cbhUsuarioRepositorio = cbhUsuarioRepositorio;
            _telefoneRepositorio = telefoneRepositorio;
            _emailUsuarioRepositorio = emailUsuarioRepositorio;
            _enderecoUsuarioRepositorio = enderecoUsuarioRepositorio;
            _logServico = logServico;
            _logRepositorio = logRepositorio;
            _cbhRepositorio = cbhRepositorio;
            _httpContext = httpContext;
        }

        public async Task Adicionar(Usuario usuario, IList<EnderecoUsuario> enderecos, IList<TelefoneUsuario> telefones, IList<EmailUsuario> email, IList<Guid> CBHs)
        {
            await _ValidarInclusaoUsuario(usuario, enderecos, telefones, email, CBHs);

            if (_notificador.TemNotificacao()) return;

            using (var t = await _usuarioRepositorio.IniciarTransacao())
            {
                try
                {
                    //usuario.Id = Guid.NewGuid();
                    usuario.NormalizedEmail = _gerenciadorUsuarios.NormalizeEmail(usuario.Email);
                    usuario.NormalizedUserName = _gerenciadorUsuarios.NormalizeName(usuario.UserName);
                    await _usuarioRepositorio.Adicionar(usuario);
                    await _usuarioRepositorio.SalvarAlteracoes();
                    var log = new LogComando
                    {
                        Id = Guid.NewGuid(),
                        IdEntidade = usuario.Id,
                        TipoComando = "INSERT",
                        Data = DateTime.Now,
                        IdUsuario = _logServico.BuscarUsuarioId()
                    };
                    IList<PropriedadeLogComando> listaPropriedades = new List<PropriedadeLogComando>();
                    listaPropriedades.Add(new PropriedadeLogComando
                    {
                        Id = Guid.NewGuid(),
                        IdLog = log.Id,
                        Propriedade = nameof(Usuario.NomeCompleto),
                        Valor = usuario.NomeCompleto,
                    });
                    listaPropriedades.Add(new PropriedadeLogComando
                    {
                        Id = Guid.NewGuid(),
                        IdLog = log.Id,
                        Propriedade = nameof(Usuario.CPF),
                        Valor = usuario.CPF,
                    });
                    listaPropriedades.Add(new PropriedadeLogComando
                    {
                        Id = Guid.NewGuid(),
                        IdLog = log.Id,
                        Propriedade = nameof(Usuario.CBHPrincipalId),
                        Valor = usuario.CBHPrincipalId.ToString(),
                    });

                    await _logRepositorio.AdicionarLog(log, listaPropriedades);

                    for (int i = 0; i < enderecos.Count; i++)
                    {
                        enderecos[i].UsuarioId = usuario.Id;
                        enderecos[i].Posicao = i + 1;
                    }

                    for (int i = 0; i < telefones.Count; i++)
                    {
                        telefones[i].UsuarioId = usuario.Id;
                        telefones[i].Posicao = i + 1;
                    }

                    for (int i = 0; i < email.Count; i++)
                    {
                        email[i].UsuarioId = usuario.Id;
                        email[i].Posicao = i + 1;
                    }

                    IList<CBHUsuario> cbhUsuario = new List<CBHUsuario>();
                    foreach (var item in CBHs)
                    {
                        cbhUsuario.Add(new CBHUsuario
                        {
                            Id = Guid.NewGuid(),
                            CBHId = item,
                            UsuarioId = usuario.Id,
                        });
                    }

                    if (cbhUsuario.Count > 0)
                        await _cbhUsuarioRepositorio.AdicionarLista(cbhUsuario);

                    if (enderecos.Count > 0)
                        await _enderecoUsuarioRepositorio.AdicionarLista(enderecos);

                    if (telefones.Count > 0)
                        await _telefoneRepositorio.AdicionarLista(telefones);

                    if (email.Count > 0)
                        await _emailUsuarioRepositorio.AdicionarLista(email);

                    await _usuarioRepositorio.SalvarAlteracoes();
                    await _usuarioRepositorio.Commit();
                }
                catch (Exception ex)
                {
                    await _usuarioRepositorio.RoolBaack();
                    _notificador.Adicionar(new Notificacao(ex.Message));
                }

            }


        }

        private async Task _ValidarInclusaoUsuario(Usuario usuario, IList<EnderecoUsuario> enderecos, IList<TelefoneUsuario> telefones, IList<EmailUsuario> email, IList<Guid> CBHs)
        {
            if (usuario == null)
            {
                Notificar("Usuario inválido !");
                return;
            }

            if (!ExecutarValidacao(new CadastrarUsuarioValidacao(), usuario))
                return;

            if (!usuario.CBHPrincipalId.HasValue || usuario.CBHPrincipalId == Guid.Empty)
            {
                Notificar("CBH Principal é obrigatório !");
            }
            else if (!await _cbhRepositorio.IdCbhValido(usuario.CBHPrincipalId.Value))
            {
                Notificar("CBH Principal inválido !");
            }

            foreach (var item in enderecos)
            {
                ExecutarValidacao(new CadastrarEnderecoUsuarioValidacao(), item);
            }

            foreach (var item in telefones)
            {
                ExecutarValidacao(new CadastrarTelefoneUsuarioValidacao(), item);
            }

            foreach (var item in email)
            {
                if (ExecutarValidacao(new CadastrarEmailUsuarioValidacao(), item))
                {
                    if (await _usuarioRepositorio.EmailPrincipalJaCadastrado(item.Email, null))
                        Notificar($"E-mail {item.Email} já cadastrado !");
                    else if (await _emailUsuarioRepositorio.EmailJaCadastrado(item.Email, null))
                        Notificar($"E-mail {item.Email} já cadastrado !");
                }
            }
        }

        public async Task<Usuario?> Login(string email, string senha)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                    _notificador.Adicionar(new Notificacao("E-mail é obrigatório para poder fazer Login !"));

                if (string.IsNullOrEmpty(senha) || string.IsNullOrWhiteSpace(senha))
                    _notificador.Adicionar(new Notificacao("Senha é obrigatório para poder fazer Login !"));

                if (_notificador.TemNotificacao()) return null;

                var usuario = await _usuarioRepositorio.BuscarPorEmail(email);

                if (usuario == null)
                {
                    _notificador.Adicionar(new Notificacao("Usuário não encontrado !"));
                    return null;
                }

                if (string.IsNullOrEmpty(usuario.PasswordHash))
                    return usuario;

                var senhaValida = await _gerenciadorLogin.CheckPasswordSignInAsync(usuario, senha, false);

                if (!senhaValida.Succeeded)
                {
                    _notificador.Adicionar(new Notificacao("Senha Inválida !"));
                    return null;
                }

                var cbhsUsuario = await _cbhUsuarioRepositorio.BuscarCBHsUsuarioId(usuario.Id);
                if (cbhsUsuario.Count < 1)
                {
                    _notificador.Adicionar(new Notificacao("Usuário não está cadastrado em nenhuma CBH"));
                    return null;
                }

                if (await _gerenciadorLogin.CanSignInAsync(usuario))
                {
                    IList<Claim> listaClaim = new List<Claim>();
                    listaClaim.Add(new Claim("NomeUsuario", usuario.NomeCompleto));
                    var listacbhs = cbhsUsuario.OrderBy(c => c.CBH.Nome).Select(c => new { cbhId = c.CBHId, cbhNome = c.CBH.Nome }).ToList();
                    listaClaim.Add(new Claim("CBHs", JsonSerializer.Serialize(listacbhs)));
                    if (usuario.CBHPrincipalId.HasValue)
                    {
                        var cbhPrincipal = await _cbhRepositorio.BuscarPorId(usuario.CBHPrincipalId.Value);
                        if (cbhPrincipal != null)
                            listaClaim.Add(new Claim("CBHSelecionada", JsonSerializer.Serialize(new { cbhId = cbhPrincipal.Id, cbhNome = cbhPrincipal.Nome })));
                    }

                    await _gerenciadorLogin.SignInWithClaimsAsync(usuario, false, listaClaim);
                    //await _gerenciadorLogin.SignInAsync(usuario, false);

                    return usuario;
                }
                else
                    _notificador.Adicionar(new Notificacao("Não foi possivel fazer login do usuário informado !"));
            }
            catch (Exception ex)
            {
                _notificador.Adicionar(new Notificacao(ex.Message));
            }
            return null;
        }

        public async Task Logout()
        {
            await _gerenciadorLogin.SignOutAsync();
        }

        public async Task CadastrarSenha(Guid idUsuario, string senha, string confirmarSenha)
        {
            await _ValidarCadastrarNovaSenha(idUsuario, senha, confirmarSenha);

            try
            {
                await _usuarioRepositorio.CadastrarSenha(idUsuario, senha);
            }
            catch (Exception)
            {
                _notificador.Adicionar(new Notificacao("Erro ao cadastrar nova Senha !"));
            }
        }

        private async Task _ValidarCadastrarNovaSenha(Guid idUsuario, string senha, string confirmarSenha)
        {
            if (!await _usuarioRepositorio.IdUsuarioValido(idUsuario))
            {
                _notificador.Adicionar(new Notificacao("Usuario Inválido !"));
                return;
            }

            if (string.IsNullOrEmpty(senha) || string.IsNullOrWhiteSpace(senha))
                _notificador.Adicionar(new Notificacao("Senha é obrigatório !"));
            else if (senha.Length < 6 || senha.Length > 30)
                _notificador.Adicionar(new Notificacao("Senha possui tamanho mínimo de 6 caracteres e máximo de 30 caracteres !"));

            if (_notificador.TemNotificacao())
                return;

            if (string.IsNullOrEmpty(confirmarSenha) || string.IsNullOrWhiteSpace(confirmarSenha))
                _notificador.Adicionar(new Notificacao("Confirmacação de Senha é obrigatório !"));
            else if (confirmarSenha.Length < 6 || confirmarSenha.Length > 30)
                _notificador.Adicionar(new Notificacao("Confirmacação de Senha possui tamanho mínimo de 6 caracteres e máximo de 30 caracteres !"));
            else if (senha != confirmarSenha)
                _notificador.Adicionar(new Notificacao("Confirmação de Senha diferente de Senha !"));
        }

        public async Task Atualizar(Guid id, Usuario usuario, IList<EnderecoUsuario> enderecos, IList<TelefoneUsuario> telefones, IList<EmailUsuario> email, IList<Guid> CBHs)
        {
            await _ValidarAtualizacaoUsuario(id, usuario, enderecos, telefones, email, CBHs);

            if (_notificador.TemNotificacao())
                return;

            var usuarioDB = await _usuarioRepositorio.BuscarUsuarioPorId(id);
            var emailsDB = await _emailUsuarioRepositorio.BuscarEmailsPorUsuarioId(id);
            var enderecosDB = await _enderecoUsuarioRepositorio.BuscarEnderecosPorUsuarioId(id);
            var telefonesDB = await _telefoneRepositorio.BuscarTelefonesPorUsuarioId(id);
            var cbhsAssociadasDB = await _cbhUsuarioRepositorio.BuscarCBHsUsuarioId(id);


            var telefonesAdd = telefones.ExceptBy(telefonesDB.Select(c => c.NumeroTelefone), c => c.NumeroTelefone).ToList();
            var telefonesRemover = telefonesDB.ExceptBy(telefones.Select(c => c.NumeroTelefone), c => c.NumeroTelefone).ToList();
            var enderecosAdd = enderecos.ExceptBy(enderecosDB.Select(c => new { c.CEP, c.Logradouro, c.Numero, c.Complemento, c.Estado, c.Municipio }), c => new { c.CEP, c.Logradouro, c.Numero, c.Complemento, c.Estado, c.Municipio }).ToList();
            var enderecosRemover = enderecosDB.ExceptBy(enderecos.Select(c => new { c.CEP, c.Logradouro, c.Numero, c.Complemento, c.Estado, c.Municipio }), c => new { c.CEP, c.Logradouro, c.Numero, c.Complemento, c.Estado, c.Municipio }).ToList();
            var emailAdd = email.ExceptBy(emailsDB.Select(c => c.Email), c => c.Email).ToList();
            var emailRemover = emailsDB.ExceptBy(email.Select(c => c.Email), c => c.Email).ToList();
            var cbhsAdd = CBHs.ExceptBy(cbhsAssociadasDB.Select(c => c.CBHId), c => c).ToList();
            var cbhsRemover = cbhsAssociadasDB.ExceptBy(CBHs, c => c.CBHId).ToList();

            var log = new LogComando
            {
                Id = Guid.NewGuid(),
                Data = DateTime.Now,
                IdEntidade = id,
                IdUsuario = _logServico.BuscarUsuarioId(),
                TipoComando = "UPDATE"
            };
            IList<PropriedadeLogComando> propriedadesModificadasUsuario = new List<PropriedadeLogComando>();
            if (usuarioDB.NomeCompleto != usuario.NomeCompleto)
            {
                usuarioDB.NomeCompleto = usuario.NomeCompleto;
                propriedadesModificadasUsuario.Add(new PropriedadeLogComando
                {
                    Id = Guid.NewGuid(),
                    IdLog = log.Id,
                    Propriedade = nameof(usuarioDB.NomeCompleto),
                    Valor = usuario.NomeCompleto
                });
            }

            if (usuarioDB.CPF != usuario.CPF)
            {
                usuarioDB.CPF = usuario.CPF;
                propriedadesModificadasUsuario.Add(new PropriedadeLogComando
                {
                    Id = Guid.NewGuid(),
                    IdLog = log.Id,
                    Propriedade = nameof(usuarioDB.CPF),
                    Valor = usuario.CPF
                });
            }

            if (usuarioDB.LockoutEnabled != usuario.LockoutEnabled)
            {
                usuarioDB.LockoutEnabled = usuario.LockoutEnabled;
                propriedadesModificadasUsuario.Add(new PropriedadeLogComando
                {
                    Id = Guid.NewGuid(),
                    IdLog = log.Id,
                    Propriedade = nameof(usuarioDB.LockoutEnabled),
                    Valor = usuarioDB.LockoutEnabled.ToString()
                });
            }

            if (usuarioDB.Email != usuario.Email)
            {
                usuarioDB.Email = usuario.Email;
                usuarioDB.UserName = usuario.Email;
                usuarioDB.NormalizedEmail = _gerenciadorUsuarios.NormalizeEmail(usuario.Email);
                usuarioDB.NormalizedUserName = _gerenciadorUsuarios.NormalizeName(usuario.Email);
            }

            if (usuarioDB.CBHPrincipalId != usuario.CBHPrincipalId)
            {
                usuarioDB.CBHPrincipalId = usuario.CBHPrincipalId;
                propriedadesModificadasUsuario.Add(new PropriedadeLogComando
                {
                    Id = Guid.NewGuid(),
                    IdLog = log.Id,
                    Propriedade = nameof(Usuario.CBHPrincipalId),
                    Valor = usuario.CBHPrincipalId.ToString()
                });
            }

            for (int i = 0; i < telefonesAdd.Count; i++)
            {
                telefonesAdd[i].UsuarioId = id;
                telefonesAdd[i].Posicao = telefonesDB == null ? i + 1 : telefonesDB.MaxBy(c => c.Posicao).Posicao + (i + 1);
            }

            for (int i = 0; i < emailAdd.Count; i++)
            {
                emailAdd[i].UsuarioId = id;
                emailAdd[i].Posicao = emailsDB == null ? i + 1 : emailsDB.MaxBy(c => c.Posicao).Posicao + (i + 1);
            }

            for (int i = 0; i < enderecosAdd.Count; i++)
            {
                enderecosAdd[i].UsuarioId = id;
                enderecosAdd[i].Posicao = enderecosDB == null ? i + 1 : enderecosDB.MaxBy(c => c.Posicao).Posicao + (i + 1);
            }

            IList<CBHUsuario> listaCBHAdd = new List<CBHUsuario>();
            foreach (var item in cbhsAdd)
            {
                listaCBHAdd.Add(new CBHUsuario
                {
                    UsuarioId = id,
                    CBHId = item,
                    Id = Guid.NewGuid()
                });
            }

            using (var t = await _usuarioRepositorio.IniciarTransacao())
            {
                try
                {
                    _usuarioRepositorio.Atualizar(usuarioDB);
                    if (propriedadesModificadasUsuario.Count > 0)
                        await _logRepositorio.AdicionarLog(log, propriedadesModificadasUsuario);

                    if (emailAdd.Count > 0)
                        await _emailUsuarioRepositorio.AdicionarLista(emailAdd);

                    if (telefonesAdd.Count > 0)
                        await _telefoneRepositorio.AdicionarLista(telefonesAdd);

                    if (enderecosAdd.Count > 0)
                        await _enderecoUsuarioRepositorio.AdicionarLista(enderecosAdd);

                    if (listaCBHAdd.Count > 0)
                        await _cbhUsuarioRepositorio.AdicionarLista(listaCBHAdd);

                    if (emailRemover.Count > 0)
                        await _emailUsuarioRepositorio.RemoverLista(emailRemover);

                    if (telefonesRemover.Count > 0)
                        await _telefoneRepositorio.RemoverLista(telefonesRemover);

                    if (enderecosRemover.Count > 0)
                        await _enderecoUsuarioRepositorio.RemoverLista(enderecosRemover);

                    if (cbhsRemover.Count > 0)
                        await _cbhUsuarioRepositorio.RemoverLista(cbhsRemover);

                    await _usuarioRepositorio.SalvarAlteracoes();
                    await _usuarioRepositorio.Commit();

                }
                catch (Exception ex)
                {
                    await _usuarioRepositorio.RoolBaack();
                    _notificador.Adicionar(new Notificacao("Erro ao atualizar as informações do usuário !"));

                }
            }

        }

        private async Task _ValidarAtualizacaoUsuario(Guid id, Usuario usuario, IList<EnderecoUsuario> enderecos, IList<TelefoneUsuario> telefones, IList<EmailUsuario> email, IList<Guid> CBHs)
        {
            if (usuario == null || id == Guid.Empty)
            {
                Notificar("Usuario inválido !");
                return;
            }

            if (!await _usuarioRepositorio.IdUsuarioValido(id))
            {
                Notificar("Usuario inválido !");
                return;
            }

            if (!usuario.CBHPrincipalId.HasValue || usuario.CBHPrincipalId == Guid.Empty)
            {
                Notificar("CBH Principal é obrigatório !");
            }
            else if (!await _cbhRepositorio.IdCbhValido(usuario.CBHPrincipalId.Value))
            {
                Notificar("CBH Principal inválido !");
            }

            if (!ExecutarValidacao(new CadastrarUsuarioValidacao(), usuario))
                return;

            foreach (var item in enderecos)
            {
                ExecutarValidacao(new CadastrarEnderecoUsuarioValidacao(), item);
            }

            foreach (var item in telefones)
            {
                ExecutarValidacao(new CadastrarTelefoneUsuarioValidacao(), item);
            }

            foreach (var item in email)
            {
                if (ExecutarValidacao(new CadastrarEmailUsuarioValidacao(), item))
                {
                    if (await _usuarioRepositorio.EmailPrincipalJaCadastrado(item.Email, id))
                        Notificar($"E-mail {item.Email} já cadastrado !");
                    else if (await _emailUsuarioRepositorio.EmailJaCadastrado(item.Email, id))
                        Notificar($"E-mail {item.Email} já cadastrado !");
                }
            }
        }

        public async Task Excluir(Guid id)
        {
            await _ValidarExclusaoUsuario(id);

            if (_notificador.TemNotificacao())
                return;

            var usuarioDB = await _usuarioRepositorio.BuscarUsuarioPorId(id);
            usuarioDB.Excluido = true;
            var logComando = new LogComando
            {
                Id = Guid.NewGuid(),
                IdUsuario = _logServico.BuscarUsuarioId(),
                TipoComando = "DELETE",
                Data = DateTime.Now,
                IdEntidade = id,
            };
            IList<PropriedadeLogComando> listaPropriedades = new List<PropriedadeLogComando>();
            listaPropriedades.Add(new PropriedadeLogComando
            {
                Id = Guid.NewGuid(),
                IdLog = logComando.Id,
                Propriedade = "Excluido",
                Valor = "true"
            });
            using (var t = await _usuarioRepositorio.IniciarTransacao())
            {
                try
                {
                    _usuarioRepositorio.Atualizar(usuarioDB);
                    await _logRepositorio.AdicionarLog(logComando, listaPropriedades);
                    await _usuarioRepositorio.SalvarAlteracoes();
                    await _usuarioRepositorio.Commit();
                }
                catch (Exception ex)
                {
                    await _usuarioRepositorio.RoolBaack();
                    _notificador.Adicionar(new Notificacao("Erro ao excluir O usuario !"));
                }
            }

        }

        private async Task _ValidarExclusaoUsuario(Guid id)
        {
            if (id == Guid.Empty || await _usuarioRepositorio.BuscarUsuarioPorIdSemRastreio(id) == null)
            {
                _notificador.Adicionar(new Notificacao("Identificador de usuário inválido !"));
                return;
            }
        }

        public async Task TrocarCBHUsuarioLogado(Guid idUsuario, Guid cbhIdSelecionada)
        {
            await _ValidarTrocaCBH(idUsuario, cbhIdSelecionada);

            if (_notificador.TemNotificacao()) return;

            var usuario = await _usuarioRepositorio.BuscarUsuarioPorId(idUsuario);
            var cbhsUsuario = await _cbhUsuarioRepositorio.BuscarCBHsUsuarioId(usuario.Id);
            IList<Claim> listaClaim = new List<Claim>();
            listaClaim.Add(new Claim("NomeUsuario", usuario.NomeCompleto));
            var listacbhs = cbhsUsuario.OrderBy(c => c.CBH.Nome).Select(c => new { cbhId = c.CBHId, cbhNome = c.CBH.Nome }).ToList();
            listaClaim.Add(new Claim("CBHs", JsonSerializer.Serialize(listacbhs)));

            var cbhPrincipal = await _cbhRepositorio.BuscarPorId(cbhIdSelecionada);
            if (cbhPrincipal != null)
                listaClaim.Add(new Claim("CBHSelecionada", JsonSerializer.Serialize(new { cbhId = cbhPrincipal.Id, cbhNome = cbhPrincipal.Nome })));

            await _gerenciadorLogin.SignOutAsync();

            if (await _gerenciadorLogin.CanSignInAsync(usuario))
                await _gerenciadorLogin.SignInWithClaimsAsync(usuario, false, listaClaim);

        }

        private async Task _ValidarTrocaCBH(Guid idUsuario, Guid cbhIdSelecionada)
        {
            if (idUsuario == Guid.Empty)
            {
                Notificar("Identificador de Usuário é obrigatório !");
            }
            else if (!await _usuarioRepositorio.IdUsuarioValido(idUsuario))
            {
                Notificar("Identificador de Usuário inválido !");
            }

            if (cbhIdSelecionada == Guid.Empty)
            {
                Notificar("Identificador de CBH é obrigatório !");
            }
            else if (!await _cbhRepositorio.IdCbhValido(cbhIdSelecionada))
            {
                Notificar("Identificador de CBH inválido !");
            }
            else if (await _cbhUsuarioRepositorio.UsuarioNaoPossuiPermissaoParaCBHSelecionada(idUsuario, cbhIdSelecionada))
            {
                Notificar("Usuário não possui permissão para CBH informada !");
            }


        }
    }
}
