using Microsoft.AspNetCore.Mvc;
using Mottu.Api.DTOs.FilialDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FiliaisController : ControllerBase
    {
        private readonly IFilialService _filialService;

        public FiliaisController(IFilialService filialService)
        {
            _filialService = filialService;
        }

        /// <summary>
        /// Retorna uma lista paginada de filiais.
        /// </summary>
        /// <response code="200">Retorna a lista paginada de filiais.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ReadFilialDto>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _filialService.GetAllPaginatedAsync(pageNumber, pageSize);
            result.Items.ForEach(AddHateoasLinks);
            return Ok(result);
        }

        /// <summary>
        /// Busca uma filial específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da filial.</param>
        /// <response code="200">Retorna os dados da filial encontrada.</response>
        /// <response code="404">Se a filial não for encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReadFilialDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var filialDto = await _filialService.GetByIdAsync(id);
            if (filialDto == null)
            {
                return NotFound();
            }
            AddHateoasLinks(filialDto);
            return Ok(filialDto);
        }

        /// <summary>
        /// Cadastra uma nova filial.
        /// </summary>
        /// <param name="createDto">Os dados da filial a ser criada.</param>
        /// <response code="201">Retorna os dados da filial recém-criada.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ReadFilialDto), 201)]
        public async Task<IActionResult> Create([FromBody] CreateFilialDto createDto)
        {
            var createdFilial = await _filialService.CreateAsync(createDto);
            AddHateoasLinks(createdFilial);
            return CreatedAtAction(nameof(GetById), new { id = createdFilial.FilialId }, createdFilial);
        }

        /// <summary>
        /// Atualiza os dados de uma filial existente.
        /// </summary>
        /// <response code="204">Se a filial foi atualizada com sucesso.</response>
        /// <response code="404">Se a filial não for encontrada.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFilialDto updateDto)
        {
            try
            {
                await _filialService.UpdateAsync(id, updateDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove uma filial.
        /// </summary>
        /// <response code="204">Se a filial foi removida com sucesso.</response>
        /// <response code="400">Se a remoção for inválida (ex: filial com pátios).</response>
        /// <response code="404">Se a filial não for encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _filialService.DeleteAsync(id);
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

        private void AddHateoasLinks(ReadFilialDto dto)
        {
            dto.Links.Add(new LinkDto($"/api/filiais/{dto.FilialId}", "self", "GET"));
            dto.Links.Add(new LinkDto($"/api/filiais/{dto.FilialId}", "update-filial", "PUT"));
            dto.Links.Add(new LinkDto($"/api/filiais/{dto.FilialId}", "delete-filial", "DELETE"));
        }
    }
}