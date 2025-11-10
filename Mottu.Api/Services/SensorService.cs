using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.DTOs.SensorDtos;
using Mottu.Api.DTOs.Shared;
using Mottu.Api.Models.Entities;
using Mottu.Api.Repositories.Interfaces;
using Mottu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly IPatioRepository _patioRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public SensorService(ISensorRepository sensorRepository, IPatioRepository patioRepository, IMapper mapper, AppDbContext context)
        {
            _sensorRepository = sensorRepository;
            _patioRepository = patioRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ReadSensorDto> CreateAsync(CreateSensorDto createDto)
        {
            // Regra de negócio: Garante que o pátio especificado existe.
            var patioExists = await _patioRepository.GetByIdAsync(createDto.PatioId);
            if (patioExists == null)
            {
                throw new KeyNotFoundException("O pátio especificado para o sensor não existe.");
            }

            var sensor = _mapper.Map<Sensor>(createDto);
            await _sensorRepository.AddAsync(sensor);
            await _context.SaveChangesAsync();

            var createdSensor = await _sensorRepository.GetByIdAsync(sensor.Id);
            return _mapper.Map<ReadSensorDto>(createdSensor);
        }

        public async Task<ReadSensorDto?> GetByIdAsync(int id)
        {
            var sensor = await _sensorRepository.GetByIdAsync(id);
            return _mapper.Map<ReadSensorDto>(sensor);
        }

        public async Task<PagedResult<ReadSensorDto>> GetAllByPatioPaginatedAsync(int patioId, int pageNumber, int pageSize)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: Iniciando consulta - patioId: {patioId}, pageNumber: {pageNumber}, pageSize: {pageSize}");
                Console.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: Iniciando consulta - patioId: {patioId}, pageNumber: {pageNumber}, pageSize: {pageSize}");
                
                var sensores = await _sensorRepository.GetAllByPatioPaginatedAsync(patioId, pageNumber, pageSize);
                var sensoresList = sensores.ToList();
                
                System.Diagnostics.Debug.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: Encontrados {sensoresList.Count} sensores do banco");
                Console.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: Encontrados {sensoresList.Count} sensores do banco");
                
                if (sensoresList.Count > 0)
                {
                    var primeiro = sensoresList.First();
                    System.Diagnostics.Debug.WriteLine($"Primeiro sensor - Id: {primeiro.Id}, Descricao: {primeiro.Descricao}, PatioId: {primeiro.PatioId}");
                    Console.WriteLine($"Primeiro sensor - Id: {primeiro.Id}, Descricao: {primeiro.Descricao}, PatioId: {primeiro.PatioId}");
                }
                
                var totalCount = await _sensorRepository.GetCountByPatioAsync(patioId);
                System.Diagnostics.Debug.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: TotalCount: {totalCount}");
                Console.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: TotalCount: {totalCount}");
                
                var dtos = _mapper.Map<List<ReadSensorDto>>(sensoresList);
                
                System.Diagnostics.Debug.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: Após mapeamento, {dtos.Count} sensores no response");
                Console.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: Após mapeamento, {dtos.Count} sensores no response");
                
                return new PagedResult<ReadSensorDto>(dtos, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: ERRO - {ex.Message}");
                Console.WriteLine($"SensorService.GetAllByPatioPaginatedAsync: ERRO - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task UpdateAsync(int id, UpdateSensorDto updateDto)
        {
            var sensor = await _sensorRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Sensor não encontrado.");

            _mapper.Map(updateDto, sensor);
            _sensorRepository.Update(sensor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sensor = await _sensorRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Sensor não encontrado.");

            _sensorRepository.Delete(sensor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // A restrição do banco de dados OnDelete(Restrict) foi acionada.
                throw new InvalidOperationException("Não é possível excluir um sensor com histórico de localização.");
            }
        }
    }
}