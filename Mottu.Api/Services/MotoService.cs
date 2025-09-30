using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.DTOs.MotoDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            var createdMoto = await _motoRepository.GetByIdAsync(moto.MotoId);
            return _mapper.Map<ReadMotoDto>(createdMoto);
        }

        public async Task<ReadMotoDto?> GetByIdAsync(int id)
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

        public async Task UpdatePatioAsync(int motoId, int novoPatioId)
        {
            var moto = await _motoRepository.GetByIdAsync(motoId) ??
                throw new KeyNotFoundException("Moto não encontrada.");

            var patioExists = await _patioRepository.GetByIdAsync(novoPatioId) ??
                throw new KeyNotFoundException("Pátio de destino não encontrado.");

            moto.PatioId = novoPatioId;
            _motoRepository.Update(moto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
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