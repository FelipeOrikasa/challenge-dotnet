using Microsoft.AspNetCore.Mvc;
using Mottu.Api.DTOs.LocalizacaoDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class LocalizacoesController : ControllerBase
    {
        private readonly ILocalizacaoService _localizacaoService;

        public LocalizacoesController(ILocalizacaoService localizacaoService)
        {
            _localizacaoService = localizacaoService;
        }

        /// <summary>
        /// Retorna o histórico de localizações de uma moto específica, de forma paginada.
        /// </summary>
        /// <param name="motoId">O ID da moto a ser consultada.</param>
        /// <param name="pageNumber">O número da página (padrão: 1).</param>
        /// <param name="pageSize">O tamanho da página (padrão: 10).</param>
        /// <response code="200">Retorna a lista paginada de registros de localização.</response>
        [HttpGet("motos/{motoId}/localizacoes")]
        [ProducesResponseType(typeof(PagedResult<ReadLocalizacaoDto>), 200)]
        public async Task<IActionResult> GetAllByMoto(int motoId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _localizacaoService.GetAllByMotoPaginatedAsync(motoId, pageNumber, pageSize);
            result.Items.ForEach(AddHateoasLinks);
            return Ok(result);
        }

        /// <summary>
        /// Busca um registro de localização específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do registro de localização.</param>
        /// <response code="200">Retorna o registro de localização encontrado.</response>
        /// <response code="404">Se o registro não for encontrado.</response>
        [HttpGet("localizacoes/{id}")]
        [ProducesResponseType(typeof(ReadLocalizacaoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _localizacaoService.GetByIdAsync(id);
            if (dto == null) return NotFound();

            AddHateoasLinks(dto);
            return Ok(dto);
        }

        /// <summary>
        /// Registra um novo evento de localização (uma moto passando por um sensor).
        /// </summary>
        /// <param name="createDto">Dados do evento (ID da moto e ID do sensor).</param>
        /// <response code="201">Retorna o registro do evento recém-criado.</response>
        /// <response code="404">Se a moto ou o sensor especificados não forem encontrados.</response>
        [HttpPost("localizacoes")]
        [ProducesResponseType(typeof(ReadLocalizacaoDto), 201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create([FromBody] CreateLocalizacaoDto createDto)
        {
            try
            {
                var createdLocalizacao = await _localizacaoService.CreateAsync(createDto);
                AddHateoasLinks(createdLocalizacao);
                return CreatedAtAction(nameof(GetById), new { id = createdLocalizacao.LocalizacaoId }, createdLocalizacao);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Remove um registro de localização (operação administrativa).
        /// </summary>
        /// <param name="id">O ID do registro a ser removido.</param>
        /// <response code="204">Se o registro foi removido com sucesso.</response>
        /// <response code="404">Se o registro não for encontrado.</response>
        [HttpDelete("localizacoes/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _localizacaoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        private void AddHateoasLinks(ReadLocalizacaoDto dto)
        {
            dto.Links.Add(new LinkDto($"/api/localizacoes/{dto.LocalizacaoId}", "self", "GET"));
            dto.Links.Add(new LinkDto($"/api/motos/{dto.MotoId}", "get-moto", "GET"));
            dto.Links.Add(new LinkDto($"/api/sensores/{dto.SensorId}", "get-sensor", "GET"));
        }
    }
}