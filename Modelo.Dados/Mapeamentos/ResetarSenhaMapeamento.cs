using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Dominio.Entidades;

namespace Modelo.Dados.Mapeamentos
{
    public class ResetarSenhaMapeamento : IEntityTypeConfiguration<ResetarSenha>
    {
        public void Configure(EntityTypeBuilder<ResetarSenha> builder)
        {
            builder.ToTable("ResetarSenha");

            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Token);
            builder.Property(e => e.UsuarioId);
            builder.Property(e => e.DataSolicitacao).HasDefaultValueSql("(getdate())");
            builder.Property(e => e.DataExpiracao);
            builder.Property(e => e.Efetivado).HasDefaultValue(false);
            builder.Property(e => e.Excluido).HasDefaultValue(false);

            builder.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ResetarSenha)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResetarSenha_Usuarios");

        }
    }
}

