using Modelo.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Infraestrutura.Crosscutting.Identity.Mapeamentos
{
    public class PerfilMapeamento : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            builder.ToTable("Perfis");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql("newId()");

            builder.HasIndex(r => r.NormalizedName).HasDatabaseName("NomePerfilIndex").IsUnique();
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            builder.Property(p => p.Name).HasColumnName("Nome");
            builder.Property(p => p.NormalizedName).HasColumnName("NomeNormalizado");
            builder.Property(p => p.Descricao).HasColumnName("Descricao");
            builder.HasQueryFilter(c => !c.Excluido);

            builder.HasMany<PerfilCBHUsuario>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            builder.HasMany<PermissaoPerfil>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
        }
    }
}
