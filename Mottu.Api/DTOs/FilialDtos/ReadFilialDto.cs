using Mottu.Api.DTOs.Shared;
using System.Collections.Generic;

namespace Mottu.Api.DTOs.FilialDtos
{
    /// <summary>
    /// DTO para a leitura e retorno de dados de uma filial.
    /// Este é o objeto que será retornado nos endpoints GET.
    /// </summary>
    public class ReadFilialDto
    {
        /// <summary>
        /// Identificador único da filial.
        /// </summary>
        /// <example>1</example>
        public int FilialId { get; set; }

        /// <summary>
        /// Nome da filial.
        /// </summary>
        /// <example>Mottu - Unidade Central SP</example>
        public string NomeFilial { get; set; } = null!;

        /// <summary>
        /// Cidade onde a filial está localizada.
        /// </summary>
        /// <example>São Paulo</example>
        public string? Cidade { get; set; }

        /// <summary>
        /// Lista de links HATEOAS relacionados à filial.
        /// </summary>
        public List<LinkDto> Links { get; set; } = new();
    }
}