using AutoMapper;
using Modelo.Aplicacao.DTO;
using Modelo.Aplicacao.DTO.Perfis;
using Modelo.Aplicacao.DTO.Usuarios;
using Modelo.Dominio.Entidades;

namespace Modelo.Aplicacao.Parsers
{
    public class MapeamentoEntidadeParaDTO : Profile
    {
        public MapeamentoEntidadeParaDTO()
        {
            CreateMap<Usuarios, UsuariosTelaInicialDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.CPF, m => m.MapFrom(c => c.CPF))
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.NomeCompleto))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            CreateMap<Usuarios, LoginUsuarioDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.NomeCompleto, m => m.MapFrom(c => c.NomeCompleto));

            CreateMap<Usuarios, CadastrarEditarUsuarioDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.NomeCompleto))
                .ForMember(c => c.CPF, m => m.MapFrom(c => c.CPF))
                .ForMember(c => c.Email, m => m.MapFrom(c => c.Email));

            CreateMap<Perfis, PerfilDTO>()
                .ForMember(c => c.Id, m => m.MapFrom(c => c.Id))
                .ForMember(c => c.Nome, m => m.MapFrom(c => c.Nome))
                .ForMember(c => c.Descricao, m => m.MapFrom(c => c.Descricao))
                .ForMember(c => c.Claims, m => m.Ignore());           

            CreateMap<LogTransacoes, LogTransacoesDTO>()
                .ForMember(c => c.Data, m => m.MapFrom(c => c.Data))
                .ForMember(c => c.EntidadeId, m => m.MapFrom(c => c.EntidadeId))
                .ForMember(c => c.UsuarioId, m => m.MapFrom(c => c.UsuarioId))
                .ForMember(c => c.Dados, m => m.MapFrom(c => c.Dados));
           
        }
    }
}
