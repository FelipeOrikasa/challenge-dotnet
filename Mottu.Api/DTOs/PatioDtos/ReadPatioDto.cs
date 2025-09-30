using Mottu.Api.DTOs.Shared;
using System.Collections.Generic;

namespace Mottu.Api.DTOs.PatioDtos
{
    /// <summary>
    /// DTO para a leitura e retorno de dados de um pátio.
    /// Este é o objeto que será retornado nos endpoints GET.
    /// </summary>
    public class ReadPatioDto
    {
        /// <summary>
        /// Identificador único do pátio.
        /// </summary>
        /// <example>10</example>
        public int PatioId { get; set; }

        /// <summary>
        /// Nome descritivo do pátio.
        /// </summary>
        /// <example>Pátio A - Vistoria e Manutenção</example>
        public string NomePatio { get; set; } = null!;

        /// <summary>
        /// ID da filial à qual o pátio pertence.
        /// </summary>
        /// <example>1</example>
        public int FilialId { get; set; }

        /// <summary>
        /// Nome da filial à qual o pátio pertence.
        /// Este é um dado "achatado" (flattened) da entidade Filial para facilitar o consumo da API.
        /// </summary>
        /// <example>Mottu - Unidade Central SP</example>
        public string NomeFilial { get; set; } = null!;

        /// <summary>
        /// Lista de links HATEOAS relacionados ao pátio.
        /// </summary>
        public List<LinkDto> Links { get; set; } = new();
    }
}