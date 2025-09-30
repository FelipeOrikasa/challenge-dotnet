using Mottu.Api.DTOs.FilialDtos;
using Mottu.Api.DTOs.Shared;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o serviço de Filiais.
    /// Contém a lógica de negócio para gerenciar as filiais.
    /// </summary>
    public interface IFilialService
    {
        /// <summary>
        /// Cria uma nova filial.
        /// </summary>
        /// <param name="createDto">DTO com os dados para a criação.</param>
        /// <returns>O DTO da filial recém-criada.</returns>
        Task<ReadFilialDto> CreateAsync(CreateFilialDto createDto);

        /// <summary>
        /// Busca uma filial pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da filial.</param>
        /// <returns>O DTO da filial encontrada ou nulo.</returns>
        Task<ReadFilialDto?> GetByIdAsync(int id);

        /// <summary>
        /// Busca todas as filiais de forma paginada.
        /// </summary>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Um resultado paginado com os DTOs das filiais.</returns>
        Task<PagedResult<ReadFilialDto>> GetAllPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Atualiza uma filial existente.
        /// </summary>
        /// <param name="id">O ID da filial a ser atualizada.</param>
        /// <param name="updateDto">DTO com os novos dados.</param>
        /// <returns>True se a atualização foi bem-sucedida, false caso contrário.</returns>
        Task UpdateAsync(int id, UpdateFilialDto updateDto);

        /// <summary>
        /// Remove uma filial.
        /// </summary>
        /// <param name="id">O ID da filial a ser removida.</param>
        /// <returns>True se a remoção foi bem-sucedida, false caso contrário.</returns>
        Task DeleteAsync(int id);
    }
}