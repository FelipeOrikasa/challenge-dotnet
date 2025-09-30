using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.DTOs.FilialDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a criação de uma nova filial.
    /// Este é o objeto esperado no corpo (body) de requisições POST.
    /// </summary>
    public class CreateFilialDto
    {
        /// <summary>
        /// Nome da filial.
        /// </summary>
        /// <example>Mottu - Unidade Campinas</example>
        [Required(ErrorMessage = "O nome da filial é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da filial não pode exceder 100 caracteres.")]
        public string NomeFilial { get; set; } = null!;

        /// <summary>
        /// Cidade onde a filial está localizada (opcional).
        /// </summary>
        /// <example>Campinas</example>
        [StringLength(100, ErrorMessage = "O nome da cidade não pode exceder 100 caracteres.")]
        public string? Cidade { get; set; }
    }
}