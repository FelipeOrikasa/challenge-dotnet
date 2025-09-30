using Microsoft.EntityFrameworkCore;
using Mottu.Api.Models;

namespace Mottu.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Cada DbSet representa uma tabela no banco de dados
        public DbSet<Filial> Filiais { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Sensor> Sensores { get; set; }
        public DbSet<Moto> Motos { get; set; }
        public DbSet<Localizacao> Localizacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Configuração da Entidade Moto ---
            // Adiciona a restrição UNIQUE na coluna Placa, conforme o script SQL.
            modelBuilder.Entity<Moto>()
                .HasIndex(m => m.Placa)
                .IsUnique();

            // --- Configuração dos Relacionamentos (Comportamento de Deleção) ---

            // Impede que uma Moto seja deletada se houver registros de Localizacao associados a ela.
            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Moto)
                .WithMany(m => m.Localizacoes)
                .HasForeignKey(l => l.MotoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Impede que um Sensor seja deletado se houver registros de Localizacao associados a ele.
            modelBuilder.Entity<Localizacao>()
                .HasOne(l => l.Sensor)
                .WithMany(s => s.Localizacoes)
                .HasForeignKey(l => l.SensorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}