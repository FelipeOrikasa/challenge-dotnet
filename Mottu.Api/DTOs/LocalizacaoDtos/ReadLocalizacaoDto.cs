using Mottu.Api.DTOs.Shared;
using System;
using System.Collections.Generic;

namespace Mottu.Api.DTOs.LocalizacaoDtos
{
    /// <summary>
    /// DTO para a leitura e retorno de um registro de localização.
    /// </summary>
    public class ReadLocalizacaoDto
    {
        /// <summary>
        /// Identificador único do registro de localização.
        /// </summary>
        /// <example>1001</example>
        public int LocalizacaoId { get; set; }

        /// <summary>
        /// Data e hora exatas em que a localização foi registrada.
        /// </summary>
        public DateTime DataHora { get; set; }

        /// <summary>
        /// ID da moto que foi localizada.
        /// </summary>
        /// <example>501</example>
        public int MotoId { get; set; }

        /// <summary>
        /// Placa da moto localizada (dado "achatado").
        /// </summary>
        /// <example>BRA2E19</example>
        public string PlacaMoto { get; set; } = null!;

        /// <summary>
        /// ID do sensor que detectou a moto.
        /// </summary>
        /// <example>25</example>
        public int SensorId { get; set; }

        /// <summary>
        /// Descrição do sensor que fez a detecção (dado "achatado").
        /// </summary>
        /// <example>Sensor de RFID - Portão de Entrada</example>
        public string? DescricaoSensor { get; set; }

        /// <summary>
        /// Nome do pátio onde a detecção ocorreu (dado "achatado").
        /// </summary>
        /// <example>Pátio A - Vistoria</example>
        public string NomePatio { get; set; } = null!;

        /// <summary>
        /// Lista de links HATEOAS relacionados a este registro.
        /// </summary>
        public List<LinkDto> Links { get; set; } = new();
    }
}