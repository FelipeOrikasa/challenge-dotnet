using System.ComponentModel.DataAnnotations;
using System;

namespace Mottu.Api.Models.DTOs.Request
{
    /// <summary>
    /// DTO para criação de um novo Entregador.
    /// </summary>
    public class EntregadorRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(18)]
        public string CNPJ { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "A CNH é obrigatória.")]
        [StringLength(20)]
        public string CNH { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Tipo CNH é obrigatório (A ou AB).")]
        [StringLength(2)]
        public string TipoCNH { get; set; } = string.Empty;

        /// <summary>
        /// Nome do arquivo da imagem da CNH (apenas o nome, será usado para upload).
        /// </summary>
        [Required(ErrorMessage = "A imagem da CNH é obrigatória.")]
        public string ImagemCNH { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para atualização de um Entregador (usado apenas para atualizar a CNH/Imagem).
    /// </summary>
    public class EntregadorUpdateRequest
    {
        [Required(ErrorMessage = "O número da CNH é obrigatório.")]
        [StringLength(20)]
        public string CNH { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "O Tipo CNH é obrigatório (A ou AB).")]
        [StringLength(2)]
        public string TipoCNH { get; set; } = string.Empty;

        /// <summary>
        /// Nome do arquivo da imagem da CNH (apenas o nome, será usado para upload).
        /// </summary>
        [Required(ErrorMessage = "A imagem da CNH é obrigatória.")]
        public string ImagemCNH { get; set; } = string.Empty;
    }
}