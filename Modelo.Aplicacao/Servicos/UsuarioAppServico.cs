using AutoMapper;
using Modelo.Aplicacao.DTO;
using Modelo.Aplicacao.DTO.Usuarios;
using Modelo.Aplicacao.Interfaces;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Dominio.Notificacoes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Modelo.Aplicacao.Servicos
{
    public class UsuarioAppServico : IUsuarioAppServico
    {
        private readonly IUsuarioServico _usuarioServico;
        private readonly IUsuarioRepositorio _usuarioRepositorio;        
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
       
        private readonly IPerfilRepositorio _perfilRepositorio;
        public UsuarioAppServico(IMapper mapper,INotificador notificador,IUsuarioRepositorio usuarioRepositorio,IUsuarioServico usuarioServico,IPerfilRepositorio perfilRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _notificador = notificador;
            _usuarioServico = usuarioServico;
            _perfilRepositorio = perfilRepositorio;
        }

        public async Task Cadastrar(string caminho, CadastrarEditarUsuarioDTO dto)
        {
            if (!string.IsNullOrEmpty(dto.CPF) && !string.IsNullOrWhiteSpace(dto.CPF))
                dto.CPF = dto.CPF.Replace(".", "").Replace("-", "");


            var usuario = _mapper.Map<Usuarios>(dto);
           

            await _usuarioServico.Adicionar(caminho, usuario);
        }

        public async Task<IList<UsuariosTelaInicialDTO>> ListarUsuariosTelaInicial(string? filtro)
        { 
            var dtos = _mapper.Map<IList<UsuariosTelaInicialDTO>>(await _usuarioRepositorio.BuscarUsuariosSemRastreio());

            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                dtos = dtos.Where(c => c.Nome.ToUpper().Contains(filtro.ToUpper()) || c.Email.ToUpper().Contains(filtro.ToUpper()) || c.CPF.Contains(filtro)).ToList();
            }

            return dtos;
        } 

        public async Task Login(string caminho, string email, string senha)
        => await _usuarioServico.Login(caminho, email, senha);

        public async Task Logout()
        {
            _usuarioServico.Logout();
        }

        public async Task<UsuariosTelaInicialDTO?> BuscarUsuarioTelaCadastrarNovaSenha(Guid idUsuario)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdSemRastreio(idUsuario);
            if (usuario == null)
                return null;

            return _mapper.Map<UsuariosTelaInicialDTO>(usuario);
        }

        public async Task CadastrarNovaSenha(CadastrarNovaSenhaDTO dto)
        {
            await _usuarioServico.CadastrarSenha(dto.Token, dto.Email, dto.Senha, dto.ConfirmarSenha);
        }
        public async Task<CadastrarEditarUsuarioDTO?> BuscarUsuarioParaEditarPorId(Guid id)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdParaEdicaoSemRastreio(id);
            if (usuario == null)
                return null;

            return _mapper.Map<CadastrarEditarUsuarioDTO>(usuario);
        }

        public async Task Editar(CadastrarEditarUsuarioDTO? dto)
        {
            if (dto == null || !dto.Id.HasValue)
            {
                _notificador.Adicionar(new Notificacao("Usuario inválido !"));
                return;
            }

            if (!string.IsNullOrEmpty(dto.CPF) && !string.IsNullOrWhiteSpace(dto.CPF))
                dto.CPF = dto.CPF.Replace(".", "").Replace("-", "");

           
            

            var usuario = _mapper.Map<Usuarios>(dto);
            
           
            await _usuarioServico.Editar(usuario);
        }
        public async Task Excluir(Guid id)
        => await _usuarioServico.Excluir(id);

        public async Task<IList<LogTransacoesDTO>?> BuscarLogUsuario(Guid usuarioId, string? filtro)
        {
            
            var todosPerfis = await _perfilRepositorio.BuscarTodos(false, true);            

            var historicoUsuario = await _usuarioRepositorio.BuscarLogPorUsuarioId(usuarioId);
           

            var configuracaoJSON = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            var listaLog = new List<LogTransacoesDTO>();


            foreach (var log in historicoUsuario.OrderBy(c => c.Data))
            {
                Usuarios usuario = JsonSerializer.Deserialize<Usuarios>(log.Dados, configuracaoJSON);
                if (log.Comando == "INSERT")
                {
                    foreach (var prop in usuario.GetType().GetProperties())
                    {
                        if (prop.Name != nameof(Usuarios.CPF) && prop.Name != nameof(Usuarios.NomeCompleto) 
                            )
                            continue;

                        if (prop.Name == nameof(Usuarios.CPF) || prop.Name == nameof(Usuarios.NomeCompleto))
                        {
                            var valorAtual = prop.GetValue(usuario);
                            listaLog.Add(new LogTransacoesDTO
                            {
                                Campo = prop.Name,
                                Data = log.Data,
                                EntidadeId = log.EntidadeId,
                                Tipo = "Inclusão",
                                Usuario = log.Usuario != null ? log.Usuario.NomeCompleto : "",
                                Valor = valorAtual.ToString()

                            });
                        }

                    }
                }
                else if (log.Comando == "UPDATE")
                {
                    foreach (var prop in usuario.GetType().GetProperties())
                    {
                        if (prop.Name != nameof(Usuarios.CPF) && prop.Name != nameof(Usuarios.NomeCompleto) 
                            && prop.Name != nameof(Usuarios.Inativo) )
                            continue;

                        if(prop.Name == nameof(Usuarios.Inativo))
                        {
                            var ultimoValorUsuarioAtivo = listaLog.Where(c => c.Campo == "Usuário Ativo ?").OrderByDescending(c => c.Data).FirstOrDefault();
                            if((ultimoValorUsuarioAtivo == null && usuario.Inativo) || (ultimoValorUsuarioAtivo != null && ultimoValorUsuarioAtivo.Valor != (usuario.Inativo ? "Falso" : "Verdadeiro")))
                            {
                                listaLog.Add(new LogTransacoesDTO
                                {
                                    Campo = "Usuário Ativo ?",
                                    Data = log.Data,
                                    EntidadeId = log.EntidadeId,
                                    Tipo = "Alteração",
                                    Usuario = log.Usuario != null ? log.Usuario.NomeCompleto : "",
                                    Valor = usuario.Inativo ? "Falso" : "Verdadeiro"
                                }) ;
                            }
                            continue;
                        }


                        var ultimoValor = listaLog.Where(c => c.Campo == prop.Name).OrderByDescending(c => c.Data).FirstOrDefault();
                        var valorAtual = prop.GetValue(usuario);
                        if (ultimoValor == null || ultimoValor.Valor != valorAtual.ToString())
                            listaLog.Add(new LogTransacoesDTO
                            {
                                Campo = prop.Name,
                                Data = log.Data,
                                EntidadeId = log.EntidadeId,
                                Tipo = "Alteração",
                                Usuario = log.Usuario != null ? log.Usuario.NomeCompleto : "",
                                Valor = valorAtual.ToString()

                            });
                    }


                }
            }

            //foreach (var log in historicoPermissoesUsuarios.OrderBy(c => c.Data))
            //{
            //    PermissoesCbhUsuario permissao = JsonSerializer.Deserialize<PermissoesCbhUsuario>(log.Dados, configuracaoJSON);
            //    var cbhUsuario = await _cBHRepositorio.BuscarPorCbhUsuarioId(permissao.CbhUsuarioId);
            //    listaLog.Add(new LogDTO
            //    {
            //        Campo = $"Permissão - CBH",
            //        Data = log.Data,
            //        EntidadeId = log.EntidadeId,
            //        Tipo = log.Comando == "INSERT" ? "Inclusão" : "Exclusão",
            //        Usuario = log.Usuario != null ? log.Usuario.NomeCompleto : "",
            //        Valor = string.Concat(permissao.Tipo, " - ", permissao.Valor, " - ", cbhUsuario.Nome)

            //    });
            //}

            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                listaLog = listaLog.Where(c => (c.Data.ToString().Contains(filtro) || c.Usuario.Contains(filtro) || c.Campo.ToUpper().Contains(filtro.ToUpper()) || c.Tipo.ToUpper().Contains(filtro.ToUpper()) || c.Valor.ToUpper().Contains(filtro.ToUpper()))).ToList();
            }
            return listaLog.OrderBy(c => c.Data).ToList();
        }

        public async Task TrocarCBHUsuarioLogado(Guid usuarioId, Guid cbhIdSelecionada)
        {
            await _usuarioServico.TrocarCBHUsuarioLogado(usuarioId, cbhIdSelecionada);
        }

        public async Task<UsuariosTelaInicialDTO?> BuscarUsuarioPorId(Guid idUsuario)
        => _mapper.Map<UsuariosTelaInicialDTO?>(await _usuarioRepositorio.BuscarUsuarioPorId(idUsuario));

        //public async Task<PerfilEPermissoesUsuarioDTO?> BuscarPermissoesUsuarioCBHPorId(Guid cbhId, Guid usuarioId)
        //{

        //    var dto = _mapper.Map<PerfilEPermissoesUsuarioDTO>(cbhUsuario);

        //    dto.Permissoes = new List<Claim>();
            
        //    return dto;
        //}

        private string BuscarNomeCampoLog(string nomeCampo)
        {
            string nomeNormalizado = "";
            switch (nomeCampo)
            {
                case nameof(Usuarios.NomeCompleto):
                    nomeNormalizado = "Nome";
                    break;
                case nameof(Usuarios.CPF):
                    nomeNormalizado = "CPF";
                    break;               

               
                default:
                    break;
            }
            if (string.IsNullOrEmpty(nomeNormalizado) && nomeCampo.Contains("Email"))
                nomeNormalizado = "E-mail";

            return nomeNormalizado;
        }

        public async Task CadastrarPermissoesEPerfilUsuario(CadastrarPerfilUsuarioDTO dto)
        {            
            await _usuarioServico.CadastrarPerfilPermissao(dto.UsuarioId,  dto.PerfilId);
        }

        public async Task ResetarSenha(string caminho, string email)
        {
            await _usuarioServico.ResetarSenha(caminho, email);
        }

        public async Task ValidarTokenEmailCadastrarNovaSenha(Guid token, string email)
        => await _usuarioServico.ValidarTokenEEmailResetarSenha(token, email);
    }
}
