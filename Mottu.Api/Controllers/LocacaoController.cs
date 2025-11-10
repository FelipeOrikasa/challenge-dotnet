using Microsoft.AspNetCore.Mvc;
using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Services.Interfaces;
using Mottu.Api.Utils; // ESSENCIAL: Garante que ApiResponse seja encontrado
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/locacao")]
    public class LocacaoController : ControllerBase
    {
        private readonly ILocacaoService _service;

        public LocacaoController(ILocacaoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Inicia uma nova locação de moto para um entregador.
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Rent([FromBody] LocacaoRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Variável 'result' recebe o ApiResponse<LocacaoResponseDto> completo
            var result = await _service.RentMotoAsync(request);

            if (!result.IsSuccess)
            {
                // Acessa o StatusCode e ErrorMessage do ApiResponse
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            // Retorna 201 Created, acessando o DTO pelo .Data
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        /// <summary>
        /// Registra a devolução de uma moto e calcula o custo final.
        /// </summary>
        /// <param name="id">ID da locação ativa.</param>
        /// <param name="dataDevolucao">Data efetiva da devolução (formato yyyy-MM-dd).</param>
        [HttpPut("{id:Guid}/devolucao")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReturnMoto(Guid id, [FromQuery] DateTime dataDevolucao)
        {
            if (dataDevolucao == default || dataDevolucao > DateTime.Now.Date)
            {
                return BadRequest(new { error = "A data de devolução é obrigatória, válida e não pode ser futura." });
            }

            // Variável 'result' recebe o ApiResponse<LocacaoResponseDto> completo
            var result = await _service.DevolucaoMotoAsync(id, dataDevolucao.Date);

            if (!result.IsSuccess)
            {
                // Acessa o StatusCode e ErrorMessage do ApiResponse
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            // Retorna 200 OK, acessando o DTO pelo .Data
            return Ok(result.Data);
        }

        /// <summary>
        /// Busca uma locação pelo ID.
        /// </summary>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Variável 'result' recebe o ApiResponse<LocacaoResponseDto?> completo
            var result = await _service.GetLocacaoByIdAsync(id);

            if (!result.IsSuccess)
            {
                // Acessa o StatusCode e ErrorMessage do ApiResponse
                return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
            }

            // Retorna 200 OK, acessando o DTO pelo .Data
            return Ok(result.Data);
        }

        /// <summary>
        /// Lista todas as locações.
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("LocacaoController.GetAll: Endpoint chamado");
                Console.WriteLine("LocacaoController.GetAll: Endpoint chamado");
                
                // O serviço retorna ApiResponse<IEnumerable<LocacaoResponseDto>>
                var result = await _service.GetAllLocacoesAsync();
                
                System.Diagnostics.Debug.WriteLine($"LocacaoController.GetAll: Result.IsSuccess: {result.IsSuccess}, Data count: {result.Data?.Count() ?? 0}");
                Console.WriteLine($"LocacaoController.GetAll: Result.IsSuccess: {result.IsSuccess}, Data count: {result.Data?.Count() ?? 0}");
                
                if (result.Data != null && result.Data.Any())
                {
                    var primeira = result.Data.First();
                    System.Diagnostics.Debug.WriteLine($"LocacaoController.GetAll: Primeira locacao no response - Id: {primeira.Id}");
                    Console.WriteLine($"LocacaoController.GetAll: Primeira locacao no response - Id: {primeira.Id}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("LocacaoController.GetAll: Data é null ou vazio");
                    Console.WriteLine("LocacaoController.GetAll: Data é null ou vazio");
                }
                
                // Assume 200 OK. Acessa a lista pelo .Data
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LocacaoController.GetAll: ERRO - {ex.Message}");
                Console.WriteLine($"LocacaoController.GetAll: ERRO - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}