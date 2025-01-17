using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Dominio.Entidades;

namespace Modelo.Dados.Mapeamentos
{
    public class TipoAcessoMapeamento : IEntityTypeConfiguration<TipoAcesso>
    {
        public void Configure(EntityTypeBuilder<TipoAcesso> builder)
        {
            builder.ToTable("TipoAcesso");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasDefaultValueSql("(newid())");
            builder.Property(c => c.Tipo).HasMaxLength(256).HasColumnName("Nome");
            builder.Property(c => c.Excluido).HasDefaultValue(false);
            
        }
    }
}
