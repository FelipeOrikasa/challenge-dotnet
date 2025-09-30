using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.DTOs.FilialDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a atualização de uma filial.
    /// Este é o objeto esperado no corpo (body) de requisições PUT.
    /// </summary>
    public class UpdateFilialDto
    {
        /// <summary>
        /// Novo nome da filial.
        /// </summary>
        /// <example>Mottu - Unidade Leste</example>
        [Required(ErrorMessage = "O nome da filial é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da filial não pode exceder 100 caracteres.")]
        public string NomeFilial { get; set; } = null!;

        /// <summary>
        /// Nova cidade onde a filial está localizada (opcional).
        /// </summary>
        /// <example>Guarulhos</example>
        [StringLength(100, ErrorMessage = "O nome da cidade não pode exceder 100 caracteres.")]
        public string? Cidade { get; set; }
    }
}