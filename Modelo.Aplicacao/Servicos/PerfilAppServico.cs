using AutoMapper;
using Modelo.Aplicacao.DTO;
using Modelo.Aplicacao.DTO.Perfis;
using Modelo.Aplicacao.DTO.Usuarios;
using Modelo.Aplicacao.Interfaces;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections;

namespace Modelo.Aplicacao.Servicos
{
    public class PerfilAppServico : IPerfilAppServico
    {
        private readonly IMapper _mapper;
        private readonly IPerfilServico _perfilServico;
        private readonly IPerfilRepositorio _perfilRepositorio;
        private readonly IPermissoesPerfilRepositorio _permissoesPerfilRepositorio;
        private readonly ILogRepositorio _logRepositorio;

        public PerfilAppServico(IMapper mapper, IPerfilServico perfilServico, IPerfilRepositorio perfilRepositorio, IPermissoesPerfilRepositorio permissoesPerfilRepositorio, ILogRepositorio logRepositorio)
        {
            _mapper = mapper;
            _perfilServico = perfilServico;
            _perfilRepositorio = perfilRepositorio;
            _permissoesPerfilRepositorio = permissoesPerfilRepositorio;
            _logRepositorio = logRepositorio;
        }
        public async Task Adicionar(PerfilDTO dto)
        {
            var perfil = _mapper.Map<Perfis>(dto);
            perfil.Id = Guid.NewGuid();
            perfil.PermissoesPerfis = new List<PermissoesPerfis>();
            foreach (var item in dto.Claims)
            {
                perfil.PermissoesPerfis.Add(new PermissoesPerfis
                {
                    Id = Guid.NewGuid(),
                    PerfilId = perfil.Id,
                    //Tipo = item.Type,
                    //Valor = item.Value,
                });
            }
            await _perfilServico.Adicionar(perfil);
        }


        public async Task<IList<LogTransacoesDTO>> BuscarLogs(Guid id, string? filtro)
        {
            var lista = new List<LogTransacoesDTO>();
            var logs = await _logRepositorio.BuscarPorIdEntidade(id);
            var idsPerfis = await _permissoesPerfilRepositorio.BuscarIdsPermissoesPerfilPorPerfilId(id);
            var configuracaoJSON = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            if (logs.Any())
            {
                foreach (var log in logs.OrderBy(c => c.Data))
                {
                    var perfil = JsonSerializer.Deserialize<Perfis>(log.Dados, configuracaoJSON);
                    if (perfil != null)
                    {
                        if (log.Comando == "INSERT")
                        {
                            foreach (var propriedade in perfil.GetType().GetProperties())
                            {
                                if (propriedade.Name == nameof(EntidadeBase.Id) || propriedade.Name == nameof(EntidadeBase.Excluido) || propriedade.Name == nameof(Perfis.Excluido) 
                                    || propriedade.Name == nameof(Perfis.PermissoesPerfis))
                                    continue;

                                lista.Add(new LogTransacoesDTO { Data = log.Data, Tipo = "Inclusão", Campo = propriedade.Name, Valor = propriedade.GetValue(perfil).ToString(), Usuario = log.Usuario.NomeCompleto });
                            }

                            if (perfil.PermissoesPerfis.Any())
                            {
                                foreach (var permissao in perfil.PermissoesPerfis)
                                {
                                    lista.Add(new LogTransacoesDTO { Data = log.Data, Tipo = "Inclusão", Campo = "Permissões", 
                                        //Valor = string.Concat(permissao.Tipo, " - ", permissao.Valor), 
                                        Usuario = log.Usuario.NomeCompleto });
                                }
                            }

                        }
                        else if (log.Comando == "UPDATE")
                        {
                            foreach (var propriedade in perfil.GetType().GetProperties())
                            {
                                var valor = propriedade.GetValue(perfil).ToString();

                                if (propriedade.Name == nameof(EntidadeBase.Id) || propriedade.Name == nameof(EntidadeBase.Excluido) || propriedade.Name == nameof(Perfis.Excluido) 
                                     || propriedade.Name == nameof(Perfis.PermissoesPerfis))
                                    continue;

                                if (lista.Where(c => c.Campo == propriedade.Name && c.Valor != valor && c.Data > lista.Max(c => c.Data)).Any())
                                    lista.Add(new LogTransacoesDTO { Data = log.Data, Tipo = log.Comando = "Alteração", Campo = propriedade.Name, Valor = propriedade.GetValue(perfil).ToString(), Usuario = log.Usuario.NomeCompleto });
                            }
                        }

                    }


                }
            }

            if (idsPerfis.Any())
            {
                var logsPermissoes = await _permissoesPerfilRepositorio.BuscarLogsPorIdsPermissoes(idsPerfis);
                foreach (var log in logsPermissoes.OrderBy(c => c.Data))
                {
                    var permissao = JsonSerializer.Deserialize<PermissoesPerfis>(log.Dados, configuracaoJSON);
                    lista.Add(new LogTransacoesDTO
                    {
                        Campo = "Permissões",
                        Data = log.Data,
                        Usuario = log.Usuario.NomeCompleto,
                        Tipo = log.Comando == "INSERT" ? "Inclusão" : log.Comando == "UPDATE" ? "Alteração" : "Exclusão",
                        //Valor = string.Concat(permissao.Tipo, " - ", permissao.Valor)
                    });
                }
            }


            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrWhiteSpace(filtro))
            {
                lista = lista.Where(c => (c.Data.ToString().Contains(filtro) || c.Usuario.Contains(filtro) || c.Campo.Contains(filtro) || c.Tipo.Contains(filtro) || c.Valor.Contains(filtro))).ToList();
            }

