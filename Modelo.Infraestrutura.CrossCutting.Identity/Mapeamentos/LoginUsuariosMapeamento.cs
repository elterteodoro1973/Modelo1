using Modelo.Dominio.Entidades.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Infraestrutura.Crosscutting.Identity.Mapeamentos
{
    public class LoginUsuariosMapeamento : IEntityTypeConfiguration<LoginUsuario>
    {
        public void Configure(EntityTypeBuilder<LoginUsuario> builder)
        {
            builder.ToTable("LoginUsuarios");

            builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            builder.Property(l => l.UserId).HasColumnName("UsuarioId");
        }
    }
}
