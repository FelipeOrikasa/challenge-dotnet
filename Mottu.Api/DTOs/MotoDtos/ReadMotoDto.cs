using Mottu.Api.DTOs.Shared;
using System.Collections.Generic;

namespace Mottu.Api.DTOs.MotoDtos
{
    /// <summary>
    /// DTO para a leitura e retorno de dados de uma moto.
    /// Este é o objeto que será retornado nos endpoints GET.
    /// </summary>
    public class ReadMotoDto
    {
        /// <summary>
        /// Identificador único da moto.
        /// </summary>
        /// <example>550e8400-e29b-41d4-a716-446655440000</example>
        public Guid MotoId { get; set; }

        /// <summary>
        /// Placa de identificação da moto.
        /// </summary>
        /// <example>BRA2E19</example>
        public string Placa { get; set; } = null!;

        /// <summary>
        /// Modelo da moto.
        /// </summary>
        /// <example>Honda CB 300F Twister</example>
        public string Modelo { get; set; } = null!;

        /// <summary>
        /// Ano de fabricação da moto.
        /// </summary>
        /// <example>2024</example>
        public int Ano { get; set; }

        /// <summary>
        /// ID do pátio onde a moto está localizada.
        /// </summary>
        /// <example>10</example>
        public int PatioId { get; set; }

        /// <summary>
        /// Nome do pátio onde a moto está localizada (dado "achatado").
        /// </summary>
        /// <example>Pátio A - Vistoria e Manutenção</example>
        public string NomePatio { get; set; } = null!;

        /// <summary>
        /// Lista de links HATEOAS relacionados à moto.
        /// </summary>
        public List<LinkDto> Links { get; set; } = new();
    }
}