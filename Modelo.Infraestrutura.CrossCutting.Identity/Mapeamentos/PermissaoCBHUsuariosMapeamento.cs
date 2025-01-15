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
    public class PermissaoCBHUsuariosMapeamento : IEntityTypeConfiguration<PermissaoCBHUsuario>
    {
        public void Configure(EntityTypeBuilder<PermissaoCBHUsuario> builder)
        {
            builder.ToTable("PermissoesCBHUsuarios");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ClaimType).HasColumnName("Tipo");
            builder.Property(p => p.ClaimValue).HasColumnName("Valor");
            builder.Property(p => p.UserId).HasColumnName("UsuarioId");
            builder.HasQueryFilter(c => !c.Excluido);

            builder.HasOne(p => p.CBHUsuario).WithMany(c => c.PermissaoCBHUsuarios).HasForeignKey(c => c.CBHUsuarioId).HasConstraintName("FK_CBHUsuarios_PermissoesUsuarios"); 
        }
    }
}
