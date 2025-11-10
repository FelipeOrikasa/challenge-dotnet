namespace Mottu.Api.Models.Enums
{
    /// <summary>
    /// Define os possíveis status para uma Locação de moto.
    /// </summary>
    public enum StatusLocacao
    {
        /// <summary>
        /// Locação em andamento, dentro do prazo.
        /// </summary>
        Ativa = 1,

        /// <summary>
        /// Locação encerrada no prazo previsto.
        /// </summary>
        FinalizadaNoPrazo = 2,

        /// <summary>
        /// Locação encerrada antes do prazo (sujeita a multa).
        /// </summary>
        FinalizadaAntecipada = 3,

        /// <summary>
        /// Locação devolvida com atraso (sujeita a multa por dia extra).
        /// </summary>
        FinalizadaAtrasada = 4,

        /// <summary>
        /// Locação expirada, mas a moto ainda não foi devolvida.
        /// </summary>
        Expirada = 5
    }
}