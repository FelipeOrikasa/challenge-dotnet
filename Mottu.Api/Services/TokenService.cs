using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Mottu.Api.Services
{
    // O TokenService é usado para gerar o JWT após um login bem-sucedido.
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        
        // Recebe IConfiguration automaticamente via Injeção de Dependência (DI)
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gera um JSON Web Token (JWT) simples para autenticação.
        /// </summary>
        /// <param name="username">O nome de usuário para incluir nas Claims.</param>
        /// <returns>Objeto contendo o token e o tempo de expiração.</returns>
        public object GenerateToken(string username)
        {
            // Tenta obter a chave secreta das configurações (appsettings.json ou appsettings.Development.json)
            // Se não encontrar, usa a chave de desenvolvimento padrão.
            var key = _configuration.GetValue<string>("Jwt:Key") ?? "ChangeThisDevKey1234567890";
            
            // Define o tempo de expiração (Ex: 1 hora)
            var expiry = DateTime.UtcNow.AddHours(1);

            // Cria a chave de segurança usando a chave secreta
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define as Claims (informações sobre o usuário e o Perfil/Role)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // Lógica de perfil/role simples: Se o usuário for 'admin', atribui o perfil 'Admin', senão 'User'
                new Claim(ClaimTypes.Role, username.ToLowerInvariant() == "admin" ? "Admin" : "User")
            };

            // Cria o token
            var token = new JwtSecurityToken(
                issuer: null, // Omitido ou definido no appsettings se necessário
                audience: null, // Omitido ou definido no appsettings se necessário
                claims: claims,
                expires: expiry,
                signingCredentials: credentials);

            // Serializa o token
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Retorna o token e o tempo de expiração em um objeto anônimo
            return new { token = tokenString, expiresIn = (int)(expiry - DateTime.UtcNow).TotalSeconds };
        }
    }
}