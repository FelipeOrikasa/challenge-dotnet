using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mottu.Api.DTOs.MotoDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MotosController : ControllerBase
    {
        private readonly IMotoService _motoService;

        public MotosController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        /// <summary>
        /// Retorna uma lista paginada de motos de um pátio específico.
        /// </summary>
        /// <param name="patioId">O ID do pátio.</param>
        /// <param name="pageNumber">O número da página (padrão: 1).</param>
        /// <param name="pageSize">O tamanho da página (padrão: 10).</param>
        /// <response code="200">Retorna a lista paginada de motos.</response>
        [HttpGet("patios/{patioId}/motos")]
        [ProducesResponseType(typeof(PagedResult<ReadMotoDto>), 200)]
        public async Task<IActionResult> GetAllByPatio(int patioId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _motoService.GetAllByPatioPaginatedAsync(patioId, pageNumber, pageSize);
            result.Items.ForEach(AddHateoasLinks);
            return Ok(result);
        }

        /// <summary>
        /// Busca uma moto específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da moto.</param>
        /// <response code="200">Retorna os dados da moto encontrada.</response>
        /// <response code="404">Se a moto não for encontrada.</response>
        [HttpGet("motos/{id}")]
        [ProducesResponseType(typeof(ReadMotoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var motoDto = await _motoService.GetByIdAsync(id);
            if (motoDto == null) return NotFound();

            AddHateoasLinks(motoDto);
            return Ok(motoDto);
        }

        /// <summary>
        /// Busca uma moto específica pela sua placa.
        /// </summary>
        /// <param name="placa">A placa da moto.</param>
        /// <response code="200">Retorna os dados da moto encontrada.</response>
        /// <response code="404">Se a moto não for encontrada.</response>
        [HttpGet("motos/placa/{placa}")]
        [ProducesResponseType(typeof(ReadMotoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByPlaca(string placa)
        {
            var motoDto = await _motoService.GetByPlacaAsync(placa);
            if (motoDto == null) return NotFound();

            AddHateoasLinks(motoDto);
            return Ok(motoDto);
        }

        /// <summary>
        /// Cadastra uma nova moto.
        /// </summary>
        /// <param name="createDto">Os dados da moto a ser criada.</param>
        /// <response code="201">Retorna os dados da moto recém-criada.</response>
        /// <response code="400">Se os dados forem inválidos (ex: placa duplicada).</response>
        /// <response code="404">Se o pátio especificado não for encontrado.</response>
        [HttpPost("motos")]
        [ProducesResponseType(typeof(ReadMotoDto), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create([FromBody] CreateMotoDto createDto)
        {
            try
            {
                var createdMoto = await _motoService.CreateAsync(createDto);
                AddHateoasLinks(createdMoto);
                return CreatedAtAction(nameof(GetById), new { id = createdMoto.MotoId }, createdMoto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Move uma moto para um novo pátio.
        /// </summary>
        /// <param name="id">O ID da moto a ser movida.</param>
        /// <param name="updateDto">Objeto contendo o ID do novo pátio.</param>
        /// <response code="204">Se a moto foi movida com sucesso.</response>
        /// <response code="404">Se a moto ou o pátio de destino não forem encontrados.</response>
        [HttpPut("motos/{id}/patio")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePatio(int id, [FromBody] UpdateMotoDto updateDto)
        {
            try
            {
                await _motoService.UpdatePatioAsync(id, updateDto.PatioId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove uma moto.
        /// </summary>
        /// <param name="id">O ID da moto a ser removida.</param>
        /// <response code="204">Se a moto foi removida com sucesso.</response>
        /// <response code="400">Se a remoção for inválida (ex: moto com histórico de localização).</response>
        /// <response code="404">Se a moto não for encontrada.</response>
        [HttpDelete("motos/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _motoService.DeleteAsync(id);
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

        private void AddHateoasLinks(ReadMotoDto dto)
        {
            dto.Links.Add(new LinkDto($"/api/motos/{dto.MotoId}", "self", "GET"));
            dto.Links.Add(new LinkDto($"/api/motos/{dto.MotoId}/patio", "update-patio", "PUT"));
            dto.Links.Add(new LinkDto($"/api/motos/{dto.MotoId}", "delete-moto", "DELETE"));
            dto.Links.Add(new LinkDto($"/api/patios/{dto.PatioId}", "get-patio", "GET"));
        }
    }
}