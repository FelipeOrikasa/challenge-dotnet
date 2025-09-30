using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.DTOs.LocalizacaoDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para criar um novo registro de localização.
    /// </summary>
    public class CreateLocalizacaoDto
    {
        /// <summary>
        /// ID da moto que foi detectada.
        /// </summary>
        /// <example>501</example>
        [Required(ErrorMessage = "O ID da moto é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID da moto deve ser um número válido.")]
        public int MotoId { get; set; }

        /// <summary>
        /// ID do sensor que detectou a moto.
        /// </summary>
        /// <example>25</example>
        [Required(ErrorMessage = "O ID do sensor é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do sensor deve ser um número válido.")]
        public int SensorId { get; set; }
    }
}