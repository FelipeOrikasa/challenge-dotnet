using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Mottu.Api.Models.DTOs.Request;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Mottu.Api.Tests.Integration
{
    // A classe de teste usa a fábrica customizada para isolar o ambiente
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            // Cria o cliente HTTP a partir da fábrica
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                // Permite testar endpoints sem autenticação
                AllowAutoRedirect = false 
            });
        }

        // Endpoint a ser testado
        private const string LoginEndpoint = "/api/v1/Auth/login";

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnOkAndToken()
        {
            // Arrange
            var validRequest = new LoginRequest
            {
                // Credenciais válidas conforme o AuthController
                Username = "admin", 
                Password = "123456" 
            };

            // Act
            var response = await _client.PostAsJsonAsync(LoginEndpoint, validRequest);

            // Assert
            // 1. Deve retornar 200 OK
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // 2. O corpo da resposta deve conter o token
            var responseBody = await response.Content.ReadFromJsonAsync<dynamic>();
            Assert.NotNull(responseBody);
            // Verifica se a propriedade 'token' existe e não é nula/vazia
            Assert.False(string.IsNullOrEmpty((string)responseBody!.token));
            
            // 3. Opcional: Verifica a validade (1 hora)
            Assert.True((int)responseBody!.expiresIn > 0);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var invalidRequest = new LoginRequest
            {
                Username = "wronguser", 
                Password = "wrongpassword" 
            };

            // Act
            var response = await _client.PostAsJsonAsync(LoginEndpoint, invalidRequest);

            // Assert
            // Deve retornar 401 Unauthorized
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithMissingFields_ShouldReturnBadRequest()
        {
            // Arrange
            // Request sem a senha (violando o [Required] no DTO)
            var missingPasswordRequest = new LoginRequest
            {
                Username = "admin", 
                Password = null! 
            };

            // Act
            var response = await _client.PostAsJsonAsync(LoginEndpoint, missingPasswordRequest);

            // Assert
            // Deve retornar 400 Bad Request devido à falha de ModelState.IsValid
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}