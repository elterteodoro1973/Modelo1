using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo.Dominio.Entidades;

namespace Modelo.Dados.Mapeamentos
{
    public class UsuarioMapeamento : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasDefaultValueSql("(newid())");
            builder.Property(c => c.NomeCompleto).HasMaxLength(256).HasColumnName("NomeCompleto");
            builder.Property(c => c.CPF).HasMaxLength(20).HasColumnName("CPF");
            builder.Property(c => c.Email).HasMaxLength(512);            
            builder.Property(c => c.Senha);
            builder.Property(c => c.Excluido).HasDefaultValue(false);
            builder.Property(c => c.Inativo).HasDefaultValue(false);           
            //builder.Property(c => c.PerfilId);
            //builder.HasOne(d => d.Perfil).WithMany(p => p.Usuarios).HasConstraintName("FK_Usuarios_Perfis");

        }
    }
}
