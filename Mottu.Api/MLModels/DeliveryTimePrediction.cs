using Microsoft.ML.Data;

namespace Mottu.Api.MLModels
{
    /// <summary>
    /// Classe que define o resultado da predição do modelo ML.NET.
    /// O atributo ColumnName deve coincidir com o nome da coluna de previsão ('Score').
    /// </summary>
    public class DeliveryTimePrediction
    {
        // O nome da coluna de predição gerada pelo ML.NET é sempre 'Score' (no contexto de regressão).
        [ColumnName("Score")]
        public float PredictedDeliveryTimeMinutes { get; set; }
    }
}