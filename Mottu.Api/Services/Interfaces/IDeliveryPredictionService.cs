using Mottu.Api.MLModels;
using Mottu.Api.Models.DTOs.Response;

namespace Mottu.Api.Services.Interfaces
{
    public interface IDeliveryPredictionService
    {
        // CORREÇÃO FINAL: O tipo de retorno deve ser nullable para corresponder à implementação
        PredictDeliveryResponseDto? Predict(DeliveryTimeInput data); 
    }
}