using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.DTOs.MotoDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a atualização de uma moto.
    /// A única alteração permitida é a movimentação da moto para um novo pátio.
    /// </summary>
    public class UpdateMotoDto
    {
        /// <summary>
        /// O ID do novo pátio para onde a moto será movida.
        /// </summary>
        /// <example>12</example>
        [Required(ErrorMessage = "O ID do novo pátio é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do pátio deve ser um número válido.")]
        public int PatioId { get; set; }
    }
}