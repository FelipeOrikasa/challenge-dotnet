using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mottu.Api.Models.Entities
{
    public class Localizacao
    {
        public Guid Id { get; set; }
        public int SensorId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime Timestamp { get; set; }

        // Propriedades de Navegação (Relacionamento)
        public Sensor Sensor { get; set; } = null!;
    }
}