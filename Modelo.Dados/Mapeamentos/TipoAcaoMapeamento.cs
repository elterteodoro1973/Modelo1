using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Dominio.Entidades;

namespace Modelo.Dados.Mapeamentos
{
    public class TipoAcaoMapeamento : IEntityTypeConfiguration<TipoAcao>
    {
        public void Configure(EntityTypeBuilder<TipoAcao> builder)
        {
            builder.ToTable("TipoAcao");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Acao).HasMaxLength(256).HasColumnName("Nome");            
            builder.Property(c => c.Excluido).HasDefaultValue(false);
            builder.HasQueryFilter(c => !c.Excluido);
        }
    }
}
