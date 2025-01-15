using Modelo.Dominio.Entidades;
using Modelo.Dominio.Entidades.Validacoes.Empreendimentos;
using Modelo.Dominio.Entidades.Validacoes.Perfis;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Notificacoes;
using Modelo.Infraestrutura.Crosscutting.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Infraestrutura.Crosscutting.Identity.Servicos
{
    public class PerfilServico : BaseServicoIdentity, IPerfilServico
    {
        private readonly INotificador _notificador;
        private readonly IPerfilRepositorio _perfilRepositorio;
        private readonly RoleManager<Perfil> _gerenciadorPerfil;
        public PerfilServico(INotificador notificador, RoleManager<Perfil> gerenciadorPerfil, IPerfilRepositorio perfilRepositorio) : base(notificador)
        {
            _notificador = notificador;
            _perfilRepositorio = perfilRepositorio;
            _gerenciadorPerfil = gerenciadorPerfil;
        }

        public async Task Adicionar(Perfil perfil)
        {
           
        }

        public async Task Adicionar(Perfil perfil, IList<Claim> permissoes)
        {
            await _ValidarInclusaoPerfil(perfil);

            if (_notificador.TemNotificacao()) return;

            IList<PermissaoPerfil> permissoesPerfil = new List<PermissaoPerfil>();
            foreach (Claim claim in permissoes) 
            {
                permissoesPerfil.Add(new PermissaoPerfil
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                    RoleId = perfil.Id,
                });
            }
            await _perfilRepositorio.IniciarTransacao();
            try
            {
                perfil.NormalizedName = _gerenciadorPerfil.NormalizeKey(perfil.Name);
                
                await _perfilRepositorio.Adicionar(perfil);
                await _perfilRepositorio.AdicionarPermissoesPerfil(permissoesPerfil);
                await _perfilRepositorio.SalvarAlteracoes();
                await _perfilRepositorio.Commit();
            }
            catch (Exception ex)
            {
                await _perfilRepositorio.Rollback();
                _notificador.Adicionar(new Notificacao("Erro ao adicionar o Perfil"));
            }
        }

        public Task AdicionarPerfilUsuario(Guid perfilId, Guid usuarioId, Guid cbhId)
        {
            throw new NotImplementedException();
        }

        private async Task _ValidarInclusaoPerfil(Perfil perfil)
        {
            if (!ExecutarValidacao<AdicionarEditarPerfilValidacao, Perfil>(new AdicionarEditarPerfilValidacao(false), perfil)) return;


        }

    }
}
