using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mottu.Api.MLModels;
using Mottu.Api.Services.Interfaces; // Usar a interface para DI
using Mottu.Api.Models.DTOs.Response; // Para o DTO de resposta
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Mottu.Api.Controllers
{
    /// <summary>
    /// Controller responsável por fornecer previsões de Machine Learning (ML.NET).
    /// Requer autenticação JWT.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize] // <--- Garante que o endpoint esteja protegido
    public class PredictionController : ControllerBase
    {
        // Alterado para injetar a interface
        private readonly IDeliveryPredictionService _predictionService; 

        // Alterado o tipo do construtor para a interface
        public PredictionController(IDeliveryPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        /// <summary>
        /// Prevê o tempo de entrega com base nas características fornecidas.
        /// </summary>
        /// <remarks>
        /// Recebe um array de features e retorna o tempo de entrega previsto.
        /// Exemplo de Features: [Distância_KM, Peso_KG, Fator_Clima_Tráfego]
        /// </remarks>
        /// <param name="input">Dados de entrada para a predição (DeliveryTimeInput).</param>
        /// <returns>O tempo de entrega previsto em minutos.</returns>
        [HttpPost("delivery-time")]
        [ProducesResponseType(typeof(PredictDeliveryResponseDto), 200)] // Usando o DTO de Resposta
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation("Calcula o tempo de entrega previsto.", "A ordem das features no array deve ser: Distância (KM), Peso (KG), Fator_Clima_Tráfego (0.0-1.0).")]
        public IActionResult PredictDeliveryTime([FromBody] DeliveryTimeInput input)
        {
            // Validação simples: garante que há 3 features
            if (input.Features == null || input.Features.Length != 3)
            {
                return BadRequest("O array de Features deve conter exatamente 3 valores na ordem esperada.");
            }

            try
            {
                // A assinatura do serviço deve retornar PredictDeliveryResponseDto
                var prediction = _predictionService.Predict(input);
                
                if (prediction == null)
                {
                    // Erro 500 se o modelo não carregou
                    return StatusCode(500, new { message = "Erro ao processar a predição. Modelo ML.NET indisponível." });
                }
                
                // Arredonda o tempo previsto (usando a propriedade do DTO de Resposta)
                prediction.EstimatedTimeMinutes = (float)Math.Round(prediction.EstimatedTimeMinutes, 1);
                
                return Ok(prediction);
            }
            catch (Exception ex)
            {
                // Trata erros genéricos de processamento
                Console.WriteLine($"Erro na predição: {ex.Message}");
                return StatusCode(500, new { message = "Erro interno ao processar a predição de ML.NET." });
            }
        }
    }
}