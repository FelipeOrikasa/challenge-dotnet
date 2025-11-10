namespace Mottu.Api.Models
{
    public class DeliveryData
    {
        public float DistanceKm { get; set; }
        public float PackageWeightKg { get; set; }
        public string VehicleType { get; set; } = string.Empty;
    }

    public class DeliveryPrediction
    {
        public float EstimatedTimeMinutes { get; set; }
    }
}
