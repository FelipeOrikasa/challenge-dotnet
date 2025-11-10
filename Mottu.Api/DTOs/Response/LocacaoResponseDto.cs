using System;

namespace Mottu.Api.Models.DTOs.Response
{
    public class LocacaoResponseDto
    {
        public Guid Id { get; set; }
        public Guid EntregadorId { get; set; }
        public Guid MotoId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTerminoPrevista { get; set; }
        public DateTime? DataTerminoEfetiva { get; set; }
        public int DiasContratados { get; set; }
        public decimal CustoDiarioContratado { get; set; }
        public decimal CustoTotalPrevisto { get; set; }
        public decimal? CustoFinal { get; set; }
        
        // ✅ NOVO: Adicione esta propriedade
        public string Status { get; set; } = string.Empty; 
    }
}