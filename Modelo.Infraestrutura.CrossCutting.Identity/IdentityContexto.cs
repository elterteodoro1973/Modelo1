using Modelo.Dominio.Entidades;
using Modelo.Dominio.Entidades.Identity;
using Modelo.Infraestrutura.Crosscutting.Identity.Mapeamentos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Infraestrutura.Crosscutting.Identity
{
    public class IdentityContexto : IdentityDbContext<Usuario, Perfil, Guid, PermissaoCBHUsuario, PerfilCBHUsuario, LoginUsuario, PermissaoPerfil, TokenUsuario>
    {
        public IdentityContexto(DbContextOptions<IdentityContexto> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<TelefoneUsuario> TelefoneUsuarios { get; set; }
        public DbSet<EmailUsuario> EmailUsuarios { get; set; }
        public DbSet<EnderecoUsuario> EnderecoUsuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UsuarioMapeamento());
            builder.ApplyConfiguration(new PerfilMapeamento());
            builder.ApplyConfiguration(new PermissaoPerfisMapeamento());
            builder.ApplyConfiguration(new PerfisCBHUsuariosMapeamento());
            builder.ApplyConfiguration(new LoginUsuariosMapeamento());
            builder.ApplyConfiguration(new TokenUsuariosMapeamento());
            builder.ApplyConfiguration(new PermissaoCBHUsuariosMapeamento());
            builder.ApplyConfiguration(new EnderecoUsuarioMapeamento());
            builder.ApplyConfiguration(new EmailUsuarioMapeamento());
            builder.ApplyConfiguration(new TelefoneUsuarioMapeamento());

            
        }

    }
}
