using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Modelo.Dominio.DTO;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Entidades.Validacoes.Usuarios;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Dominio.Notificacoes;

namespace Modelo.Dominio.Servicos
{

    public class ResetarSenhaServico : BaseServico<ResetarSenha>, IResetarSenhaServico
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IResetarSenhaRepositorio _resetarSenhaRepositorio;
        private INotificador _notificador;
        public ResetarSenhaServico(IHttpContextAccessor httpContext,
            IResetarSenhaRepositorio resetarSenhaRepositorio,
            INotificador notificador,
            ILogServico logServico,
            
            IOptions<EmailConfiguracao> emailConfiguracao) : base(notificador, logServico)
        {
            _httpContext = httpContext;
            _resetarSenhaRepositorio = resetarSenhaRepositorio;
            _notificador = notificador;           
        }


        

        public async Task Adicionar(ResetarSenha resetarSenha)
        {
            await _resetarSenhaRepositorio.IniciarTransacao();

            try
            {
                await _resetarSenhaRepositorio.Adicionar(resetarSenha);
                await _resetarSenhaRepositorio.SalvarAlteracoes();
                await _resetarSenhaRepositorio.Commit();
            }
            catch (Exception ex)
            {
                await _resetarSenhaRepositorio.Roolback();
                _notificador.Adicionar(new Notificacao("Erro ao adicionar o Usuario !" + ex.Message));
            }
        }



        public async Task Excluir(Guid resetarSenhaId)
        {
            var usuarioDB = await _resetarSenhaRepositorio.BuscarPorId(resetarSenhaId);
            await _resetarSenhaRepositorio.IniciarTransacao();

            try
            {
                await _resetarSenhaRepositorio.Excluir(usuarioDB);
                await _resetarSenhaRepositorio.SalvarAlteracoes();
                await _resetarSenhaRepositorio.Commit();
            }
            catch (Exception ex)
            {
                await _resetarSenhaRepositorio.Roolback();
                _notificador.Adicionar(new Notificacao("Erro ao excluir o resete de senha !"));
            }
        }

        
        
         
        
    }
}