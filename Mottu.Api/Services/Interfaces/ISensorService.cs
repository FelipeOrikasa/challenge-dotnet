using Mottu.Api.DTOs.SensorDtos;
using Mottu.Api.DTOs.Shared;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o serviço de Sensores.
    /// Contém a lógica de negócio para gerenciar os sensores instalados nos pátios.
    /// </summary>
    public interface ISensorService
    {
        /// <summary>
        /// Cadastra um novo sensor em um pátio.
        /// </summary>
        /// <param name="createDto">DTO com os dados para a criação do sensor.</param>
        /// <returns>O DTO do sensor recém-criado.</returns>
        Task<ReadSensorDto> CreateAsync(CreateSensorDto createDto);

        /// <summary>
        /// Busca um sensor pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do sensor.</param>
        /// <returns>O DTO do sensor encontrado ou nulo.</returns>
        Task<ReadSensorDto?> GetByIdAsync(int id);

        /// <summary>
        /// Busca todos os sensores de um pátio específico, de forma paginada.
        /// </summary>
        /// <param name="patioId">O ID do pátio.</param>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Um resultado paginado com os DTOs dos sensores no pátio.</returns>
        Task<PagedResult<ReadSensorDto>> GetAllByPatioPaginatedAsync(int patioId, int pageNumber, int pageSize);

        /// <summary>
        /// Atualiza a descrição de um sensor existente.
        /// </summary>
        /// <param name="id">O ID do sensor a ser atualizado.</param>
        /// <param name="updateDto">DTO com os novos dados.</param>
        Task UpdateAsync(int id, UpdateSensorDto updateDto);

        /// <summary>
        /// Remove um sensor do sistema.
        /// </summary>
        /// <param name="id">O ID do sensor a ser removido.</param>
        Task DeleteAsync(int id);
    }
}