using Microsoft.EntityFrameworkCore;
using Mottu.Api.Models.Entities;

namespace Mottu.Api.Data
{
    /// <summary>
    /// Contexto do banco de dados da aplicação, configurando todas as entidades.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Domínio de Logística/Tracking
        public DbSet<Filial> Filiais { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Sensor> Sensores { get; set; }
        public DbSet<Localizacao> Localizacoes { get; set; }

        // Domínio de Aluguel
        public DbSet<Moto> Motos { get; set; }
        public DbSet<Entregador> Entregadores { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Configurações de Entidades de Aluguel ---

            // Configuração da Moto
            modelBuilder.Entity<Moto>()
                .HasIndex(m => m.Placa).IsUnique();
            modelBuilder.Entity<Moto>()
                .Property(m => m.Placa).HasMaxLength(10).IsRequired();
            modelBuilder.Entity<Moto>()
                .Property(m => m.Modelo).HasMaxLength(100).IsRequired();
            
            // Relacionamento Moto -> Patio (N:1)
            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Patio)
                .WithMany(p => p.Motos)
                .HasForeignKey(m => m.PatioId)
                .OnDelete(DeleteBehavior.SetNull);


            // Configuração do Entregador
            modelBuilder.Entity<Entregador>()
                .HasIndex(e => e.CNPJ).IsUnique();
            modelBuilder.Entity<Entregador>()
                .HasIndex(e => e.CNH).IsUnique();
            modelBuilder.Entity<Entregador>()
                .Property(e => e.TipoCNH).HasMaxLength(2).IsRequired(); // CNH tipo A ou AB
            modelBuilder.Entity<Entregador>()
                .Property(e => e.CNPJ).HasMaxLength(18).IsRequired();
            modelBuilder.Entity<Entregador>()
                .Property(e => e.CNH).HasMaxLength(20).IsRequired();

            // Configuração da Locação e seus relacionamentos
            modelBuilder.Entity<Locacao>()
                .Property(l => l.CustoDiarioContratado).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Locacao>()
                .Property(l => l.CustoTotalPrevisto).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Locacao>()
                .Property(l => l.CustoFinal).HasColumnType("decimal(18, 2)");

            // Relacionamento Locacao -> Entregador (1:N)
            modelBuilder.Entity<Locacao>()
                .HasOne(l => l.Entregador)
                .WithMany(e => e.Locacoes)
                .HasForeignKey(l => l.EntregadorId)
                .IsRequired();

            // Relacionamento Locacao -> Moto (1:N)
            modelBuilder.Entity<Locacao>()
                .HasOne(l => l.Moto)
                .WithMany()
                .HasForeignKey(l => l.MotoId)
                .IsRequired();
            
            // Nota: Moto não tem propriedade de navegação Locacoes no modelo atual
            
            // --- Configurações de Entidades de Logística/Tracking ---
            
            // CNPJ removido - não existe no modelo Filial

            modelBuilder.Entity<Patio>()
                .Property(p => p.CapacidadeMaxima).IsRequired();
            modelBuilder.Entity<Patio>()
                .HasOne(p => p.Filial)
                .WithMany(f => f.Patios)
                .HasForeignKey(p => p.FilialId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração do Sensor
            modelBuilder.Entity<Sensor>()
                .HasOne(s => s.Patio)
                .WithMany(p => p.Sensores)
                .HasForeignKey(s => s.PatioId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Configuração de booleanos para Oracle (converter para NUMBER(1))
            modelBuilder.Entity<Sensor>()
                .Property(s => s.Ativo)
                .HasConversion<int>(); // Converte bool para int (0 ou 1) para compatibilidade com Oracle

            // Configuração da Localizacao
            modelBuilder.Entity<Localizacao>()
                .Property(l => l.Latitude).HasColumnType("decimal(10, 8)");
            modelBuilder.Entity<Localizacao>()
                .Property(l => l.Longitude).HasColumnType("decimal(11, 8)");
            
            // Relacionamento Localizacao -> Sensor (N:1)
            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Sensor)
                .WithMany(s => s.Localizacoes)
                .HasForeignKey(l => l.SensorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}