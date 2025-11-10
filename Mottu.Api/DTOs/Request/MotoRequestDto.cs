using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models.DTOs.Request
{
    /// <summary>
    /// DTO para criação de uma nova Moto.
    /// </summary>
    public class MotoRequest
    {
        [Required(ErrorMessage = "O ano é obrigatório.")]
        [Range(1900, 3000, ErrorMessage = "Ano inválido.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(10)]
        public string Placa { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para atualização de uma Moto (permite apenas a troca de placa).
    /// </summary>
    public class MotoUpdateRequest
    {
        [Required(ErrorMessage = "A nova placa é obrigatória.")]
        [StringLength(10)]
        public string Placa { get; set; } = string.Empty;
    }
}