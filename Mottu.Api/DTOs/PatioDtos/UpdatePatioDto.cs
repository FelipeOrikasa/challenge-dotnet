using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.DTOs.PatioDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a atualização de um pátio.
    /// Apenas o nome do pátio pode ser alterado.
    /// </summary>
    public class UpdatePatioDto
    {
        /// <summary>
        /// Novo nome do pátio.
        /// </summary>
        /// <example>Pátio C - Longa Duração</example>
        [Required(ErrorMessage = "O nome do pátio é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do pátio não pode exceder 100 caracteres.")]
        public string NomePatio { get; set; } = null!;
    }
}