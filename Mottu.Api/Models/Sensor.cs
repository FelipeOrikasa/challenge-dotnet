using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mottu.Api.Models
{
    /// <summary>
    /// Representa um sensor de localização instalado em um pátio.
    /// Corresponde à tabela 'Sensor' no banco de dados.
    /// </summary>
    public class Sensor
    {
        /// <summary>
        /// Identificador único do sensor (Chave Primária).
        /// </summary>
        /// <example>25</example>
        [Key]
        public int SensorId { get; set; }

        /// <summary>
        /// Descrição da localização ou tipo do sensor.
        /// </summary>
        /// <example>Sensor de RFID - Portão de Saída Leste</example>
        [StringLength(100, ErrorMessage = "A descrição não pode exceder 100 caracteres.")]
        public string? Descricao { get; set; }

        // --- Relacionamento com Patio (Muitos-para-Um) ---

        /// <summary>
        /// Chave estrangeira que referencia o Pátio onde o sensor está instalado.
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
        /// Histórico de localizações detectadas por este sensor.
        /// </summary>
        public ICollection<Localizacao> Localizacoes { get; set; } = new List<Localizacao>();
    }
}