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
    public class TelefoneUsuarioMapeamento : IEntityTypeConfiguration<TelefoneUsuario>
    {
        public void Configure(EntityTypeBuilder<TelefoneUsuario> builder)
        {
            builder.ToTable("TelefoneUsuarios");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Posicao).HasColumnName("Posicao");
            builder.Property(c => c.NumeroTelefone).HasColumnName("Numero");
            builder.Property(c => c.Ramal).HasColumnName("Ramal");

            builder.HasOne(c => c.Usuario)
                .WithMany(c => c.Telefones)
                .HasForeignKey(c => c.UsuarioId)
                .HasConstraintName("FK_TelefoneUsuarios_Usuarios");
        }
    }
}
