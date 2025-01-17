using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Dominio.Entidades;

namespace Modelo.Dados.Mapeamentos
{
    public class PerfisMapeamento : IEntityTypeConfiguration<Perfis>
    {
        public void Configure(EntityTypeBuilder<Perfis> builder)
        {
            builder.ToTable("Perfis");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasDefaultValueSql("(newid())");
            builder.Property(c => c.Nome).HasMaxLength(256).HasColumnName("Nome");
            builder.Property(c => c.Descricao).HasColumnName("Descricao");
            builder.Property(c => c.Administrador).HasDefaultValue(false);             
            builder.Property(c => c.Excluido).HasDefaultValue(false);

        }
    }
}
