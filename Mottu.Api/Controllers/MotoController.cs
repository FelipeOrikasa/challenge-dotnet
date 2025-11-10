using Microsoft.AspNetCore.Mvc;
using Mottu.Api.Models.DTOs.Request; // Usando os namespaces corretos para Request DTOs
using Mottu.Api.Models.DTOs.Response; // Usando os namespaces corretos para Response DTOs
using Mottu.Api.Services.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/motos")]
    public class MotoController : ControllerBase
    {
        private readonly IMotoService _service;

        public MotoController(IMotoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria uma nova Motocicleta no sistema.
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)] // 409 para placa duplicada
        // DTO CORRIGIDO: MotoRequest
        public async Task<IActionResult> Post([FromBody] MotoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Serviço retorna ApiResponse<MotoResponse>
            var result = await _service.AddMotoAsync(request);

            if (!result.IsSuccess)
            {
                // Retorna 400 (Placa Duplicada) ou outro erro
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            // Retorna 201 Created
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        /// <summary>
        /// Busca todas as motocicletas cadastradas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("MotoController.GetAll: Endpoint chamado");
                Console.WriteLine("MotoController.GetAll: Endpoint chamado");
                
                // Serviço retorna ApiResponse<IEnumerable<MotoResponse>>
                var result = await _service.GetAllMotosAsync();
                
                System.Diagnostics.Debug.WriteLine($"MotoController.GetAll: Result.IsSuccess: {result.IsSuccess}, Data count: {result.Data?.Count() ?? 0}");
                Console.WriteLine($"MotoController.GetAll: Result.IsSuccess: {result.IsSuccess}, Data count: {result.Data?.Count() ?? 0}");
                
                if (result.Data != null && result.Data.Any())
                {
                    var primeira = result.Data.First();
                    System.Diagnostics.Debug.WriteLine($"MotoController.GetAll: Primeira moto no response - Id: {primeira.Id}, Placa: {primeira.Placa}, Modelo: {primeira.Modelo}");
                    Console.WriteLine($"MotoController.GetAll: Primeira moto no response - Id: {primeira.Id}, Placa: {primeira.Placa}, Modelo: {primeira.Modelo}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("MotoController.GetAll: Data é null ou vazio");
                    Console.WriteLine("MotoController.GetAll: Data é null ou vazio");
                }
                
                // Assume 200 OK mesmo que a lista esteja vazia
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MotoController.GetAll: ERRO - {ex.Message}");
                Console.WriteLine($"MotoController.GetAll: ERRO - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Busca uma motocicleta por ID (Guid).
        /// Nota: O endpoint de busca por Placa precisa ser separado ou o método de serviço adaptado.
        /// </summary>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        // Renomeado para GetById para maior clareza de rota
        public async Task<IActionResult> GetById(Guid id)
        {
            // Serviço retorna ApiResponse<MotoResponse>
            var result = await _service.GetMotoByIdAsync(id);

            if (!result.IsSuccess)
            {
                // Retorna 404 Not Found
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        // Se você precisa buscar por placa, você pode adicionar um novo endpoint:
        /* [HttpGet("placa/{placa}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByPlaca(string placa)
        {
             // Você precisaria adicionar GetMotoByPlacaAsync ao IMotoService
             var result = await _service.GetMotoByPlacaAsync(placa); 
             if (!result.IsSuccess)
             {
                 return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
             }
             return Ok(result.Data);
        }
        */

        /// <summary>
        /// Atualiza os dados de uma motocicleta existente (apenas a Placa).
        /// </summary>
        [HttpPut("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // DTO CORRIGIDO: MotoUpdateRequest
        public async Task<IActionResult> Put(Guid id, [FromBody] MotoUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Serviço retorna ApiResponse<MotoResponse>
            var result = await _service.UpdateMotoAsync(id, request);
            
            if (!result.IsSuccess)
            {
                // Retorna 404 (Não encontrada), 400 (Placa duplicada/Locação ativa)
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Exclui uma motocicleta por ID (Guid).
        /// </summary>
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)] // 400 se tiver locação ativa
        public async Task<IActionResult> Delete(Guid id)
        {
            // Serviço retorna ApiResponse<bool>
            var result = await _service.DeleteMotoAsync(id);

            if (!result.IsSuccess)
            {
                // Retorna 404 (Não encontrada) ou 400 (Locação ativa)
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            // Retorna 204 No Content
            return NoContent();
        }
    }
}