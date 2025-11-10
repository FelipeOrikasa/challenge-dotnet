using System;
using Mottu.Api.Models.Enums;

namespace Mottu.Api.Models.Entities
{
    public class Locacao
    {
        public Guid Id { get; set; }
        public Guid EntregadorId { get; set; }
        public Guid MotoId { get; set; }

        public DateTime DataInicio { get; set; }
        // Consistente: Data de Termino Prevista
        public DateTime DataTerminoPrevista { get; set; }
        // Consistente: Data de Termino Efetiva (Real)
        public DateTime? DataTerminoEfetiva { get; set; } // Nullable

        public int DiasContratados { get; set; }
        public decimal CustoDiarioContratado { get; set; }
        public decimal CustoTotalPrevisto { get; set; }
        public decimal CustoFinal { get; set; }

        public StatusLocacao Status { get; set; }

        // Propriedades de Navegação
        public Entregador Entregador { get; set; } = null!;
        public Moto Moto { get; set; } = null!;
    }
}