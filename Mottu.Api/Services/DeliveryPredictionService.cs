using Microsoft.ML;
using Mottu.Api.MLModels;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Services.Interfaces;
using System;
using System.IO;

namespace Mottu.Api.Services
{
    public class DeliveryPredictionService : IDeliveryPredictionService
    {
        private readonly MLContext _mlContext;
        // Adicionado '?' (nullable) para resolver o warning/amarelado
        private ITransformer? _trainedModel; 
        private PredictionEngine<DeliveryTimeInput, DeliveryTimePrediction>? _predictionEngine;
        private const string MLModelPath = "ml_model.zip"; 

        public DeliveryPredictionService()
        {
            _mlContext = new MLContext();
            LoadModel();
        }

        private void LoadModel()
        {
            try
            {
                if (File.Exists(MLModelPath))
                {
                    using (var stream = new FileStream(MLModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        _trainedModel = _mlContext.Model.Load(stream, out var modelSchema);
                    }
                    
                    if (_trainedModel != null)
                    {
                        // O '!' garante ao compilador que _trainedModel não é nulo neste ponto
                        _predictionEngine = _mlContext.Model.CreatePredictionEngine<DeliveryTimeInput, DeliveryTimePrediction>(_trainedModel!);
                    }
                    else
                    {
                        Console.WriteLine($"[AVISO ML.NET] Modelo ML não carregado, _trainedModel é null.");
                    }
                }
                else
                {
                    Console.WriteLine($"[ERRO ML.NET] Arquivo do modelo não encontrado: {MLModelPath}. O serviço de predição não funcionará.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO ML.NET] Falha ao carregar o modelo ML: {ex.Message}");
                // As atribuições a null agora são permitidas
                _trainedModel = null; 
                _predictionEngine = null; 
            }
        }

        /// <summary>
        /// Realiza a predição do tempo de entrega com base nos dados de entrada.
        /// </summary>
        /// <param name="data">O modelo de input do ML.NET (DeliveryTimeInput) com as features.</param>
        /// <returns>DTO de resposta com o tempo de entrega estimado, ou null se o modelo não estiver disponível.</returns>
        // O tipo de retorno agora é 'nullable' para permitir o retorno de null na linha 73
        public PredictDeliveryResponseDto? Predict(DeliveryTimeInput data)
        {
            // O teste de nulidade (linha 68) agora é obrigatório
            if (_predictionEngine is null) 
            {
                return null; // O tipo de retorno permite null
            }

            // O '!' garante que o compilador não reclame sobre a possibilidade de ser nulo
            var prediction = _predictionEngine!.Predict(data);

            return new PredictDeliveryResponseDto
            {
                EstimatedTimeMinutes = prediction.PredictedDeliveryTimeMinutes
            };
        }
    }
}