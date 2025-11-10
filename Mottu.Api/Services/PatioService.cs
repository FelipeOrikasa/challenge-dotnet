using AutoMapper;
using Mottu.Api.Data;
using Mottu.Api.DTOs.PatioDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Models.Entities;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mottu.Api.Utils;
namespace Mottu.Api.Services
{
    public class PatioService : IPatioService
    {
        private readonly IPatioRepository _patioRepository;
        private readonly IFilialRepository _filialRepository;
        private readonly IMotoRepository _motoRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public PatioService(IPatioRepository patioRepository, IFilialRepository filialRepository, IMotoRepository motoRepository, IMapper mapper, AppDbContext context)
        {
            _patioRepository = patioRepository;
            _filialRepository = filialRepository;
            _motoRepository = motoRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ReadPatioDto> CreateAsync(CreatePatioDto createDto)
        {
            // Regra de negócio: Garante que a filial especificada existe.
            var filialExists = await _filialRepository.GetByIdAsync(createDto.FilialId);
            if (filialExists == null)
            {
                throw new KeyNotFoundException("A filial especificada para o pátio não existe.");
            }

            var patio = _mapper.Map<Patio>(createDto);
            await _patioRepository.AddAsync(patio);
            await _context.SaveChangesAsync();

            // Recarrega a entidade com os dados da filial para retornar um DTO completo.
            var createdPatio = await _patioRepository.GetByIdAsync(patio.Id);
            return _mapper.Map<ReadPatioDto>(createdPatio);
        }

        public async Task<ReadPatioDto?> GetByIdAsync(int id)
        {
            var patio = await _patioRepository.GetByIdAsync(id);
            return _mapper.Map<ReadPatioDto>(patio);
        }

        public async Task<PagedResult<ReadPatioDto>> GetAllByFilialPaginatedAsync(int filialId, int pageNumber, int pageSize)
        {
            var patios = await _patioRepository.GetAllByFilialPaginatedAsync(filialId, pageNumber, pageSize);
            var totalCount = await _patioRepository.GetCountByFilialAsync(filialId);
            var dtos = _mapper.Map<List<ReadPatioDto>>(patios);
            return new PagedResult<ReadPatioDto>(dtos, totalCount, pageNumber, pageSize);
        }

        public async Task UpdateAsync(int id, UpdatePatioDto updateDto)
        {
            var patio = await _patioRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Pátio não encontrado.");

            _mapper.Map(updateDto, patio);
            _patioRepository.Update(patio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patio = await _patioRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Pátio não encontrado.");

            var motoCount = await _motoRepository.GetCountByPatioAsync(id);
            if (motoCount > 0)
            {
                throw new InvalidOperationException("Não é possível excluir um pátio que contém motos.");
            }

            _patioRepository.Delete(patio);
            await _context.SaveChangesAsync();
        }
    }
}