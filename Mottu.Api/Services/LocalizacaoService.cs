using AutoMapper;
using Mottu.Api.Data;
using Mottu.Api.DTOs.LocalizacaoDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Models.Entities;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mottu.Api.Utils;
namespace Mottu.Api.Services
{
    public class LocalizacaoService : ILocalizacaoService
    {
        private readonly ILocalizacaoRepository _localizacaoRepository;
        private readonly IMotoRepository _motoRepository;
        private readonly ISensorRepository _sensorRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public LocalizacaoService(
            ILocalizacaoRepository localizacaoRepository,
            IMotoRepository motoRepository,
            ISensorRepository sensorRepository,
            IMapper mapper,
            AppDbContext context)
        {
            _localizacaoRepository = localizacaoRepository;
            _motoRepository = motoRepository;
            _sensorRepository = sensorRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ReadLocalizacaoDto> CreateAsync(CreateLocalizacaoDto createDto)
        {
            // Regra 1: Validar se a moto existe (para garantir que a moto está no sistema)
            var motoExists = await _motoRepository.GetByIdAsync(createDto.MotoId);
            if (motoExists == null)
            {
                throw new KeyNotFoundException("A moto especificada não existe.");
            }

            // Regra 2: Validar se o sensor existe
            var sensorExists = await _sensorRepository.GetByIdAsync(createDto.SensorId) ??
                throw new KeyNotFoundException("O sensor especificado não existe.");

            var localizacao = _mapper.Map<Localizacao>(createDto);

            // Regra 3: A data e hora do evento são sempre geradas pelo servidor
            localizacao.Timestamp = System.DateTime.UtcNow;
            localizacao.Id = Guid.NewGuid();
            // SensorId já é int, não precisa de conversão

            await _localizacaoRepository.AddAsync(localizacao);
            await _context.SaveChangesAsync();

            var createdLocalizacao = await _localizacaoRepository.GetByIdAsync(localizacao.Id);
            return _mapper.Map<ReadLocalizacaoDto>(createdLocalizacao);
        }

        public async Task<ReadLocalizacaoDto?> GetByIdAsync(Guid id)
        {
            var localizacao = await _localizacaoRepository.GetByIdAsync(id);
            return _mapper.Map<ReadLocalizacaoDto>(localizacao);
        }

        public async Task<PagedResult<ReadLocalizacaoDto>> GetAllByMotoPaginatedAsync(Guid motoId, int pageNumber, int pageSize)
        {
            var localizacoes = await _localizacaoRepository.GetAllByMotoPaginatedAsync(motoId, pageNumber, pageSize);
            var totalCount = await _localizacaoRepository.GetCountByMotoAsync(motoId);
            var dtos = _mapper.Map<List<ReadLocalizacaoDto>>(localizacoes);
            return new PagedResult<ReadLocalizacaoDto>(dtos, totalCount, pageNumber, pageSize);
        }

        public async Task DeleteAsync(Guid id)
        {
            var localizacao = await _localizacaoRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Registro de localização não encontrado.");

            _localizacaoRepository.Delete(localizacao);
            await _context.SaveChangesAsync();
        }
    }
}