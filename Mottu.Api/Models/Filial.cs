using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models
{
    /// <summary>
    /// Representa a entidade Filial, a unidade de negócio principal.
    /// Corresponde à tabela 'Filial' no banco de dados.
    /// </summary>
    public class Filial
    {
        /// <summary>
        /// Identificador único da filial (Chave Primária).
        /// </summary>
        /// <example>1</example>
        [Key]
        public int FilialId { get; set; }

        /// <summary>
        /// Nome descritivo da filial.
        /// </summary>
        /// <example>Mottu - Unidade Central SP</example>
        [Required(ErrorMessage = "O nome da filial é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da filial não pode exceder 100 caracteres.")]
        public string NomeFilial { get; set; } = null!;

        /// <summary>
        /// Cidade onde a filial está localizada.
        /// </summary>
        /// <example>São Paulo</example>
        [StringLength(100, ErrorMessage = "O nome da cidade não pode exceder 100 caracteres.")]
        public string? Cidade { get; set; }

        // Propriedade de navegação para o Entity Framework Core
        /// <summary>
        /// Coleção de pátios que pertencem a esta filial.
        /// </summary>
        public ICollection<Patio> Patios { get; set; } = new List<Patio>();
    }
}