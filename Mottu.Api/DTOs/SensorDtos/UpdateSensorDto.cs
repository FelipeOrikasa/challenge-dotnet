using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.DTOs.SensorDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a atualização de um sensor.
    /// Apenas a descrição do sensor pode ser alterada.
    /// </summary>
    public class UpdateSensorDto
    {
        /// <summary>
        /// A nova descrição da localização ou tipo do sensor.
        /// </summary>
        /// <example>Sensor RFID - Portão de Entrada (Manutenção)</example>
        [StringLength(100, ErrorMessage = "A descrição não pode exceder 100 caracteres.")]
        public string? Descricao { get; set; }
    }
}