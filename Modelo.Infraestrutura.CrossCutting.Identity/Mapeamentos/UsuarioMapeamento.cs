using Modelo.Dominio.Entidades;
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
    public class UsuarioMapeamento : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            // Primary key
            builder.HasKey(u => u.Id);
            builder.Property(x => x.Id).HasDefaultValueSql("newId()");

            // Indexes for "normalized" username and email, to allow efficient lookups
            builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("NomeUsuarioIndex").IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");
            
            builder.Property(u => u.NomeCompleto).HasColumnName("NomeCompleto");
            
            builder.Property(u => u.UserName).HasColumnName("NomeUsuario");
            builder.Property(u => u.NormalizedUserName).HasColumnName("NomeUsuarioNormalizado");
            
            builder.Property(u => u.PhoneNumber).HasColumnName("NumeroTelefone");
            builder.Property(u => u.AccessFailedCount).HasColumnName("QuantidadeFalhasLogin");
            builder.Property(u => u.PhoneNumberConfirmed).HasColumnName("NumeroTelefoneConfirmado");
            builder.Property(u => u.EmailConfirmed).HasColumnName("EmailConfirmado");
            builder.Property(u => u.NormalizedEmail).HasColumnName("EmailNormalizado");
            builder.Property(u => u.PasswordHash).HasColumnName("SenhaHash");
            builder.Property(u => u.TwoFactorEnabled).HasColumnName("AutenticacaoEmDoisFatores");

            builder.HasMany<PerfilCBHUsuario>().WithOne().HasForeignKey(uc => uc.UserId)
               .IsRequired();
            builder.HasMany<PermissaoCBHUsuario>().WithOne().HasForeignKey(uc => uc.UserId)
                .IsRequired();
            builder.HasMany<LoginUsuario>().WithOne()
                .HasForeignKey(ul => ul.UserId).IsRequired();
            builder.HasMany<TokenUsuario>().WithOne().HasForeignKey(ut => ut.UserId)
                .IsRequired();
            builder.HasMany<EmailUsuario>().WithOne().HasForeignKey(ut => ut.UsuarioId)
                .IsRequired();
            builder.HasMany<EnderecoUsuario>().WithOne().HasForeignKey(ut => ut.UsuarioId)
                .IsRequired();
            builder.HasMany<TelefoneUsuario>().WithOne().HasForeignKey(ut => ut.UsuarioId)
                .IsRequired();
        }
    }
}
