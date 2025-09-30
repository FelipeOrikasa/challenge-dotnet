using AutoMapper;
using Mottu.Api.Data;
using Mottu.Api.DTOs.FilialDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Services
{
    public class FilialService : IFilialService
    {
        private readonly IFilialRepository _filialRepository;
        private readonly IPatioRepository _patioRepository; // Injetado para regras de negócio
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public FilialService(IFilialRepository filialRepository, IPatioRepository patioRepository, IMapper mapper, AppDbContext context)
        {
            _filialRepository = filialRepository;
            _patioRepository = patioRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ReadFilialDto> CreateAsync(CreateFilialDto createDto)
        {
            var filial = _mapper.Map<Filial>(createDto);
            await _filialRepository.AddAsync(filial);
            await _context.SaveChangesAsync(); // Efetiva a transação no banco
            return _mapper.Map<ReadFilialDto>(filial);
        }

        public async Task<ReadFilialDto?> GetByIdAsync(int id)
        {
            var filial = await _filialRepository.GetByIdAsync(id);
            return _mapper.Map<ReadFilialDto>(filial);
        }

        public async Task<PagedResult<ReadFilialDto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var filiais = await _filialRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var totalCount = await _filialRepository.GetCountAsync();
            var dtos = _mapper.Map<List<ReadFilialDto>>(filiais);
            return new PagedResult<ReadFilialDto>(dtos, totalCount, pageNumber, pageSize);
        }

        public async Task UpdateAsync(int id, UpdateFilialDto updateDto)
        {
            var filial = await _filialRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Filial não encontrada.");

            _mapper.Map(updateDto, filial);
            _filialRepository.Update(filial);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var filial = await _filialRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Filial não encontrada.");

            var patioCount = await _patioRepository.GetCountByFilialAsync(id);
            if (patioCount > 0)
            {
                throw new InvalidOperationException("Não é possível excluir uma filial que possui pátios.");
            }

            _filialRepository.Delete(filial);
            await _context.SaveChangesAsync();
        }
    }
}