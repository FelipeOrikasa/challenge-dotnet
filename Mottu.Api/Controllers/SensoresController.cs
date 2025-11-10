using Microsoft.AspNetCore.Mvc;
using Mottu.Api.DTOs.SensorDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class SensoresController : ControllerBase
    {
        private readonly ISensorService _sensorService;

        public SensoresController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        /// <summary>
        /// Retorna uma lista paginada de sensores de um pátio específico.
        /// </summary>
        /// <param name="patioId">O ID do pátio.</param>
        /// <param name="pageNumber">O número da página (padrão: 1).</param>
        /// <param name="pageSize">O tamanho da página (padrão: 10).</param>
        /// <response code="200">Retorna a lista paginada de sensores.</response>
        [HttpGet("patios/{patioId}/sensores")]
        [ProducesResponseType(typeof(PagedResult<ReadSensorDto>), 200)]
        public async Task<IActionResult> GetAllByPatio(int patioId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"SensoresController.GetAllByPatio: Endpoint chamado - patioId: {patioId}, pageNumber: {pageNumber}, pageSize: {pageSize}");
                Console.WriteLine($"SensoresController.GetAllByPatio: Endpoint chamado - patioId: {patioId}, pageNumber: {pageNumber}, pageSize: {pageSize}");
                
                var result = await _sensorService.GetAllByPatioPaginatedAsync(patioId, pageNumber, pageSize);
                
                System.Diagnostics.Debug.WriteLine($"SensoresController.GetAllByPatio: Result.Items.Count: {result.Items.Count}, TotalCount: {result.TotalCount}");
                Console.WriteLine($"SensoresController.GetAllByPatio: Result.Items.Count: {result.Items.Count}, TotalCount: {result.TotalCount}");
                
                if (result.Items.Count > 0)
                {
                    var primeiro = result.Items.First();
                    System.Diagnostics.Debug.WriteLine($"SensoresController.GetAllByPatio: Primeiro sensor - Id: {primeiro.SensorId}, Descricao: {primeiro.Descricao}");
                    Console.WriteLine($"SensoresController.GetAllByPatio: Primeiro sensor - Id: {primeiro.SensorId}, Descricao: {primeiro.Descricao}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("SensoresController.GetAllByPatio: Items é vazio");
                    Console.WriteLine("SensoresController.GetAllByPatio: Items é vazio");
                }
                
                result.Items.ForEach(AddHateoasLinks);
                return Ok(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SensoresController.GetAllByPatio: ERRO - {ex.Message}");
                Console.WriteLine($"SensoresController.GetAllByPatio: ERRO - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Busca um sensor específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do sensor.</param>
        /// <response code="200">Retorna os dados do sensor encontrado.</response>
        /// <response code="404">Se o sensor não for encontrado.</response>
        [HttpGet("sensores/{id}")]
        [ProducesResponseType(typeof(ReadSensorDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var sensorDto = await _sensorService.GetByIdAsync(id);
            if (sensorDto == null) return NotFound();

            AddHateoasLinks(sensorDto);
            return Ok(sensorDto);
        }

        /// <summary>
        /// Cadastra um novo sensor em um pátio.
        /// </summary>
        /// <param name="createDto">Os dados do sensor a ser criado.</param>
        /// <response code="201">Retorna os dados do sensor recém-criado.</response>
        /// <response code="404">Se o pátio especificado não for encontrado.</response>
        [HttpPost("sensores")]
        [ProducesResponseType(typeof(ReadSensorDto), 201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create([FromBody] CreateSensorDto createDto)
        {
            try
            {
                var createdSensor = await _sensorService.CreateAsync(createDto);
                AddHateoasLinks(createdSensor);
                return CreatedAtAction(nameof(GetById), new { id = createdSensor.SensorId }, createdSensor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza a descrição de um sensor existente.
        /// </summary>
        /// <param name="id">O ID do sensor a ser atualizado.</param>
        /// <param name="updateDto">A nova descrição do sensor.</param>
        /// <response code="204">Se o sensor foi atualizado com sucesso.</response>
        /// <response code="404">Se o sensor não for encontrado.</response>
        [HttpPut("sensores/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSensorDto updateDto)
        {
            try
            {
                await _sensorService.UpdateAsync(id, updateDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove um sensor.
        /// </summary>
        /// <param name="id">O ID do sensor a ser removido.</param>
        /// <response code="204">Se o sensor foi removido com sucesso.</response>
        /// <response code="400">Se a remoção for inválida (ex: sensor com histórico de localização).</response>
        /// <response code="404">Se o sensor não for encontrado.</response>
        [HttpDelete("sensores/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sensorService.DeleteAsync(id);
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

        private void AddHateoasLinks(ReadSensorDto dto)
        {
            dto.Links.Add(new LinkDto($"/api/sensores/{dto.SensorId}", "self", "GET"));
            dto.Links.Add(new LinkDto($"/api/sensores/{dto.SensorId}", "update-sensor", "PUT"));
            dto.Links.Add(new LinkDto($"/api/sensores/{dto.SensorId}", "delete-sensor", "DELETE"));
            dto.Links.Add(new LinkDto($"/api/patios/{dto.PatioId}", "get-patio", "GET"));
        }
    }
}