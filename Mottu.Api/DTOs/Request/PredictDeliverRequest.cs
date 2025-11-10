using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models.DTOs.Request
{
    /// <summary>
    /// Define os dados de entrada para o endpoint de predição.
    /// Esta classe é o DTO de Requisição (JSON Body) do Controller.
    /// </summary>
    public class PredictDeliveryRequestDto
    {
        [Required(ErrorMessage = "A distância em KM é obrigatória.")]
        [Range(0.1, 1000, ErrorMessage = "A distância deve ser um valor positivo.")]
        public float DistanceKm { get; set; }
        
        [Required(ErrorMessage = "O peso da encomenda em KG é obrigatório.")]
        [Range(0.01, 50, ErrorMessage = "O peso deve ser um valor positivo.")]
        public float PackageWeightKg { get; set; }
        
        // Adicionar campos de tráfego/clima se necessário, ou usar 
        // valores padrão no serviço de predição. Vou mantê-lo simples 
        // com base no que o seu controller estava verificando (Distância e Peso).
    }
}