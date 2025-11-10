using Microsoft.AspNetCore.Mvc;
using Mottu.Api.Models.DTOs.Request; // Namespace correto para Request DTOs
using Mottu.Api.Models.DTOs.Response; // Namespace correto para Response DTOs
using Mottu.Api.Services.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/entregadores")]
    // [Route("api/v{version:apiVersion}/entregadores")] // Padrão mais profissional
    // [ApiVersion("1.0")]
    public class EntregadorController : ControllerBase
    {
        private readonly IEntregadorService _service;

        public EntregadorController(IEntregadorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria um novo Entregador no sistema.
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // Usamos EntregadorRequest, que é o nome do DTO que definimos
        public async Task<IActionResult> Post([FromBody] EntregadorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // O Serviço retorna ApiResponse<EntregadorResponse>
            var result = await _service.AddEntregadorAsync(request);

            if (!result.IsSuccess)
            {
                // Captura erros de regra de negócio (400) ou outros erros de serviço
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            // Se for sucesso, retorna 201 Created com os dados
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        /// <summary>
        /// Busca todos os entregadores cadastrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("EntregadorController.GetAll: Endpoint chamado");
                Console.WriteLine("EntregadorController.GetAll: Endpoint chamado");
                
                // O Serviço retorna ApiResponse<IEnumerable<EntregadorResponse>>
                var result = await _service.GetAllEntregadoresAsync();
                
                System.Diagnostics.Debug.WriteLine($"EntregadorController.GetAll: Result.IsSuccess: {result.IsSuccess}, Data count: {result.Data?.Count() ?? 0}");
                Console.WriteLine($"EntregadorController.GetAll: Result.IsSuccess: {result.IsSuccess}, Data count: {result.Data?.Count() ?? 0}");
                
                if (result.Data != null && result.Data.Any())
                {
                    var primeiro = result.Data.First();
                    System.Diagnostics.Debug.WriteLine($"EntregadorController.GetAll: Primeiro entregador no response - Id: {primeiro.Id}, Nome: {primeiro.Nome}");
                    Console.WriteLine($"EntregadorController.GetAll: Primeiro entregador no response - Id: {primeiro.Id}, Nome: {primeiro.Nome}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("EntregadorController.GetAll: Data é null ou vazio");
                    Console.WriteLine("EntregadorController.GetAll: Data é null ou vazio");
                }
                
                // Aqui presumimos que GetAll é sempre 200 OK ou 204 No Content se vazio.
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EntregadorController.GetAll: ERRO - {ex.Message}");
                Console.WriteLine($"EntregadorController.GetAll: ERRO - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Busca um entregador por ID (Guid).
        /// </summary>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            // O Serviço retorna ApiResponse<EntregadorResponse>
            var result = await _service.GetEntregadorByIdAsync(id);

            if (!result.IsSuccess)
            {
                // Captura 404 Not Found ou outro erro de serviço
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Atualiza os dados de um entregador existente.
        /// </summary>
        [HttpPut("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // Usamos EntregadorUpdateRequest
        public async Task<IActionResult> Put(Guid id, [FromBody] EntregadorUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // O Serviço retorna ApiResponse<EntregadorResponse>
            var result = await _service.UpdateEntregadorAsync(id, request);
            
            if (!result.IsSuccess)
            {
                // Captura 404 (Entregador não encontrado) ou 400 (CNH já existe)
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }
            
            return Ok(result.Data);
        }

        /// <summary>
        /// Exclui um entregador por ID (Guid).
        /// </summary>
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            // O Serviço retorna ApiResponse<bool>
            var result = await _service.DeleteEntregadorAsync(id);

            // Se não for sucesso (404 Not Found ou 400 Locação Ativa)
            if (!result.IsSuccess) 
            {
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            // Se for sucesso (200 OK ou 204 No Content)
            return NoContent();
        }
    }
}