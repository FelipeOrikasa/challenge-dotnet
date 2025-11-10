using AutoMapper;
using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Models.Entities;
using Mottu.Api.Models.Enums;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services.Interfaces;
using Mottu.Api.Utils; // Garante que ApiResponse seja encontrado
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Services.Implementations
{
    public class EntregadorService : IEntregadorService
    {
        private readonly IEntregadorRepository _entregadorRepository;
        private readonly ILocacaoRepository _locacaoRepository;
        private readonly IMapper _mapper;

        public EntregadorService(IEntregadorRepository entregadorRepository, ILocacaoRepository locacaoRepository, IMapper mapper)
        {
            _entregadorRepository = entregadorRepository;
            _locacaoRepository = locacaoRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<EntregadorResponse>> AddEntregadorAsync(EntregadorRequest request)
        {
            // Validações de unicidade (CNPJ e CNH)
            if (await _entregadorRepository.GetByCNPJAsync(request.CNPJ) != null)
            {
                return new ApiResponse<EntregadorResponse>(400, "CNPJ já cadastrado.");
            }
            if (await _entregadorRepository.GetByCNHAsync(request.CNH) != null)
            {
                return new ApiResponse<EntregadorResponse>(400, "CNH já cadastrada.");
            }

            // Validação do tipo de CNH (deve ser 'A' ou 'AB')
            if (request.TipoCNH != "A" && request.TipoCNH != "AB")
            {
                 return new ApiResponse<EntregadorResponse>(400, "TipoCNH inválido. Deve ser 'A' ou 'AB'.");
            }

            var entregador = _mapper.Map<Entregador>(request);
            entregador.Id = Guid.NewGuid();

            await _entregadorRepository.AddAsync(entregador);

            var response = _mapper.Map<EntregadorResponse>(entregador);
            return new ApiResponse<EntregadorResponse>(201, response);
        }

        public async Task<ApiResponse<EntregadorResponse>> GetEntregadorByIdAsync(Guid id)
        {
            var entregador = await _entregadorRepository.GetByIdAsync(id);
            if (entregador == null)
            {
                return new ApiResponse<EntregadorResponse>(404, "Entregador não encontrado.");
            }

            var response = _mapper.Map<EntregadorResponse>(entregador);
            return new ApiResponse<EntregadorResponse>(200, response);
        }

        public async Task<ApiResponse<IEnumerable<EntregadorResponse>>> GetAllEntregadoresAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("EntregadorService.GetAllEntregadoresAsync: Iniciando consulta");
                Console.WriteLine("EntregadorService.GetAllEntregadoresAsync: Iniciando consulta");
                
                var entregadores = await _entregadorRepository.GetAllAsync();
                var entregadoresList = entregadores.ToList();
                
                System.Diagnostics.Debug.WriteLine($"EntregadorService.GetAllEntregadoresAsync: Encontrados {entregadoresList.Count} entregadores do banco");
                Console.WriteLine($"EntregadorService.GetAllEntregadoresAsync: Encontrados {entregadoresList.Count} entregadores do banco");
                
                if (entregadoresList.Count > 0)
                {
                    var primeiro = entregadoresList.First();
                    System.Diagnostics.Debug.WriteLine($"Primeiro entregador - Id: {primeiro.Id}, Nome: {primeiro.Nome}, CNPJ: {primeiro.CNPJ}, CNH: {primeiro.CNH}");
                    Console.WriteLine($"Primeiro entregador - Id: {primeiro.Id}, Nome: {primeiro.Nome}, CNPJ: {primeiro.CNPJ}, CNH: {primeiro.CNH}");
                }
                
                var response = _mapper.Map<IEnumerable<EntregadorResponse>>(entregadoresList);
                var responseList = response.ToList();
                
                System.Diagnostics.Debug.WriteLine($"EntregadorService.GetAllEntregadoresAsync: Após mapeamento, {responseList.Count} entregadores no response");
                Console.WriteLine($"EntregadorService.GetAllEntregadoresAsync: Após mapeamento, {responseList.Count} entregadores no response");
                
                if (responseList.Count > 0)
                {
                    var primeiroResponse = responseList.First();
                    System.Diagnostics.Debug.WriteLine($"Primeiro response - Id: {primeiroResponse.Id}, Nome: {primeiroResponse.Nome}");
                    Console.WriteLine($"Primeiro response - Id: {primeiroResponse.Id}, Nome: {primeiroResponse.Nome}");
                }
                
                return new ApiResponse<IEnumerable<EntregadorResponse>>(200, responseList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EntregadorService.GetAllEntregadoresAsync: ERRO - {ex.Message}");
                Console.WriteLine($"EntregadorService.GetAllEntregadoresAsync: ERRO - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ApiResponse<EntregadorResponse>> UpdateEntregadorAsync(Guid id, EntregadorUpdateRequest request)
        {
            var existingEntregador = await _entregadorRepository.GetByIdAsync(id);
            if (existingEntregador == null)
            {
                return new ApiResponse<EntregadorResponse>(404, "Entregador não encontrado.");
            }

            // Validação de unicidade apenas se CNH for alterada
            if (request.CNH != existingEntregador.CNH && await _entregadorRepository.GetByCNHAsync(request.CNH) != null)
            {
                return new ApiResponse<EntregadorResponse>(400, "Nova CNH já cadastrada para outro usuário.");
            }
            
            _mapper.Map(request, existingEntregador);
            
            await _entregadorRepository.UpdateAsync(existingEntregador);

            var response = _mapper.Map<EntregadorResponse>(existingEntregador);
            return new ApiResponse<EntregadorResponse>(200, response);
        }

        public async Task<ApiResponse<bool>> DeleteEntregadorAsync(Guid id)
        {
             var existingEntregador = await _entregadorRepository.GetByIdAsync(id);
            if (existingEntregador == null)
            {
                return new ApiResponse<bool>(404, "Entregador não encontrado.");
            }
            
            // Verifica se o entregador possui locações ativas
            var activeLocacao = await _locacaoRepository.GetActiveByEntregadorIdAsync(id);
            if (activeLocacao != null)
            {
                return new ApiResponse<bool>(400, "Entregador não pode ser deletado: possui uma locação ativa.");
            }

            await _entregadorRepository.DeleteAsync(id);
            return new ApiResponse<bool>(200, true);
        }
    }
}