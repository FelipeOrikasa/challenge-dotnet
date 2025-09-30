using Mottu.Api.DTOs.MotoDtos;
using Mottu.Api.DTOs.Shared;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o serviço de Motos.
    /// Contém a lógica de negócio para gerenciar as motos da frota.
    /// </summary>
    public interface IMotoService
    {
        /// <summary>
        /// Cadastra uma nova moto no sistema, validando se a placa já existe.
        /// </summary>
        /// <param name="createDto">DTO com os dados para a criação.</param>
        /// <returns>O DTO da moto recém-criada.</returns>
        Task<ReadMotoDto> CreateAsync(CreateMotoDto createDto);

        /// <summary>
        /// Busca uma moto pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da moto.</param>
        /// <returns>O DTO da moto encontrada ou nulo.</returns>
        Task<ReadMotoDto?> GetByIdAsync(int id);

        /// <summary>
        /// Busca uma moto pela sua placa.
        /// </summary>
        /// <param name="placa">A placa da moto.</param>
        /// <returns>O DTO da moto encontrada ou nulo.</returns>
        Task<ReadMotoDto?> GetByPlacaAsync(string placa);

        /// <summary>
        /// Busca todas as motos de um pátio específico, de forma paginada.
        /// </summary>
        /// <param name="patioId">O ID do pátio.</param>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Um resultado paginado com os DTOs das motos no pátio.</returns>
        Task<PagedResult<ReadMotoDto>> GetAllByPatioPaginatedAsync(int patioId, int pageNumber, int pageSize);

        /// <summary>
        /// Atualiza o pátio de uma moto existente, efetivamente movendo-a.
        /// </summary>
        /// <param name="motoId">O ID da moto a ser atualizada.</param>
        /// <param name="novoPatioId">O ID do novo pátio para onde a moto será movida.</param>
        /// <returns>True se a atualização foi bem-sucedida, false caso contrário.</returns>
        Task UpdatePatioAsync(int motoId, int novoPatioId);

        /// <summary>
        /// Remove uma moto do sistema.
        /// </summary>
        /// <param name="id">O ID da moto a ser removida.</param>
        /// <returns>True se a remoção foi bem-sucedida, false caso contrário.</returns>
        Task DeleteAsync(int id);
    }
}