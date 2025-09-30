using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.DTOs.PatioDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a criação de um novo pátio.
    /// Este é o objeto esperado no corpo (body) de requisições POST.
    /// </summary>
    public class CreatePatioDto
    {
        /// <summary>
        /// Nome do pátio.
        /// </summary>
        /// <example>Pátio B - Motos Elétricas</example>
        [Required(ErrorMessage = "O nome do pátio é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do pátio não pode exceder 100 caracteres.")]
        public string NomePatio { get; set; } = null!;

        /// <summary>
        /// ID da filial à qual este pátio pertence.
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "O ID da filial é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID da filial deve ser um número válido.")]
        public int FilialId { get; set; }
    }
}