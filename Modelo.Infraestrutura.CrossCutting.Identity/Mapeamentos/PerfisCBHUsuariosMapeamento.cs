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
    public class PerfisCBHUsuariosMapeamento : IEntityTypeConfiguration<PerfilCBHUsuario>
    {
        public void Configure(EntityTypeBuilder<PerfilCBHUsuario> builder)
        {
            builder.ToTable("PerfisCBHUsuarios");

            builder.HasKey(u => new { u.UserId, u.RoleId , u.CBHUsuarioId });

            builder.Property(u => u.UserId).HasColumnName("UsuarioId");
            builder.Property(u => u.RoleId).HasColumnName("PerfilId");
            builder.HasQueryFilter(c => !c.Excluido);

            builder.HasOne(c => c.CBHUsuario).WithMany(c => c.PerfilCBHUsuarios).HasForeignKey(c => c.CBHUsuarioId)
                .HasConstraintName("FK_CBHUsuarios_PerfisUsuarios");
        }
    }
}
