using AutoMapper;
using Modelo.Aplicacao.DTO;
using Modelo.Aplicacao.DTO.Perfis;
using Modelo.Aplicacao.DTO.Usuarios;
using Modelo.Dominio.Entidades;

namespace Modelo.Aplicacao.Parsers
{
    public class PerfilMapeamentoDTOParaEntidade : Profile
    {
        public PerfilMapeamentoDTOParaEntidade()
        {
            CreateMap<CadastrarEditarUsuarioDTO, Usuario>()
                .ForMember(c => c.Id, m =>
                {
                    m.MapFrom(c => c.Id.HasValue ? c.Id : Guid.NewGuid());
                })                
                .ForMember(c => c.NomeCompleto, m => m.MapFrom(c => c.Nome))
                .ForMember(c => c.CPF, m => m.MapFrom(c => c.CPF))
                
                .ForMember(c => c.Email, m =>
                {
                    m.PreCondition(c => c.Emails.Any());
                    m.MapFrom(c => c.Emails.First());
                })
                
                .ForMember(c => c.Inativo, m => m.MapFrom(c => !c.UsuarioAtivo));

          


            CreateMap<PerfilDTO, Perfil>()
                .ForMember(c => c.Id, m =>
                {
                    m.PreCondition(c => c.Id.HasValue);
                    m.MapFrom(c => c.Id);
                })
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
                .ForMember(c => c.Descricao, m => m.MapFrom(c => c.Descricao))
                .ForMember(c => c.Administrador, m => m.MapFrom(c => c.Administrador));

          
          

            CreateMap<LogDTO, Log>()
            .ForMember(c => c.Data, m => m.MapFrom(c => c.Data))
            .ForMember(c => c.EntidadeId, m => m.MapFrom(c => c.EntidadeId))
            .ForMember(c => c.UsuarioId, m => m.MapFrom(c => c.UsuarioId))
            .ForMember(c => c.Dados, m => m.MapFrom(c => c.Dados));


        }
    }
}