            return lista.OrderBy(c => c.Data).ToList();
        }

        public async Task<IList<PerfilDTO>> BuscarPerfis()
            => _mapper.Map<IList<PerfilDTO>>(await _perfilRepositorio.BuscarPerfis());

        public async Task<PerfilDTO?> BuscarPerfilAdministrador()
        {
            return _mapper.Map<PerfilDTO>(await _perfilRepositorio.BuscarPerfilAdministrador());
        }

        public async Task<PerfilDTO?> BuscarPorId(Guid id)
        {
            var perfil = await _perfilRepositorio.BuscarPorIdParaEdicao(id);
            if (perfil == null) return null;
            var dto = _mapper.Map<PerfilDTO>(perfil);
            dto.Claims = new List<Claim>();
            //foreach (var item in perfil.PermissoesPerfis)
            //{
            //    dto.Claims.Add(new Claim(item.Tipo, item.Valor));
            //}
            return dto;
        }



        public async Task<IList<LogTransacoesDTO>> BuscarLogPerfilPorId(Guid? idPerfil)
        {
            if (!idPerfil.HasValue)
                return null;

            var logsFinalizades = _mapper.Map<IList<LogTransacoesDTO>>(await _perfilRepositorio.BuscarTodosEntidadeId(idPerfil.Value));

            return logsFinalizades.OrderByDescending(c => Convert.ToDateTime(c.Data)).ToList();
        }

        public async Task Editar(PerfilDTO dto)
        {
            var perfil = _mapper.Map<Perfis>(dto);
            perfil.PermissoesPerfis = new List<PermissoesPerfis>();
            foreach (var item in dto.Claims)
            {
                perfil.PermissoesPerfis.Add(new PermissoesPerfis
                {
                    Id = Guid.NewGuid(),
                    PerfilId = perfil.Id,
                    //Tipo = item.Type,
                    //Valor = item.Value,
                });
            }
            await _perfilServico.Editar(perfil);
        }

        public async Task Excluir(Guid id)
        {
            await _perfilServico.Excluir(id);
        }
    }
}
