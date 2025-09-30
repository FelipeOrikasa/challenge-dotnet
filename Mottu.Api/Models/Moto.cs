using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mottu.Api.Models
{
    /// <summary>
    /// Representa uma moto da frota da empresa.
    /// Corresponde à tabela 'Moto' no banco de dados.
    /// </summary>
    public class Moto
    {
        /// <summary>
        /// Identificador único da moto (Chave Primária).
        /// </summary>
        /// <example>501</example>
        [Key]
        public int MotoId { get; set; }

        /// <summary>
        /// Placa da moto. Este campo deve ser único.
        /// A restrição de unicidade será configurada no AppDbContext.
        /// </summary>
        /// <example>BRA2E19</example>
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(20, ErrorMessage = "A placa não pode exceder 20 caracteres.")]
        public string Placa { get; set; } = null!;

        /// <summary>
        /// Modelo da moto.
        /// </summary>
        /// <example>Honda CB 300F Twister</example>
        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O modelo não pode exceder 100 caracteres.")]
        public string Modelo { get; set; } = null!;

        /// <summary>
        /// Ano de fabricação da moto.
        /// </summary>
        /// <example>2024</example>
        [Required(ErrorMessage = "O ano é obrigatório.")]
        public int Ano { get; set; }

        // --- Relacionamento com Patio (Muitos-para-Um) ---

        /// <summary>
        /// Chave estrangeira que referencia o Pátio onde a moto está (ou foi vista por último).
        /// </summary>
        /// <example>10</example>
        public int PatioId { get; set; }

        /// <summary>
        /// Propriedade de navegação para o Pátio.
        /// </summary>
        [ForeignKey("PatioId")]
        public Patio Patio { get; set; } = null!;
        // --- Relacionamento com Localizacao (Um-para-Muitos) ---

        /// <summary>
        /// Histórico de localizações registradas para esta moto.
        /// </summary>
        public ICollection<Localizacao> Localizacoes { get; set; } = new List<Localizacao>();
    }
}