using System;

namespace Mottu.Api.Models.DTOs.Response
{
    /// <summary>
    /// DTO de resposta para um Entregador.
    /// </summary>
    public class EntregadorResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string CNH { get; set; } = string.Empty;
        public string TipoCNH { get; set; } = string.Empty;
        public string ImagemCNH { get; set; } = string.Empty;
    }
}