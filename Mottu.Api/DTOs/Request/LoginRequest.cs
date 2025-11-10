using System.ComponentModel.DataAnnotations;

namespace Mottu.Api.Models.DTOs.Request // <--- ESTE DEVE SER O NAMESPACE EXATO!
{
    /// <summary>
    /// DTO para requisição de autenticação (Login).
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome de usuário deve ter entre 3 e 50 caracteres.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string? Password { get; set; }
    }
}