using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Interfaces
{
    /// <summary>
    /// Define os métodos de negócios para a entidade Moto.
    /// </summary>
    public interface IMotoService
    {
        Task<ApiResponse<MotoResponse>> AddMotoAsync(MotoRequest request);
        Task<ApiResponse<MotoResponse>> GetMotoByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<MotoResponse>>> GetAllMotosAsync();
        Task<ApiResponse<MotoResponse>> UpdateMotoAsync(Guid id, MotoUpdateRequest request);
        Task<ApiResponse<bool>> DeleteMotoAsync(Guid id);
    }
}