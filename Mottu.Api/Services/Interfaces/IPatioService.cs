using Mottu.Api.DTOs.PatioDtos;
using Mottu.Api.DTOs.Shared;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o serviço de Pátios.
    /// Contém a lógica de negócio para gerenciar os pátios de uma filial.
    /// </summary>
    public interface IPatioService
    {
        /// <summary>
        /// Cria um novo pátio vinculado a uma filial.
        /// </summary>
        /// <param name="createDto">DTO com os dados para a criação.</param>
        /// <returns>O DTO do pátio recém-criado.</returns>
        Task<ReadPatioDto> CreateAsync(CreatePatioDto createDto);

        /// <summary>
        /// Busca um pátio pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do pátio.</param>
        /// <returns>O DTO do pátio encontrado ou nulo.</returns>
        Task<ReadPatioDto?> GetByIdAsync(int id);

        /// <summary>
        /// Busca todos os pátios de uma filial específica, de forma paginada.
        /// </summary>
        /// <param name="filialId">O ID da filial à qual os pátios pertencem.</param>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Um resultado paginado com os DTOs dos pátios da filial.</returns>
        Task<PagedResult<ReadPatioDto>> GetAllByFilialPaginatedAsync(int filialId, int pageNumber, int pageSize);

        /// <summary>
        /// Atualiza um pátio existente.
        /// </summary>
        /// <param name="id">O ID do pátio a ser atualizado.</param>
        /// <param name="updateDto">DTO com os novos dados.</param>
        /// <returns>True se a atualização foi bem-sucedida, false caso contrário.</returns>
        Task UpdateAsync(int id, UpdatePatioDto updateDto);

        /// <summary>
        /// Remove um pátio.
        /// </summary>
        /// <param name="id">O ID do pátio a ser removido.</param>
        /// <returns>True se a remoção foi bem-sucedida, false caso contrário.</returns>
        Task DeleteAsync(int id);
    }
}