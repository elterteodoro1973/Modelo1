using Modelo.Dominio.Entidades;
using Modelo.Dominio.Entidades.Validacoes.Perfis;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Dominio.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Dominio.Servicos
{
    public class PerfilServico : BaseServico<Perfis>,  IPerfilServico
    {
        private readonly INotificador _notificador;
        private readonly IPerfilRepositorio _perfilRepositorio;
        private readonly IPermissoesPerfilRepositorio _permissoesPerfilRepositorio;
        public PerfilServico(INotificador notificador, ILogServico logServico, IPerfilRepositorio perfilRepositorio, IPermissoesPerfilRepositorio permissoesPerfilRepositorio) : base(notificador, logServico)
        {
            _notificador = notificador;
            _perfilRepositorio = perfilRepositorio;
            _permissoesPerfilRepositorio = permissoesPerfilRepositorio;
        }

        public async Task Adicionar(Perfis perfil)
        {
            await _ValidarInclusao(perfil);

            if (_notificador.TemNotificacao()) return;

            await _perfilRepositorio.IniciarTransacao();

            try
            {
                await _perfilRepositorio.Adicionar(perfil);
                await _perfilRepositorio.SalvarAlteracoes();
                await _perfilRepositorio.Commit();
            }
            catch (Exception ex)
            {
                _notificador.Adicionar(new Notificacao("Erro ao adicionar o Perfil !" + ex.Message));
            }
        }

        public async Task Editar(Perfis perfil)
        {
            await _ValidarEdicao(perfil);

            if (_notificador.TemNotificacao()) return;

            var permissoesDB = await _permissoesPerfilRepositorio.BuscarPermissoesPorPerfil(perfil.Id);

            var permissoesAdd = perfil.PermissoesPerfis.ExceptBy(permissoesDB.Select(c => new { c.TipoAcaoId, c.TipoAcessoId }), c => new { c.TipoAcaoId, c.TipoAcessoId }).ToList();
            var permissoesRemover = permissoesDB.ExceptBy(perfil.PermissoesPerfis.Select(c => new { c.TipoAcaoId, c.TipoAcessoId }), c => new { c.TipoAcaoId, c.TipoAcessoId }).ToList();

            await _perfilRepositorio.IniciarTransacao();

            try
            {
                await _perfilRepositorio.Atualizar(perfil);

                if (permissoesAdd.Any())
                    await _permissoesPerfilRepositorio.AdicionarLista(permissoesAdd);
                
                if (permissoesRemover.Any())
                    await _permissoesPerfilRepositorio.RemoverLista(permissoesRemover);

                await _perfilRepositorio.SalvarAlteracoes();
                await _perfilRepositorio.Commit();
            }
            catch (Exception ex)
            {
                await _perfilRepositorio.Roolback();
                _notificador.Adicionar(new Notificacao("Erro ao alterar o perfil !" + ex.Message));
            }
        }

        private async Task _ValidarInclusao(Perfis perfil)
        {
            if (ExecutarValidacao<AdicionarEditarPerfilValidacao, Perfis>(new AdicionarEditarPerfilValidacao(false), perfil)) return;
        }

        private async Task _ValidarEdicao(Perfis perfil)
        {
            if (ExecutarValidacao<AdicionarEditarPerfilValidacao, Perfis>(new AdicionarEditarPerfilValidacao(true), perfil)) return;

            if (!await _perfilRepositorio.VerficarSeIdEValido(perfil.Id))
                _notificador.Adicionar(new Notificacao("Identificador de Perfil inválido !"));
        }

        public async Task Excluir(Guid id)
        {
            await _ValidarExclusao(id);

            if (_notificador.TemNotificacao()) return;

            var perfilDB = await _perfilRepositorio.BuscarPorId(id);
            await _perfilRepositorio.IniciarTransacao();

            try
            {
                await _perfilRepositorio.Excluir(perfilDB);
                await _perfilRepositorio.SalvarAlteracoes();
                await _perfilRepositorio.Commit();
            }
            catch (Exception ex)
            {
                await _perfilRepositorio.Roolback();
                _notificador.Adicionar(new Notificacao("Erro ao excluir o perfil !" + ex.Message));
            }
        }

        private async Task _ValidarExclusao(Guid id)
        {
            if (id == Guid.Empty)
                _notificador.Adicionar(new Notificacao("Identificador é obrigatório !"));
            else if (!await _perfilRepositorio.VerficarSeIdEValido(id))
                _notificador.Adicionar(new Notificacao("Identificador inválido !"));
            //else if (await _perfilRepositorio.PerfilPossuiUsuariosAssociados(id))
            //    _notificador.Adicionar(new Notificacao("Perfil possui usuários associados !"));

        }
    }
}
