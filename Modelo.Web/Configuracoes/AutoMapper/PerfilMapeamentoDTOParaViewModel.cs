using AutoMapper;
using Modelo.Aplicacao.DTO;
using Modelo.Aplicacao.DTO.Perfis;
using Modelo.Aplicacao.DTO.Usuarios;
using Modelo.Web.ViewModels;
using Modelo.Web.ViewModels.Perfis;
using Modelo.Web.ViewModels.Usuarios;

namespace Modelo.Web.Configuracoes.AutoMapper
{
    public class PerfilMapeamentoDTOParaViewModel : Profile
    {
        public PerfilMapeamentoDTOParaViewModel()
        {
            CreateMap<UsuariosTelaInicialDTO, UsuariosViewModel>();
            CreateMap<CadastrarEditarUsuarioDTO, CadastrarEditarUsuarioViewModel>();
            CreateMap<EnderecoCadastroUsuarioDTO, EnderecoViewModel>();
            CreateMap<LogDTO, LogViewModel>()
                .ForMember(c => c.DataFormatada, m =>
                {
                    m.MapFrom(c => c.Data.ToString("dd/MM/yyyy HH:mm:ss"));
                });



            CreateMap<SelectOptionDTO, SelectOptionViewModel>();

            CreateMap<PerfilDTO, PerfilViewModel>();

            CreateMap<PerfilDTO, PerfilViewModel>()
                .ForMember(c => c.Claims, m => m.Ignore());

            CreateMap<LogDTO, LogPerfilViewModel>()
             .ForMember(c => c.Data, m => m.MapFrom(c => c.Data))
             .ForMember(c => c.EntidadeId, m => m.MapFrom(c => c.EntidadeId))
             .ForMember(c => c.UsuarioId, m => m.MapFrom(c => c.UsuarioId))
             .ForMember(c => c.Dados, m => m.MapFrom(c => c.Dados));



        }   
    }
}
