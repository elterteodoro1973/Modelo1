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
    public class EnderecoUsuarioMapeamento : IEntityTypeConfiguration<EnderecoUsuario>
    {
        public void Configure(EntityTypeBuilder<EnderecoUsuario> builder)
        {
            builder.ToTable("EnderecosUsuarios");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Posicao).HasColumnName("Posicao");
            builder.Property(c => c.Logradouro).HasColumnName("Logradouro");
            builder.Property(c => c.Numero).HasColumnName("Numero");
            builder.Property(c => c.Complemento).HasColumnName("Complemento");
            builder.Property(c => c.Municipio).HasColumnName("Municipio");
            builder.Property(c => c.Estado).HasColumnName("Estado");
            builder.Property(c => c.CEP).HasColumnName("CEP");

            builder.HasOne(c => c.Usuario)
                .WithMany(c => c.Enderecos)
                .HasForeignKey(c => c.UsuarioId)
                .HasConstraintName("FK_EnderecosUsuarios_Usuarios");
        }
    }
}
