using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.DTOs.MotoDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a criação de uma nova moto.
    /// Este é o objeto esperado no corpo (body) de requisições POST.
    /// </summary>
    public class CreateMotoDto
    {
        /// <summary>
        /// Placa da moto.
        /// </summary>
        /// <example>NEW-2025</example>
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(20, ErrorMessage = "A placa não pode exceder 20 caracteres.")]
        public string Placa { get; set; } = null!;

        /// <summary>
        /// Modelo da moto.
        /// </summary>
        /// <example>Honda PCX 150</example>
        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O modelo não pode exceder 100 caracteres.")]
        public string Modelo { get; set; } = null!;

        /// <summary>
        /// Ano de fabricação da moto.
        /// </summary>
        /// <example>2025</example>
        [Required(ErrorMessage = "O ano é obrigatório.")]
        [Range(1980, 2026, ErrorMessage = "O ano de fabricação deve ser válido.")]
        public int Ano { get; set; }

        /// <summary>
        /// ID do pátio onde a moto será inicialmente alocada.
        /// </summary>
        /// <example>10</example>
        [Required(ErrorMessage = "O ID do pátio é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do pátio deve ser um número válido.")]
        public int PatioId { get; set; }
    }
}