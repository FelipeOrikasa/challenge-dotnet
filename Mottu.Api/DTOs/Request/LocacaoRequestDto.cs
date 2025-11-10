using System;
using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models.DTOs.Request
{
    public class LocacaoRequestDto
    {
        [Required]
        public Guid EntregadorId { get; set; }
        
        [Required]
        public Guid MotoId { get; set; }
        
        [Required(ErrorMessage = "O plano de dias é obrigatório.")]
        [Range(7, 50, ErrorMessage = "O plano deve ser de 7, 15, 30, 45 ou 50 dias.")]
        public int PlanoDias { get; set; }
    }
}