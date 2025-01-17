using System;
using DAEE.Cobranca.Dados.Mapeamentos;
using Microsoft.EntityFrameworkCore;
using Modelo.Dados.Mapeamentos;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Servicos;

namespace Modelo.Dados.Contexto
{
    public class DbContexto : DbContext
    {
        private readonly ILogServico _logServico;
        public DbContexto(DbContextOptions<DbContexto> options, ILogServico logServico) : base(options)
        {
            _logServico = logServico;
        }

        public virtual DbSet<LogAcessos> LogAcessos { get; set; }
        public virtual DbSet<LogTransacoes> LogTransacoes { get; set; }
        public virtual DbSet<Perfis> Perfis { get; set; }
        public virtual DbSet<PermissoesPerfis> PermissoesPerfis { get; set; }
        public virtual DbSet<TipoAcao> TipoAcao { get; set; }
        public virtual DbSet<TipoAcesso> TipoAcesso { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UsuarioMapeamento());
            builder.ApplyConfiguration(new PerfisMapeamento());
            builder.ApplyConfiguration(new PermissaoPerfilMapeamento());
            builder.ApplyConfiguration(new TipoAcessoMapeamento());
            builder.ApplyConfiguration(new TipoAcaoMapeamento());
            builder.ApplyConfiguration(new LogTransacoesMapeamento());
        }

        public override int SaveChanges()
        {

            SalvarLog().ConfigureAwait(false).GetAwaiter().GetResult();


            foreach (var item in ChangeTracker.Entries()
               .Where(e => e.State == EntityState.Deleted &&
               e.Metadata.GetProperties().Any(x => x.Name == "Excluido")))
            {
                item.State = EntityState.Unchanged;
                item.CurrentValues["Excluido"] = true;
            }
            return base.SaveChanges();

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await SalvarLog();
            foreach (var item in ChangeTracker.Entries()
              .Where(e => e.State == EntityState.Deleted &&
              e.Metadata.GetProperties().Any(x => x.Name == "Excluido")))
            {
                item.State = EntityState.Unchanged;
                item.CurrentValues["Excluido"] = true;
            }


            return await base.SaveChangesAsync(cancellationToken);
        }


        private async Task SalvarLog()
        {
            try
            {
                ChangeTracker.DetectChanges();

                foreach (var item in ChangeTracker.Entries())
                {
                    if (item.Entity is LogTransacoes || item.State is EntityState.Detached or EntityState.Unchanged)
                        continue;

                    if (item.State is EntityState.Modified)
                    {
                        Guid entidadeId;
                        Guid? usuarioId = _logServico.BuscarUsuarioId();

                        var entidadeIdObject = item.Properties.Where(c => c.Metadata.Name == nameof(EntidadeBase.Id)).FirstOrDefault()?.CurrentValue;
                        if (!usuarioId.HasValue || entidadeIdObject == null || !Guid.TryParse(entidadeIdObject.ToString(), out entidadeId))
                            continue;

                        foreach (var propriedade in item.Properties.Where(c => c.IsModified && c.Metadata.Name != nameof(EntidadeBase.Id) && c.Metadata.Name != nameof(EntidadeBase.Excluido)))
                        {
                            string valorAnterior = propriedade.OriginalValue == null ? string.Empty : propriedade.OriginalValue.ToString();
                            string valorAtual = propriedade.CurrentValue == null ? string.Empty : propriedade.CurrentValue.ToString();

                            if (propriedade.IsModified && valorAnterior != valorAtual)
                            {
                                var log = new LogTransacoes
                                {
                                    Comando = "UPDATE",
                                    Data = DateTime.Now,
                                    EntidadeId = entidadeId,
                                    UsuarioId = usuarioId,
                                    //ValorAnterior = valorAnterior,
                                    //ValorAtual = valorAtual,
                                    //CampoAlterado = propriedade.Metadata.Name,

                                };
                                await LogTransacoes.AddAsync(log);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {


            }
        }

    }
}