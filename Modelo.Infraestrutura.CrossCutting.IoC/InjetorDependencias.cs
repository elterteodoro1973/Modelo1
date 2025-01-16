using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modelo.Aplicacao.Interfaces;
using Modelo.Aplicacao.Parsers;
using Modelo.Aplicacao.Servicos;
using Modelo.Dados.Repositorios;
using Modelo.Dados.Servicos;
using Modelo.Dominio.Interfaces;
using Modelo.Dominio.Interfaces.Repositorios;
//using Modelo.Dominio.Interfaces.Repositorios.Modelo.Dominio.Interfaces.Repositorios;
using Modelo.Dominio.Interfaces.Servicos;
using Modelo.Dominio.Notificacoes;
using Modelo.Dominio.Servicos;

namespace Modelo.Infraestrutura.CrossCutting.IoC
{
    public static class InjetorDependencias
    {
        public static void ConfigurarContextosEFCore(this IServiceCollection services, string conexao)
        {
           services.AddDbContext<Dados.Contexto.DbContexto>(o => o.UseSqlServer(conexao).EnableSensitiveDataLogging(), ServiceLifetime.Scoped);
        }

        public static void ConfigurarAutoMapper(this IServiceCollection services)
        {
           services.AddAutoMapper(typeof(PerfilMapeamentoEntidadeParaDTO), typeof(PerfilMapeamentoDTOParaEntidade));
        }

        public static void ConfigurarServicosERepositorios(this IServiceCollection services)
        {
            ////Repositorios
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();            
            services.AddScoped<IPerfilRepositorio, PerfilRepositorio>();           
            services.AddScoped<IPermissoesPerfilRepositorio, PermissoesPerfilRepositorio>();           
            services.AddScoped<ILogRepositorio, LogRepositorio>();

            ////Servicos de Dominio           
            services.AddScoped<IUsuarioServico, UsuarioServico>();           
            services.AddScoped<IPerfilServico, PerfilServico>(); 

            ////Servicos de Aplicacao
            services.AddScoped<IUsuarioAppServico, UsuarioAppServico>();           
            services.AddScoped<IPerfilAppServico, PerfilAppServico>();

            //Outros Servicos
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<ILogServico, LogServico>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IEmailServico, EmailServico>();
        }

        public static void ConfigurarMensagensMVC(this IServiceCollection services)
        {
            
            services.AddControllersWithViews().AddMvcOptions(o =>
            {
                o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "Este campo precisa ser preenchido.");
                o.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Este campo precisa ser preenchido.");
                o.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "É necessário que o body na requisição não esteja vazio.");
                o.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O campo deve ser numérico");
                o.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => "O valor preenchido é inválido para este campo.");
                o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "O campo deve ser numérico.");
                o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "Este campo precisa ser preenchido.");
                o.Filters.Add(
                    new ResponseCacheAttribute
                    {
                        NoStore = true,
                        Location = ResponseCacheLocation.None
                    });
                o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

           
        }

    }
}
