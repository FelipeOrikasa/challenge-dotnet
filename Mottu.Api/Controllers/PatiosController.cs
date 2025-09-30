using Microsoft.AspNetCore.Mvc;
using Mottu.Api.DTOs.PatioDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class PatiosController : ControllerBase
    {
        private readonly IPatioService _patioService;

        public PatiosController(IPatioService patioService)
        {
            _patioService = patioService;
        }

        /// <summary>
        /// Retorna uma lista paginada de pátios de uma filial específica.
        /// </summary>
        /// <param name="filialId">O ID da filial.</param>
        /// <param name="pageNumber">O número da página (padrão: 1).</param>
        /// <param name="pageSize">O tamanho da página (padrão: 10).</param>
        /// <response code="200">Retorna a lista paginada de pátios.</response>
        [HttpGet("filiais/{filialId}/patios")]
        [ProducesResponseType(typeof(PagedResult<ReadPatioDto>), 200)]
        public async Task<IActionResult> GetAllByFilial(int filialId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _patioService.GetAllByFilialPaginatedAsync(filialId, pageNumber, pageSize);
            result.Items.ForEach(p => AddHateoasLinks(p));
            return Ok(result);
        }

        /// <summary>
        /// Busca um pátio específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do pátio.</param>
        /// <response code="200">Retorna os dados do pátio encontrado.</response>
        /// <response code="404">Se o pátio não for encontrado.</response>
        [HttpGet("patios/{id}")]
        [ProducesResponseType(typeof(ReadPatioDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var patioDto = await _patioService.GetByIdAsync(id);
            if (patioDto == null) return NotFound();

            AddHateoasLinks(patioDto);
            return Ok(patioDto);
        }

        /// <summary>
        /// Cadastra um novo pátio para uma filial.
        /// </summary>
        /// <param name="createDto">Os dados do pátio a ser criado.</param>
        /// <response code="201">Retorna os dados do pátio recém-criado.</response>
        /// <response code="404">Se a filial especificada não for encontrada.</response>
        [HttpPost("patios")]
        [ProducesResponseType(typeof(ReadPatioDto), 201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create([FromBody] CreatePatioDto createDto)
        {
            try
            {
                var createdPatio = await _patioService.CreateAsync(createDto);
                AddHateoasLinks(createdPatio);
                return CreatedAtAction(nameof(GetById), new { id = createdPatio.PatioId }, createdPatio);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza o nome de um pátio existente.
        /// </summary>
        /// <param name="id">O ID do pátio a ser atualizado.</param>
        /// <param name="updateDto">Os novos dados do pátio.</param>
        /// <response code="204">Se o pátio foi atualizado com sucesso.</response>
        /// <response code="404">Se o pátio não for encontrado.</response>
        [HttpPut("patios/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatioDto updateDto)
        {
            try
            {
                await _patioService.UpdateAsync(id, updateDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove um pátio.
        /// </summary>
        /// <param name="id">O ID do pátio a ser removido.</param>
        /// <response code="204">Se o pátio foi removido com sucesso.</response>
        /// <response code="400">Se a remoção for inválida (ex: pátio com motos).</response>
        /// <response code="404">Se o pátio não for encontrado.</response>
        [HttpDelete("patios/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _patioService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private void AddHateoasLinks(ReadPatioDto dto)
        {
            dto.Links.Add(new LinkDto($"/api/patios/{dto.PatioId}", "self", "GET"));
            dto.Links.Add(new LinkDto($"/api/patios/{dto.PatioId}", "update-patio", "PUT"));
            dto.Links.Add(new LinkDto($"/api/patios/{dto.PatioId}", "delete-patio", "DELETE"));
            dto.Links.Add(new LinkDto($"/api/filiais/{dto.FilialId}", "get-filial", "GET"));
        }
    }
}