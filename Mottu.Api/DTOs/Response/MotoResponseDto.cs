using System;

namespace Mottu.Api.Models.DTOs.Response
{
    /// <summary>
    /// DTO de resposta para uma Moto.
    /// </summary>
    public class MotoResponse
    {
        public Guid Id { get; set; }
        public int Ano { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
    }
}