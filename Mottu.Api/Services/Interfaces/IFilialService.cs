using Mottu.Api.DTOs.FilialDtos;
using Mottu.Api.DTOs.Shared;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Interfaces
{
    /// <summary>
    /// Contrato de serviço para operações da entidade Filial.
    /// </summary>
    public interface IFilialService
    {
        Task<ReadFilialDto> CreateAsync(CreateFilialDto createDto);
        Task<ReadFilialDto?> GetByIdAsync(int id);
        Task<PagedResult<ReadFilialDto>> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task UpdateAsync(int id, UpdateFilialDto updateDto);
        Task DeleteAsync(int id);
    }
}