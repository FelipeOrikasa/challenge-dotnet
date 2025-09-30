using Mottu.Api.DTOs.Shared;
using System.Collections.Generic;

namespace Mottu.Api.DTOs.SensorDtos
{
    /// <summary>
    /// DTO para a leitura e retorno de dados de um sensor.
    /// </summary>
    public class ReadSensorDto
    {
        /// <summary>
        /// Identificador único do sensor.
        /// </summary>
        /// <example>25</example>
        public int SensorId { get; set; }

        /// <summary>
        /// Descrição da localização ou tipo do sensor.
        /// </summary>
        /// <example>Sensor de RFID - Portão de Entrada</example>
        public string? Descricao { get; set; }

        /// <summary>
        /// ID do pátio onde o sensor está instalado.
        /// </summary>
        /// <example>10</example>
        public int PatioId { get; set; }

        /// <summary>
        /// Nome do pátio onde o sensor está instalado (dado "achatado" para conveniência).
        /// </summary>
        /// <example>Pátio A - Vistoria</example>
        public string NomePatio { get; set; } = null!;

        /// <summary>
        /// Lista de links HATEOAS relacionados ao sensor.
        /// </summary>
        public List<LinkDto> Links { get; set; } = new();
    }
}