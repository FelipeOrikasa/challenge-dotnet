
using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models.Entities
{
    // Representa uma moto no sistema.
    public class Moto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "A placa da moto é obrigatória")]
        [MaxLength(7)]
        public required string Placa { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório")]
        [MaxLength(100)]
        public required string Modelo { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório")]
        public int Ano { get; set; }

        // Chave estrangeira para Patio
        public int? PatioId { get; set; }

        // Propriedade de navegação para Patio (ESSENCIAL para o AutoMapper)
        // Usado no mapeamento: src.Patio.NomePatio
        public Patio? Patio { get; set; } 

        // Propriedade de navegação para Localizações
        public ICollection<Localizacao>? Localizacoes { get; set; }
    }
}