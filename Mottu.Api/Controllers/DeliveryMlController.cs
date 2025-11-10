using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mottu.Api.MLModels; 
using Mottu.Api.Models.DTOs.Request; 
using Mottu.Api.Models.DTOs.Response; 
using Mottu.Api.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Mottu.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/m1")]
    [SwaggerTag("Endpoints de Predição e Machine Learning (ML.NET)")]
    public class DeliveryMIController : ControllerBase
    {
        private readonly IDeliveryPredictionService _service; 

        public DeliveryMIController(IDeliveryPredictionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Estima o tempo de entrega de uma encomenda com base nas características fornecidas (Distância, Peso e Fator).
        /// </summary>
        /// <param name="data">O DTO de Requisição enviado pelo cliente (contém Distância e Peso).</param>
        /// <returns>O tempo estimado de entrega em minutos.</returns>
        [HttpPost("predict")]
        [SwaggerOperation(Summary = "Predição de Tempo de Entrega (ML.NET)")]
        [ProducesResponseType(typeof(PredictDeliveryResponseDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public IActionResult Predict([FromBody] PredictDeliveryRequestDto data)
        {
            if (data == null)
            {
                return BadRequest(new { error = "Request body is required." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- Mapeamento do DTO de Requisição para o Modelo de ML.NET (DeliveryTimeInput) ---
            const float DefaultThirdFeatureValue = 0.5f; 

            var mlInput = new DeliveryTimeInput
            {
                Features = new float[] 
                { 
                    data.DistanceKm, 
                    data.PackageWeightKg, 
                    DefaultThirdFeatureValue 
                }
            };

            // CORREÇÃO FINAL: Variável 'result' declarada como nullable (com '?')
            PredictDeliveryResponseDto? result = _service.Predict(mlInput);

            // A verificação de null é fundamental, pois o serviço pode retornar null se o modelo não carregar.
            if (result == null)
            {
                return StatusCode(500, new { error = "ML model not available on server or service failed." });
            }
            
            // O '!' afirma ao compilador que 'result' não é nulo aqui.
            result!.EstimatedTimeMinutes = (float)Math.Round(result.EstimatedTimeMinutes, 1);

            return Ok(result); 
        }
    }
}