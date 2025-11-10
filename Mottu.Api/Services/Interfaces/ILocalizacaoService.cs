using Mottu.Api.DTOs.LocalizacaoDtos;
using Mottu.Api.DTOs.Shared;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o serviço de Localizações.
    /// Contém a lógica de negócio para registrar e consultar o histórico de localizações.
    /// </summary>
    public interface ILocalizacaoService
    {
        /// <summary>
        /// Registra uma nova passagem de moto por um sensor.
        /// </summary>
        /// <param name="createDto">DTO com os dados para o registro da localização.</param>
        /// <returns>O DTO do registro de localização recém-criado.</returns>
        Task<ReadLocalizacaoDto> CreateAsync(CreateLocalizacaoDto createDto);

        /// <summary>
        /// Busca um registro de localização pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do registro de localização.</param>
        /// <returns>O DTO do registro encontrado ou nulo.</returns>
        Task<ReadLocalizacaoDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Busca o histórico de localizações de uma moto específica, de forma paginada.
        /// </summary>
        /// <param name="motoId">O ID da moto.</param>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Um resultado paginado com os DTOs do histórico da moto.</returns>
        Task<PagedResult<ReadLocalizacaoDto>> GetAllByMotoPaginatedAsync(Guid motoId, int pageNumber, int pageSize);

        /// <summary>
        /// Remove um registro de localização (operação administrativa).
        /// </summary>
        /// <param name="id">O ID do registro a ser removido.</param>
        Task DeleteAsync(Guid id);
    }
}