using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mottu.Api.Models
{
    /// <summary>
    /// Representa um pátio, que pertence a uma filial e pode conter motos e sensores.
    /// Corresponde à tabela 'Patio' no banco de dados.
    /// </summary>
    public class Patio
    {
        /// <summary>
        /// Identificador único do pátio (Chave Primária).
        /// </summary>
        /// <example>10</example>
        [Key]
        public int PatioId { get; set; }

        /// <summary>
        /// Nome descritivo do pátio.
        /// </summary>
        /// <example>Pátio A - Motos de Baixa Cilindrada</example>
        [Required(ErrorMessage = "O nome do pátio é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do pátio não pode exceder 100 caracteres.")]
        public string NomePatio { get; set; } = null!;

        // --- Relacionamento com Filial (Muitos-para-Um) ---

        /// <summary>
        /// Chave estrangeira que referencia a Filial à qual este pátio pertence.
        /// </summary>
        /// <example>1</example>
        public int FilialId { get; set; }

        /// <summary>
        /// Propriedade de navegação para a Filial. Permite o acesso
        /// aos dados da filial a partir de um objeto Pátio.
        /// </summary>
        [ForeignKey("FilialId")]
        public Filial Filial { get; set; } = null!;

        // --- Outras Propriedades de Navegação (Um-para-Muitos) ---

        /// <summary>
        /// Coleção de motos estacionadas neste pátio.
        /// </summary>
        public ICollection<Moto> Motos { get; set; } = new List<Moto>();

        /// <summary>
        /// Coleção de sensores instalados neste pátio.
        /// </summary>
        public ICollection<Sensor> Sensores { get; set; } = new List<Sensor>();
    }
}