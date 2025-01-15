
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
    public class EmailUsuarioMapeamento : IEntityTypeConfiguration<EmailUsuario>
    {
        public void Configure(EntityTypeBuilder<EmailUsuario> builder)
        {
            builder.ToTable("EmailUsuarios");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Posicao).HasColumnName("Posicao");
            builder.Property(c => c.Posicao).HasColumnName("Posicao");
            builder.Property(c => c.Email).HasColumnName("Email");
            builder.Property(c => c.Excluido).HasColumnName("Excluido");

            builder.HasOne(c => c.Usuario)
                .WithMany(c => c.Emails)
                .HasForeignKey(c => c.UsuarioId)
                .HasConstraintName("FK_EmailUsuarios_Usuarios");

        }
    }
}
