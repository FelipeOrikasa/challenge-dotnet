using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mottu.Api.Models
{
    /// <summary>
    /// Representa um registro de localização de uma moto por um sensor em um momento específico.
    /// Esta é a entidade transacional do sistema, registrando um evento que conecta uma Moto a um Sensor.
    /// Corresponde à tabela 'Localizacao' no banco de dados.
    /// </summary>
    public class Localizacao
    {
        /// <summary>
        /// Identificador único do registro de localização (Chave Primária).
        /// </summary>
        /// <example>1001</example>
        [Key]
        public int LocalizacaoId { get; set; }

        /// <summary>
        /// Data e hora exatas em que a localização foi registrada.
        /// </summary>
        [Required(ErrorMessage = "A data e hora são obrigatórias.")]
        public DateTime DataHora { get; set; }

        // --- Relacionamento com Moto (Muitos-para-Um) ---

        /// <summary>
        /// Chave estrangeira que referencia a Moto que foi localizada.
        /// </summary>
        /// <example>501</example>
        public int MotoId { get; set; }

        /// <summary>
        /// Propriedade de navegação para a Moto.
        /// </summary>
        [ForeignKey("MotoId")]
        public Moto Moto { get; set; } = null!;
        // --- Relacionamento com Sensor (Muitos-para-Um) ---

        /// <summary>
        /// Chave estrangeira que referencia o Sensor que detectou a moto.
        /// </summary>
        /// <example>25</example>
        public int SensorId { get; set; }

        /// <summary>
        /// Propriedade de navegação para o Sensor.
        /// </summary>
        [ForeignKey("SensorId")]
        public Sensor Sensor { get; set; } = null!;
    }
}