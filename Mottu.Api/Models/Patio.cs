
using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models.Entities
{
    // Representa um pátio de armazenamento dentro de uma filial.
    public class Patio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do pátio é obrigatório")]
        [MaxLength(100)]
        public required string NomePatio { get; set; } // Propriedade que o AutoMapper busca (no mapeamento de Localizacao/Sensor).

        [Required(ErrorMessage = "A capacidade é obrigatória")]
        public int CapacidadeMaxima { get; set; }

        // Chave estrangeira para Filial
        public int FilialId { get; set; }

        // Propriedade de navegação para Filial (ESSENCIAL para o AutoMapper)
        public required Filial Filial { get; set; }

        // Propriedades de navegação para Moto e Sensor
        public ICollection<Moto>? Motos { get; set; }
        public ICollection<Sensor>? Sensores { get; set; }
    }
}