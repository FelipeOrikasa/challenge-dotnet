using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models.Entities
{
    // Representa um sensor de localização ou de status em um pátio ou moto.
    public class Sensor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [MaxLength(150)]
        public required string Descricao { get; set; } // Usado no mapeamento de Localizacao

        [Required]
        public bool Ativo { get; set; }

        // Chave estrangeira para Patio
        public int PatioId { get; set; }

        // Propriedade de navegação para Patio (Usado para buscar o NomePatio)
        public required Patio Patio { get; set; }

        // Propriedade de navegação para Localizações
        public ICollection<Localizacao>? Localizacoes { get; set; }
    }
}