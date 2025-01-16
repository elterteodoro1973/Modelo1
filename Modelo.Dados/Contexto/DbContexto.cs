using Microsoft.EntityFrameworkCore;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Servicos;

namespace Modelo.Dados.Contexto
{
    public class DbContexto : DbContext
    {
        private readonly ILogServico _logServico;
        //public DbContexto(DbContextOptions<DbContexto> options, ILogServico logServico) : base(options)
        //{
        //    _logServico = logServico;
        //}

        public DbContexto(DbContextOptions<DbContexto> options) : base(options)
        {
            
        }

        public virtual DbSet<LogAcessos> LogAcessos { get; set; }
        public virtual DbSet<LogTransacoes> LogTransacoes { get; set; }
        public virtual DbSet<Perfis> Perfis { get; set; }
        public virtual DbSet<PermissoesPerfis> PermissoesPerfis { get; set; }
        public virtual DbSet<TipoAcao> TipoAcao { get; set; }
        public virtual DbSet<TipoAcesso> TipoAcesso { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //        //                optionsBuilder.UseSqlServer("Data Source=localhost\\SQL2019;Initial Catalog=Modelo;User ID=sa;Password=1478;Encrypt=False", x => x.UseHierarchyId());
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogAcessos>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.LogAcessos)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_LogAcessos_Usuarios");
            });

            modelBuilder.Entity<LogTransacoes>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.LogTransacoes)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_LogTransacoes_Usuarios");
            });

            modelBuilder.Entity<Perfis>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<PermissoesPerfis>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.PermissoesPerfis)
                    .HasForeignKey(d => d.PerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PermissoesPerfis_Perfil");

                entity.HasOne(d => d.TipoAcao)
                    .WithMany(p => p.PermissoesPerfis)
                    .HasForeignKey(d => d.TipoAcaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PermissoesPerfis_TipoAcao");

                entity.HasOne(d => d.TipoAcesso)
                    .WithMany(p => p.PermissoesPerfis)
                    .HasForeignKey(d => d.TipoAcessoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PermissoesPerfis_TipoAcesso");
            });

            modelBuilder.Entity<TipoAcao>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<TipoAcesso>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Excluido).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.PerfilId)
                    .HasConstraintName("FK_Usuarios_Perfis");
            });

            //OnModelCreatingGeneratedProcedures(modelBuilder);
            //OnModelCreatingGeneratedFunctions(modelBuilder);
            OnModelCreating(modelBuilder);
        }




        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}

        //public override int SaveChanges()
        //{

        //     SalvarLog().ConfigureAwait(false).GetAwaiter().GetResult();


        //    foreach (var item in ChangeTracker.Entries()
        //       .Where(e => e.State == EntityState.Deleted &&
        //       e.Metadata.GetProperties().Any(x => x.Name == "Excluido")))
        //    {
        //        item.State = EntityState.Unchanged;
        //        item.CurrentValues["Excluido"] = true;
        //    }
        //    return base.SaveChanges();

        //}
        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{

        //    await SalvarLog();



        //    foreach (var item in ChangeTracker.Entries()
        //      .Where(e => e.State == EntityState.Deleted &&
        //      e.Metadata.GetProperties().Any(x => x.Name == "Excluido")))
        //    {
        //        item.State = EntityState.Unchanged;
        //        item.CurrentValues["Excluido"] = true;
        //    }


        //    return await base.SaveChangesAsync(cancellationToken);
        //}

        //private async Task SalvarLog()
        //{
        //    try
        //    {
        //        ChangeTracker.DetectChanges();

        //        foreach (var item in ChangeTracker.Entries())
        //        {
        //            if (item.Entity is LogTransacoes || item.State is EntityState.Detached or EntityState.Unchanged)
        //                continue;

        //            if (item.State is EntityState.Modified)
        //            {
        //                Guid entidadeId;
        //                Guid? usuarioId = _logServico.BuscarUsuarioId();

        //                var entidadeIdObject = item.Properties.Where(c => c.Metadata.Name == nameof(EntidadeBase.Id)).FirstOrDefault()?.CurrentValue;
        //                if (!usuarioId.HasValue || entidadeIdObject == null || !Guid.TryParse(entidadeIdObject.ToString(), out entidadeId))
        //                    continue;

        //                foreach (var propriedade in item.Properties.Where(c => c.IsModified && c.Metadata.Name != nameof(EntidadeBase.Id) && c.Metadata.Name != nameof(EntidadeBase.Excluido)))
        //                {
        //                    string valorAnterior = propriedade.OriginalValue == null ? string.Empty : propriedade.OriginalValue.ToString();
        //                    string valorAtual = propriedade.CurrentValue == null ? string.Empty : propriedade.CurrentValue.ToString();

        //                    if (propriedade.IsModified && valorAnterior != valorAtual)
        //                    {
        //                        var log = new LogTransacoes
        //                        {
        //                            Comando = "UPDATE",
        //                            Data = DateTime.Now,
        //                            EntidadeId = entidadeId,
        //                            UsuarioId = usuarioId,
        //                            //ValorAnterior = valorAnterior,
        //                            //ValorAtual = valorAtual,
        //                            //CampoAlterado = propriedade.Metadata.Name,

        //                        };
        //                        await LogTransacoes.AddAsync(log);
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {

                
        //    }
        //}

    }
}