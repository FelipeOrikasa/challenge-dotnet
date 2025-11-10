using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Utils; // Adicionado para ApiResponse
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Interfaces
{
    /// <summary>
    /// Define os métodos de negócios para a entidade Entregador.
    /// </summary>
    public interface IEntregadorService
    {
        Task<ApiResponse<EntregadorResponse>> AddEntregadorAsync(EntregadorRequest request);
        Task<ApiResponse<EntregadorResponse>> GetEntregadorByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<EntregadorResponse>>> GetAllEntregadoresAsync();
        Task<ApiResponse<EntregadorResponse>> UpdateEntregadorAsync(Guid id, EntregadorUpdateRequest request);
        Task<ApiResponse<bool>> DeleteEntregadorAsync(Guid id); // Retorna ApiResponse<bool> para mensagens de erro claras
    }
}