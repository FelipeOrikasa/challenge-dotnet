using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.DTOs.MotoDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Models.DTOs.Request;
using Mottu.Api.Models.DTOs.Response;
using Mottu.Api.Models.Entities;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mottu.Api.Utils;

namespace Mottu.Api.Services
{
    public class MotoService : IMotoService
    {
        private readonly IMotoRepository _motoRepository;
        private readonly IPatioRepository _patioRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public MotoService(IMotoRepository motoRepository, IPatioRepository patioRepository, IMapper mapper, AppDbContext context)
        {
            _motoRepository = motoRepository;
            _patioRepository = patioRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResponse<MotoResponse>> AddMotoAsync(MotoRequest request)
        {
            // Validação: Placa já existe
            var placaExists = await _motoRepository.GetByPlacaAsync(request.Placa);
            if (placaExists != null)
            {
                return new ApiResponse<MotoResponse>(409, "Uma moto com esta placa já está cadastrada.");
            }

            var moto = _mapper.Map<Moto>(request);
            await _motoRepository.AddAsync(moto);
            await _context.SaveChangesAsync();

            var createdMoto = await _motoRepository.GetByIdAsync(moto.Id);
            var response = _mapper.Map<MotoResponse>(createdMoto);
            return new ApiResponse<MotoResponse>(201, response);
        }

        public async Task<ApiResponse<MotoResponse>> GetMotoByIdAsync(Guid id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
            {
                return new ApiResponse<MotoResponse>(404, "Moto não encontrada.");
            }

            var response = _mapper.Map<MotoResponse>(moto);
            return new ApiResponse<MotoResponse>(200, response);
        }

        public async Task<ApiResponse<IEnumerable<MotoResponse>>> GetAllMotosAsync()
        {
            try
            {
                var motos = await _motoRepository.GetAllPaginatedAsync(1, int.MaxValue);
                var motosList = motos.ToList();
                
                // Log para debug
                System.Diagnostics.Debug.WriteLine($"MotoService.GetAllMotosAsync: Encontradas {motosList.Count} motos do banco");
                Console.WriteLine($"MotoService.GetAllMotosAsync: Encontradas {motosList.Count} motos do banco");
                
                if (motosList.Count > 0)
                {
                    var primeiraMoto = motosList.First();
                    System.Diagnostics.Debug.WriteLine($"Primeira moto - Id: {primeiraMoto.Id}, Placa: {primeiraMoto.Placa}, Modelo: {primeiraMoto.Modelo}, Ano: {primeiraMoto.Ano}");
                    Console.WriteLine($"Primeira moto - Id: {primeiraMoto.Id}, Placa: {primeiraMoto.Placa}, Modelo: {primeiraMoto.Modelo}, Ano: {primeiraMoto.Ano}");
                }
                
                var response = _mapper.Map<IEnumerable<MotoResponse>>(motosList);
                var responseList = response.ToList();
                
                System.Diagnostics.Debug.WriteLine($"MotoService.GetAllMotosAsync: Após mapeamento, {responseList.Count} motos no response");
                Console.WriteLine($"MotoService.GetAllMotosAsync: Após mapeamento, {responseList.Count} motos no response");
                
                if (responseList.Count > 0)
                {
                    var primeiraResponse = responseList.First();
                    System.Diagnostics.Debug.WriteLine($"Primeira response - Id: {primeiraResponse.Id}, Placa: {primeiraResponse.Placa}, Modelo: {primeiraResponse.Modelo}, Ano: {primeiraResponse.Ano}");
                    Console.WriteLine($"Primeira response - Id: {primeiraResponse.Id}, Placa: {primeiraResponse.Placa}, Modelo: {primeiraResponse.Modelo}, Ano: {primeiraResponse.Ano}");
                }
                
                return new ApiResponse<IEnumerable<MotoResponse>>(200, responseList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MotoService.GetAllMotosAsync: Erro - {ex.Message}");
                Console.WriteLine($"MotoService.GetAllMotosAsync: Erro - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ApiResponse<MotoResponse>> UpdateMotoAsync(Guid id, MotoUpdateRequest request)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
            {
                return new ApiResponse<MotoResponse>(404, "Moto não encontrada.");
            }

            // Validação: Nova placa já existe
            var placaExists = await _motoRepository.GetByPlacaAsync(request.Placa);
            if (placaExists != null && placaExists.Id != moto.Id)
            {
                return new ApiResponse<MotoResponse>(409, "Uma moto com esta placa já está cadastrada.");
            }

            moto.Placa = request.Placa;
            _motoRepository.Update(moto);
            await _context.SaveChangesAsync();

            var updatedMoto = await _motoRepository.GetByIdAsync(moto.Id);
            var response = _mapper.Map<MotoResponse>(updatedMoto);
            return new ApiResponse<MotoResponse>(200, response);
        }

        public async Task<ApiResponse<bool>> DeleteMotoAsync(Guid id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
            {
                return new ApiResponse<bool>(404, "Moto não encontrada.");
            }

            _motoRepository.Delete(moto);

            try
            {
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(200, true);
            }
            catch (DbUpdateException)
            {
                return new ApiResponse<bool>(400, "Não é possível excluir uma moto com histórico de localização.");
            }
        }

        // Métodos auxiliares mantidos para compatibilidade
        public async Task<ReadMotoDto> CreateAsync(CreateMotoDto createDto)
        {
            // Regra 1: Validar se a placa já existe
            var placaExists = await _motoRepository.GetByPlacaAsync(createDto.Placa);
            if (placaExists != null)
            {
                throw new InvalidOperationException("Uma moto com esta placa já está cadastrada.");
            }

            // Regra 2: Validar se o pátio de destino existe
            var patioExists = await _patioRepository.GetByIdAsync(createDto.PatioId);
            if (patioExists == null)
            {
                throw new KeyNotFoundException("O pátio especificado não existe.");
            }

            var moto = _mapper.Map<Moto>(createDto);
            await _motoRepository.AddAsync(moto);
            await _context.SaveChangesAsync();

            var createdMoto = await _motoRepository.GetByIdAsync(moto.Id);
            return _mapper.Map<ReadMotoDto>(createdMoto);
        }

        public async Task<ReadMotoDto?> GetByIdAsync(Guid id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            return _mapper.Map<ReadMotoDto>(moto);
        }

        public async Task<ReadMotoDto?> GetByPlacaAsync(string placa)
        {
            var moto = await _motoRepository.GetByPlacaAsync(placa);
            return _mapper.Map<ReadMotoDto>(moto);
        }

        public async Task<PagedResult<ReadMotoDto>> GetAllByPatioPaginatedAsync(int patioId, int pageNumber, int pageSize)
        {
            var motos = await _motoRepository.GetAllByPatioPaginatedAsync(patioId, pageNumber, pageSize);
            var totalCount = await _motoRepository.GetCountByPatioAsync(patioId);
            var dtos = _mapper.Map<List<ReadMotoDto>>(motos);
            return new PagedResult<ReadMotoDto>(dtos, totalCount, pageNumber, pageSize);
        }

        public async Task UpdatePatioAsync(Guid motoId, int novoPatioId)
        {
            var moto = await _motoRepository.GetByIdAsync(motoId) ??
                throw new KeyNotFoundException("Moto não encontrada.");

            var patioExists = await _patioRepository.GetByIdAsync(novoPatioId) ??
                throw new KeyNotFoundException("Pátio de destino não encontrado.");

            moto.PatioId = novoPatioId;
            _motoRepository.Update(moto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var moto = await _motoRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Moto não encontrada.");

            _motoRepository.Delete(moto);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // A restrição do banco de dados foi acionada.
                throw new InvalidOperationException("Não é possível excluir uma moto com histórico de localização.");
            }
        }
    }
}