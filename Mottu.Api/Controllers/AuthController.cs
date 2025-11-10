using Microsoft.AspNetCore.Mvc;
using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Services;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        // O TokenService é injetado automaticamente pelo DI
        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Realiza a autenticação e retorna um JWT (JSON Web Token).
        /// </summary>
        /// <remarks>
        /// Credenciais de demonstração: Username 'admin' ou 'user', Senha '123456'
        /// </remarks>
        /// <param name="request">DTO contendo Username e Password.</param>
        /// <returns>Objeto contendo o token JWT e o tempo de expiração.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // O [ApiController] já lida com DTOs inválidos ( ModelState.IsValid )
            
            // --- 1. Validação de Credenciais (Lógica Simples de Demonstração) ---
            // Em uma aplicação real, você usaria Entity Framework para consultar um banco de dados de usuários.
            var isValidUser = (request.Username?.ToLowerInvariant() == "admin" || request.Username?.ToLowerInvariant() == "user") 
                            && request.Password == "123456";

            if (!isValidUser)
            {
                // Retorna 401 Unauthorized se as credenciais forem inválidas
                return Unauthorized(new { message = "Credenciais inválidas." });
            }

            // --- 2. Geração do Token ---
            var tokenData = _tokenService.GenerateToken(request.Username!);

            // Retorna 200 OK com o token e informações de expiração
            return Ok(tokenData);
        }
    }
}