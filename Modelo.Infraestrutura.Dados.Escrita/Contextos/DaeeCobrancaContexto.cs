using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Modelo.Dominio.Modelos;

namespace Modelo.Infraestrutura.Dados.Escrita.Contextos
{
    public partial class DaeeCobrancaContexto : DbContext
    {
        public DaeeCobrancaContexto()
        {
        }

        public DaeeCobrancaContexto(DbContextOptions<DaeeCobrancaContexto> options)
            : base(options)
        {
        }

        public virtual DbSet<Aquifero> Aquiferos { get; set; } = null!;
        public virtual DbSet<Cbh> Cbhs { get; set; } = null!;
        public virtual DbSet<ClasseReceptorLancamentoSuperficial> ClasseReceptorLancamentoSuperficials { get; set; } = null!;
        public virtual DbSet<ClasseRioCaptacaoSuperficial> ClasseRioCaptacaoSuperficials { get; set; } = null!;
        public virtual DbSet<CursoDagua> CursoDaguas { get; set; } = null!;
        public virtual DbSet<DadoComplementar> DadosComplementares { get; set; }
        public virtual DbSet<DisponibilidadeHidricaCaptacaoSuperficial> DisponibilidadeHidricaCaptacaoSuperficials { get; set; } = null!;
        public virtual DbSet<Dominalidade> Dominalidades { get; set; } = null!;
        public virtual DbSet<Empreendimento> Empreendimentos { get; set; } = null!;
        public virtual DbSet<EmpreendimentoAjuste> EmpreendimentosAjustes { get; set; }
        public virtual DbSet<Endereco> Enderecos { get; set; } = null!;
        public virtual DbSet<FinalidadeCbh> FinalidadesCbhs { get; set; } = null!;
        public virtual DbSet<FinalidadeGlobal> FinalidadesGlobals { get; set; } = null!;
        public virtual DbSet<FinalidadesUsosEmpreendimento> FinalidadesUsosEmpreendimentos { get; set; } = null!;
        public virtual DbSet<Municipio> Municipios { get; set; } = null!;
        public virtual DbSet<Observacao> Observacoes { get; set; } = null!;
        public virtual DbSet<RepresentanteEmpreendimento> RepresentanteEmpreendimentos { get; set; } = null!;
        public virtual DbSet<ResponsavelEmpreendimento> ResponsavelEmpreendimentos { get; set; } = null!;
        public virtual DbSet<SchemaVersion> SchemaVersions { get; set; } = null!;
        public virtual DbSet<SituacaoEmpreendimento> SituacaoEmpreendimentos { get; set; } = null!;
        public virtual DbSet<Telefone> Telefones { get; set; } = null!;
        public virtual DbSet<TipoAjuste> TiposAjustes { get; set; } = null!;
        public virtual DbSet<TiposDocumento> TiposDocumentos { get; set; } = null!;
        public virtual DbSet<TiposEmpreendimento> TiposEmpreendimentos { get; set; } = null!;
        public virtual DbSet<TiposTratamentoLancamentoSuperficial> TiposTratamentoLancamentoSuperficials { get; set; } = null!;
        public virtual DbSet<TiposUso> TiposUsos { get; set; } = null!;
        public virtual DbSet<TransposicaoBaciaCaptacaoSuperficial> TransposicaoBaciaCaptacaoSuperficials { get; set; } = null!;
        public virtual DbSet<Ugrhi> Ugrhis { get; set; } = null!;
        public virtual DbSet<UsosCaptacaoSubterranea> UsosCaptacaoSubterraneas { get; set; } = null!;
        public virtual DbSet<UsosCaptacaoSuperficial> UsosCaptacaoSuperficials { get; set; } = null!;
        public virtual DbSet<UsosEmpreendimento> UsosEmpreendimentos { get; set; } = null!;
        public virtual DbSet<UsosLancamentoSuperficial> UsosLancamentoSuperficials { get; set; } = null!;
        public virtual DbSet<UsosPortaria> UsosPortarias { get; set; } = null!;
        public virtual DbSet<ValoresOutorgadosUso> ValoresOutorgadosUsos { get; set; } = null!;
        public virtual DbSet<VolumePrevistoMedidoUso> VolumePrevistoMedidoUsos { get; set; } = null!;
        public virtual DbSet<ValorUfesp> ValoresUfesp { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=10.200.142.142;Database=dbDAEEweb;User Id=usudaee;Password=pwdaeeuser;Application Name=DAEE");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Aquifero>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cbh>(entity =>
            {
                entity.ToTable("CBHs");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Calculo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo).ValueGeneratedOnAdd();

                entity.Property(e => e.CoeficienteVazaoVolumeMedido).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.CoeficienteVazaoVolumeMedidoMenor).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.CoeficienteVazaoVolumeOutorgado).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.CoeficienteVazaoVolumeOutorgadoMenor).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.DescontoRegressivo).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrecoUnitarioBasicoCaptacao).HasColumnType("decimal(4, 3)");

                entity.Property(e => e.PrecoUnitarioBasicoConsumo).HasColumnType("decimal(4, 3)");

                entity.Property(e => e.PrecoUnitarioBasicoLancamento).HasColumnType("decimal(4, 3)");

                entity.Property(e => e.Qareia).HasColumnName("QAreia");

                entity.Property(e => e.Sigla)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMinimoParaCobranca).HasColumnType("decimal(9, 2)");
            });

            modelBuilder.Entity<ClasseReceptorLancamentoSuperficial>(entity =>
            {
                entity.ToTable("ClasseReceptorLancamentoSuperficial");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClasseRioCaptacaoSuperficial>(entity =>
            {
                entity.ToTable("ClasseRioCaptacaoSuperficial");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CursoDagua>(entity =>
            {
                entity.ToTable("CursoDAgua");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DisponibilidadeHidricaCaptacaoSuperficial>(entity =>
            {
                entity.ToTable("DisponibilidadeHidricaCaptacaoSuperficial");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Dominalidade>(entity =>
            {
                entity.ToTable("Dominalidade");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Empreendimento>(entity =>
            {
                entity.HasIndex(e => e.Id, "IX_Empreendimentos");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).ValueGeneratedOnAdd();

                entity.Property(e => e.NomeFantasia)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroDocumento)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RazaoSocial)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cbh)
                    .WithMany(p => p.Empreendimentos)
                    .HasForeignKey(d => d.CbhId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empreendi__CbhId__387A3A7B");

                entity.HasOne(d => d.EnderecoCorrespondencia)
                    .WithMany(p => p.EmpreendimentoEnderecoCorrespondencia)
                    .HasForeignKey(d => d.EnderecoCorrespondenciaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empreendi__Ender__3A6282ED");

                entity.HasOne(d => d.Endereco)
                    .WithMany(p => p.EmpreendimentoEnderecos)
                    .HasForeignKey(d => d.EnderecoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empreendi__Ender__396E5EB4");

                entity.HasOne(d => d.Situacao)
                    .WithMany(p => p.Empreendimentos)
                    .HasForeignKey(d => d.SituacaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empreendi__Situa__58E70A0D");

                entity.HasOne(d => d.TipoDocumento)
                    .WithMany(p => p.Empreendimentos)
                    .HasForeignKey(d => d.TipoDocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empreendi__TipoD__45D43599");

                entity.HasOne(d => d.TipoEmpreendimento)
                    .WithMany(p => p.Empreendimentos)
                    .HasForeignKey(d => d.TipoEmpreendimentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Empreendi__TipoE__3B56A726");

                entity.HasMany(d => d.Observacaos)
                    .WithMany(p => p.Empreendimentos)
                    .UsingEntity<Dictionary<string, object>>(
                        "ObservacoesEmpreendimento",
                        l => l.HasOne<Observacao>().WithMany().HasForeignKey("ObservacaoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Observaco__Obser__25676607"),
                        r => r.HasOne<Empreendimento>().WithMany().HasForeignKey("EmpreendimentoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Observaco__Empre__247341CE"),
                        j =>
                        {
                            j.HasKey("EmpreendimentoId", "ObservacaoId").HasName("PK_Observacao_Empreendimento");

                            j.ToTable("ObservacoesEmpreendimentos");
                        });
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Bairro)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CaixaPostal)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Cep)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Cidade)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Logradouro)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Uf)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<FinalidadeCbh>(entity =>
            {
                entity.ToTable("FinalidadesCBH");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Qareia).HasColumnName("QAreia");

                entity.HasOne(d => d.Cbh)
                    .WithMany(p => p.Finalidades)
                    .HasForeignKey(d => d.CbhId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Finalidades_CBHs");

                entity.HasOne(d => d.FinalidadeGlobal)
                    .WithMany(p => p.FinalidadesCbhs)
                    .HasForeignKey(d => d.FinalidadeGlobalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Finalidades_FinalidadesGlobal");
            });

            modelBuilder.Entity<FinalidadeGlobal>(entity =>
            {
                entity.ToTable("FinalidadesGlobal");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Qareia).HasColumnName("QAreia");
            });

            modelBuilder.Entity<FinalidadesUsosEmpreendimento>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Percentual).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Finalidade)
                    .WithMany(p => p.FinalidadesUsosEmpreendimentos)
                    .HasForeignKey(d => d.FinalidadeId)
                    .HasConstraintName("FK__Finalidad__Final__2EBBC617");

                entity.HasOne(d => d.UsoEmpreendimento)
                    .WithMany(p => p.FinalidadesUsosEmpreendimentos)
                    .HasForeignKey(d => d.UsoEmpreendimentoId)
                    .HasConstraintName("FK__Finalidad__UsoEm__2DC7A1DE");
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao).HasMaxLength(50);

                entity.Property(e => e.Uf)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("UF")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Observacao>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao).IsUnicode(false);
            });

            modelBuilder.Entity<RepresentanteEmpreendimento>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Empreendimento)
                    .WithMany(p => p.RepresentanteEmpreendimentos)
                    .HasForeignKey(d => d.EmpreendimentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Represent__Empre__265B8A40");
            });

            modelBuilder.Entity<ResponsavelEmpreendimento>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Cpf)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Empreendimento)
                    .WithMany(p => p.ResponsavelEmpreendimentos)
                    .HasForeignKey(d => d.EmpreendimentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Responsav__Empre__274FAE79");
            });

            modelBuilder.Entity<SchemaVersion>(entity =>
            {
                entity.HasKey(e => e.InstalledRank)
                    .HasName("schema_version_pk");

                entity.ToTable("schema_version");

                entity.HasIndex(e => e.Success, "schema_version_s_idx");

                entity.Property(e => e.InstalledRank)
                    .ValueGeneratedNever()
                    .HasColumnName("installed_rank");

                entity.Property(e => e.Checksum).HasColumnName("checksum");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.ExecutionTime).HasColumnName("execution_time");

                entity.Property(e => e.InstalledBy)
                    .HasMaxLength(100)
                    .HasColumnName("installed_by");

                entity.Property(e => e.InstalledOn)
                    .HasColumnType("datetime")
                    .HasColumnName("installed_on")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Script)
                    .HasMaxLength(1000)
                    .HasColumnName("script");

                entity.Property(e => e.Success).HasColumnName("success");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .HasColumnName("type");

                entity.Property(e => e.Version)
                    .HasMaxLength(50)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<SituacaoEmpreendimento>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Telefone>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Numero)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Ramal)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasMany(d => d.Enderecos)
                    .WithMany(p => p.Telefones)
                    .UsingEntity<Dictionary<string, object>>(
                        "TelefonesEndereco",
                        l => l.HasOne<Endereco>().WithMany().HasForeignKey("EnderecosId").HasConstraintName("FK__Telefones__Ender__6FCA6F65"),
                        r => r.HasOne<Telefone>().WithMany().HasForeignKey("TelefoneId").HasConstraintName("FK__Telefones__Telef__70BE939E"),
                        j =>
                        {
                            j.HasKey("TelefoneId", "EnderecosId").HasName("PK_Telefone_Endereco");

                            j.ToTable("TelefonesEnderecos");
                        });

                entity.HasMany(d => d.Representantes)
                    .WithMany(p => p.Telefones)
                    .UsingEntity<Dictionary<string, object>>(
                        "TelefonesRepresentante",
                        l => l.HasOne<RepresentanteEmpreendimento>().WithMany().HasForeignKey("RepresentanteId").HasConstraintName("FK__Telefones__Repre__6DE226F3"),
                        r => r.HasOne<Telefone>().WithMany().HasForeignKey("TelefoneId").HasConstraintName("FK__Telefones__Telef__6ED64B2C"),
                        j =>
                        {
                            j.HasKey("TelefoneId", "RepresentanteId").HasName("PK_Telefone_Representante");

                            j.ToTable("TelefonesRepresentantes");
                        });
            });

            modelBuilder.Entity<TiposDocumento>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposEmpreendimento>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposTratamentoLancamentoSuperficial>(entity =>
            {
                entity.ToTable("TiposTratamentoLancamentoSuperficial");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposUso>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransposicaoBaciaCaptacaoSuperficial>(entity =>
            {
                entity.ToTable("TransposicaoBaciaCaptacaoSuperficial");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ugrhi>(entity =>
            {
                entity.ToTable("UGRHI");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsosCaptacaoSubterranea>(entity =>
            {
                entity.ToTable("UsosCaptacaoSubterranea");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Aquiferos)
                    .WithMany(p => p.UsosCaptacaoSubterraneas)
                    .HasForeignKey(d => d.AquiferosId)
                    .HasConstraintName("FK__UsosCapta__Aquif__6EA14102");

                entity.HasOne(d => d.UsoEmpreendimento)
                    .WithMany(p => p.UsosCaptacaoSubterraneas)
                    .HasForeignKey(d => d.UsoEmpreendimentoId)
                    .HasConstraintName("FK__UsosCapta__UsoEm__6DAD1CC9");
            });

            modelBuilder.Entity<UsosCaptacaoSuperficial>(entity =>
            {
                entity.ToTable("UsosCaptacaoSuperficial");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.ClasseRio)
                    .WithMany(p => p.UsosCaptacaoSuperficials)
                    .HasForeignKey(d => d.ClasseRioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsoCaptacaoSuperficial_ClasseRioCaptacaoSuperficial");

                entity.HasOne(d => d.CursoDagua)
                    .WithMany(p => p.UsosCaptacaoSuperficials)
                    .HasForeignKey(d => d.CursoDaguaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsoCaptacaoSuperficial_CursoDagua");

                entity.HasOne(d => d.DisponibilidadeHidrica)
                    .WithMany(p => p.UsosCaptacaoSuperficials)
                    .HasForeignKey(d => d.DisponibilidadeHidricaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsoCaptacaoSuperficial_DisponibilidadeHidricaCaptacaoSuperficial");

                entity.HasOne(d => d.Dominalidade)
                    .WithMany(p => p.UsosCaptacaoSuperficials)
                    .HasForeignKey(d => d.DominalidadeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsoCaptacaoSuperficial_Dominalidade");

                entity.HasOne(d => d.TransposicaoBacia)
                    .WithMany(p => p.UsosCaptacaoSuperficials)
                    .HasForeignKey(d => d.TransposicaoBaciaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsoCaptacaoSuperficial_TransposicaoBaciaCaptacaoSuperficial");

                entity.HasOne(d => d.UsoEmpreendimento)
                    .WithMany(p => p.UsosCaptacaoSuperficials)
                    .HasForeignKey(d => d.UsoEmpreendimentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsoCaptacaoSuperficial_UsosEmpreendimentos");
            });

            modelBuilder.Entity<UsosEmpreendimento>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Codigo).ValueGeneratedOnAdd();

                entity.Property(e => e.CoordenadaLatitudadeSegundo).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.CoordenadaLongitudeSegundo).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.CoordenadaMeridianoCentral)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CoordenadaUtmleste)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("CoordenadaUTMLeste");

                entity.Property(e => e.CoordenadaUtmnorte)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("CoordenadaUTMNorte");

                entity.Property(e => e.Descricao).IsUnicode(false);

                entity.Property(e => e.Ugrhiid).HasColumnName("UGRHIId");

                entity.HasOne(d => d.Empreendimento)
                    .WithMany(p => p.UsosEmpreendimentos)
                    .HasForeignKey(d => d.EmpreendimentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsosEmpreendimentos_Empreendimentos");

                entity.HasOne(d => d.TipoUso)
                    .WithMany(p => p.UsosEmpreendimentos)
                    .HasForeignKey(d => d.TipoUsoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsosEmpreendimentos_TiposUsos");

                entity.HasOne(d => d.Ugrhi)
                    .WithMany(p => p.UsosEmpreendimentos)
                    .HasForeignKey(d => d.Ugrhiid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsosEmpreendimentos_UGRHI");
            });

            modelBuilder.Entity<UsosLancamentoSuperficial>(entity =>
            {
                entity.ToTable("UsosLancamentoSuperficial");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ConcentracaoDbo)
                    .HasColumnType("decimal(11, 2)")
                    .HasColumnName("ConcentracaoDBO");

                entity.Property(e => e.NumeroCetesb1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NumeroCETESB1");

                entity.Property(e => e.NumeroCetesb2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NumeroCETESB2");

                entity.Property(e => e.PorcentagemEficiencia).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.VazaoTratada).HasColumnType("decimal(11, 2)");

                entity.HasOne(d => d.ClasseReceptor)
                    .WithMany(p => p.UsosLancamentoSuperficials)
                    .HasForeignKey(d => d.ClasseReceptorId)
                    .HasConstraintName("FK__UsosLanca__Class__77368703");

                entity.HasOne(d => d.CursoDagua)
                    .WithMany(p => p.UsosLancamentoSuperficials)
                    .HasForeignKey(d => d.CursoDaguaId)
                    .HasConstraintName("FK__UsosLanca__Curso__764262CA");

                entity.HasOne(d => d.Dominalidade)
                    .WithMany(p => p.UsosLancamentoSuperficials)
                    .HasForeignKey(d => d.DominalidadeId)
                    .HasConstraintName("FK__UsosLanca__Domin__782AAB3C");

                entity.HasOne(d => d.TipoTratamento)
                    .WithMany(p => p.UsosLancamentoSuperficials)
                    .HasForeignKey(d => d.TipoTratamentoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__UsosLanca__TipoT__791ECF75");

                entity.HasOne(d => d.UsoEmpreendimento)
                    .WithMany(p => p.UsosLancamentoSuperficials)
                    .HasForeignKey(d => d.UsoEmpreendimentoId)
                    .HasConstraintName("FK__UsosLanca__UsoEm__754E3E91");
            });

            modelBuilder.Entity<UsosPortaria>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DataPortaria).HasColumnType("datetime");

                entity.Property(e => e.DataVencimento).HasColumnType("datetime");

                entity.Property(e => e.Ordem).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Uso)
                    .WithMany(p => p.UsosPortaria)
                    .HasForeignKey(d => d.UsoId)
                    .HasConstraintName("FK__UsosPorta__UsoId__3939548A");
            });

            modelBuilder.Entity<ValoresOutorgadosUso>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.HorasDia).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.Vazao).HasColumnType("decimal(11, 2)");

                entity.HasOne(d => d.UsoEmpreendimento)
                    .WithMany(p => p.ValoresOutorgadosUsos)
                    .HasForeignKey(d => d.UsoEmpreendimentoId)
                    .HasConstraintName("FK__ValoresOu__UsoEm__7ED7A8CB");
            });

            modelBuilder.Entity<VolumePrevistoMedidoUso>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Medido).HasColumnType("decimal(11, 2)");

                entity.Property(e => e.Previsto).HasColumnType("decimal(11, 2)");

                entity.HasOne(d => d.UsoEmpreendimento)
                    .WithMany(p => p.VolumePrevistoMedidoUsos)
                    .HasForeignKey(d => d.UsoEmpreendimentoId)
                    .HasConstraintName("FK__VolumePre__UsoEm__01B41576");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
