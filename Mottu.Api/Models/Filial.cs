using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models.Entities
{
    // Representa a filial da Mottu.
    public class Filial
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da filial é obrigatório")]
        [MaxLength(100)]
        public required string NomeFilial { get; set; } // Propriedade que o AutoMapper busca.

        [Required(ErrorMessage = "O endereço é obrigatório")]
        [MaxLength(200)]
        public required string Endereco { get; set; }

        // Propriedade de navegação para Pátios
        public ICollection<Patio>? Patios { get; set; }
    }
}