using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Utils; // **ESSENCIAL**
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Interfaces
{
    public interface ILocacaoService
    {
        // 🚨 O TIPO DE RETORNO DEVE SER ApiResponse<T>
        Task<ApiResponse<LocacaoResponseDto>> RentMotoAsync(LocacaoRequestDto dto);
        Task<ApiResponse<LocacaoResponseDto>> DevolucaoMotoAsync(Guid locacaoId, DateTime dataDevolucao);
        Task<ApiResponse<LocacaoResponseDto?>> GetLocacaoByIdAsync(Guid id); // Usa o '?' dentro do ApiResponse
        Task<ApiResponse<IEnumerable<LocacaoResponseDto>>> GetAllLocacoesAsync();
        Task<ApiResponse<LocacaoResponseDto?>> GetActiveLocacaoByEntregadorIdAsync(Guid entregadorId);
    }
}