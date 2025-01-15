using AutoMapper;
using Modelo.Aplicacao.DTO;
using Modelo.Aplicacao.DTO.Perfis;
using Modelo.Aplicacao.DTO.Usuarios;
using Modelo.Web.ViewModels.Perfis;
using Modelo.Web.ViewModels.Usuarios;

namespace Modelo.Web.Configuracoes.AutoMapper
{
    public class PerfilMapeamentoViewModelParaDTO : Profile
    {
        public PerfilMapeamentoViewModelParaDTO()
        {
            CreateMap<CadastrarEditarUsuarioViewModel, CadastrarEditarUsuarioDTO>();
            CreateMap<EnderecoViewModel, EnderecoCadastroUsuarioDTO>();
            CreateMap<CadastrarNovaSenhaViewModel, CadastrarNovaSenhaDTO>();
            
            CreateMap<PerfilViewModel, PerfilDTO>()
                .ForMember(c => c.Id, m =>
                {
                    m.PreCondition(c => c.Id.HasValue);
                    m.MapFrom(c => c.Id);
                }).
                ForMember(c => c.Descricao, m => m.MapFrom(c => c.Descricao))
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
                .ForMember(c => c.Administrador, m => m.MapFrom(c => c.Administrador))
                .ForMember(c => c.Claims, m => m.Ignore());
            
            CreateMap<CadastrarPerfilUsuarioViewModel, CadastrarPerfilUsuarioDTO>()
                .ForMember(c => c.PerfilId, m => m.MapFrom(c => c.PerfilId))
                .ForMember(c => c.UsuarioId, m => m.MapFrom(c => c.UsuarioId))
                .ForMember(c => c.CbhId, m => m.MapFrom(c => c.CbhId)); 

            CreateMap<LogPerfilViewModel, LogTransacoesDTO>()
               .ForMember(c => c.Data, m => m.MapFrom(c => c.Data))
               .ForMember(c => c.EntidadeId, m => m.MapFrom(c => c.EntidadeId))
               .ForMember(c => c.UsuarioId, m => m.MapFrom(c => c.UsuarioId))
               .ForMember(c => c.Dados, m => m.MapFrom(c => c.Dados));
        }
    }
}
