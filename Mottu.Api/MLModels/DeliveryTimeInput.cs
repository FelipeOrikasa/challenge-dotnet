using Microsoft.ML.Data;
using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.MLModels // <--- O NAMESPACE TEM QUE SER EXATAMENTE ESTE!
{
    // Esta classe define os dados de entrada para o modelo de predição do ML.NET.
    // Ela será usada no Controller e no Serviço de Predição.
    public class DeliveryTimeInput
    {
        // 1. Campo para o resultado real durante o treinamento (Target)
        // Usado apenas durante o treinamento (ver DeliveryPredictionService.cs)
        [ColumnName("TargetDeliveryTimeMinutes")]
        public float TargetDeliveryTimeMinutes { get; set; }

        // 2. Campo para as características (Features) que o modelo usará para prever
        
        [Required(ErrorMessage = "O array Features é obrigatório.")]
        [MinLength(3, ErrorMessage = "O array Features deve conter 3 elementos.")]
        
        // [VectorType(3)] informa ao ML.NET que este array é o vetor de features
        // A ordem dos elementos é crítica: [Distância_KM, Clima_Score, Tráfego_Intensidade]
        [VectorType(3)] 
        [ColumnName("Features")]
        public float[] Features { get; set; } = new float[3]; 
    }
}