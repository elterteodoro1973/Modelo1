using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Dominio.Entidades;

namespace DAEE.Cobranca.Dados.Mapeamentos
{
    public class PermissaoPerfilMapeamento : IEntityTypeConfiguration<PermissoesPerfis>
    {
        public void Configure(EntityTypeBuilder<PermissoesPerfis> builder)
        {
            builder.ToTable("PermissoesPerfis");

            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(c => c.PerfilId);
            builder.Property(c => c.TipoAcessoId);            
            builder.Property(c => c.Excluido).HasDefaultValue(false);
            builder.HasQueryFilter(c => !c.Excluido.Value);

            builder.HasOne(d => d.Perfil).WithMany(p => p.PermissoesPerfis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PermissoesPerfis_Perfil");

            builder.HasOne(d => d.TipoAcao).WithMany(p => p.PermissoesPerfis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PermissoesPerfis_TipoAcao");

            builder.HasOne(d => d.TipoAcesso).WithMany(p => p.PermissoesPerfis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PermissoesPerfis_TipoAcesso");
        }
    }
}
