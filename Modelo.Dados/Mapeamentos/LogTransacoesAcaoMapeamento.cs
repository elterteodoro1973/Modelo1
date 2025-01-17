using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Dominio.Entidades;

namespace Modelo.Dados.Mapeamentos
{
    public class LogTransacoesMapeamento : IEntityTypeConfiguration<LogTransacoes>
    {
        public void Configure(EntityTypeBuilder<LogTransacoes> builder)
        {
            builder.ToTable("LogTransacoes");

            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(c => c.EntidadeId);
            builder.Property(c => c.Data);
            builder.Property(c => c.UsuarioId);
            builder.Property(c => c.Dados);
            builder.Property(c => c.Comando).HasMaxLength(50).HasColumnName("Comando"); ;

            builder.HasOne(d => d.Usuario).WithMany(p => p.LogTransacoes).HasConstraintName("FK_LogTransacoes_Usuarios");


        }
    }
}
