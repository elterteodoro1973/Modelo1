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
    public class PermissaoPerfisMapeamento : IEntityTypeConfiguration<PermissaoPerfil>
    {
        public void Configure(EntityTypeBuilder<PermissaoPerfil> builder)
        {
            builder.ToTable("PermissoesPerfis");


            builder.HasKey(r => r.Id);

            builder.Property(r => r.ClaimType).HasColumnName("Tipo");
            builder.Property(r => r.ClaimValue).HasColumnName("Valor");
            builder.Property(r => r.RoleId).HasColumnName("PerfilId");


        }
    }
}
