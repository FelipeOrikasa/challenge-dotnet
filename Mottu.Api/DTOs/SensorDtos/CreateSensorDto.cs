using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.DTOs.SensorDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a criação de um novo sensor.
    /// </summary>
    public class CreateSensorDto
    {
        /// <summary>
        /// Descrição opcional da localização ou tipo do sensor.
        /// </summary>
        /// <example>Sensor de RFID - Portão de Saída</example>
        [StringLength(100, ErrorMessage = "A descrição não pode exceder 100 caracteres.")]
        public string? Descricao { get; set; }

        /// <summary>
        /// ID do pátio onde este sensor está instalado.
        /// </summary>
        /// <example>10</example>
        [Required(ErrorMessage = "O ID do pátio é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do pátio deve ser um número válido.")]
        public int PatioId { get; set; }
    }
}