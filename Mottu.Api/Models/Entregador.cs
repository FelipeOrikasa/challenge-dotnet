using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models.Entities
{
    // Representa o entregador que aluga as motos.
    public class Entregador
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(150)]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [MaxLength(18)]
        public required string CNPJ { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O número da CNH é obrigatório")]
        [MaxLength(20)]
        public required string CNH { get; set; }

        [Required(ErrorMessage = "O tipo da CNH é obrigatório (A ou AB)")]
        [MaxLength(2)]
        public required string TipoCNH { get; set; } // Ex: "A", "AB"

        [Required(ErrorMessage = "O caminho da imagem da CNH é obrigatório")]
        public required string ImagemCNH { get; set; }

        // Propriedade de navegação para Locações (1:N)
        public ICollection<Locacao>? Locacoes { get; set; }
    }
}