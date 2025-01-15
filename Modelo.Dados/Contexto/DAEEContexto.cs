using Microsoft.EntityFrameworkCore;
using Modelo.Dominio.Entidades;
using Modelo.Dominio.Interfaces.Servicos;

namespace Modelo.Dados.Contexto
{
    public class DAEEContexto : DbContext
    {
        private readonly ILogServico _logServico;
        public DAEEContexto(DbContextOptions<DAEEContexto> options, ILogServico logServico) : base(options)
        {
            _logServico = logServico;
        }

        
        public DbSet<Usuario> Usuarios { get; set; }
       
        public DbSet<PermissaoPerfil> PermissaoPerfils { get; set; }
        public DbSet<Perfil> Perfis { get; set; }
        
        public DbSet<Log> Logs { get; set; }
        

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
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
                    if (item.Entity is Log || item.State is EntityState.Detached or EntityState.Unchanged)
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
                                var log = new Log
                                {
                                    Comando = "UPDATE",
                                    Data = DateTime.Now,
                                    EntidadeId = entidadeId,
                                    UsuarioId = usuarioId,
                                    ValorAnterior = valorAnterior,
                                    ValorAtual = valorAtual,
                                    CampoAlterado = propriedade.Metadata.Name,

                                };
                                await Logs.AddAsync(log);
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