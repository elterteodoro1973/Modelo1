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
    public class TokenUsuariosMapeamento : IEntityTypeConfiguration<TokenUsuario>
    {
        public void Configure(EntityTypeBuilder<TokenUsuario> builder)
        {
            builder.ToTable("TokensUsuarios");

            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            builder.Property(t => t.UserId).HasColumnName("UsuarioId");
            builder.Property(t => t.Name).HasColumnName("Nome");
            builder.Property(t => t.Value).HasColumnName("Valor");
        }
    }
}
