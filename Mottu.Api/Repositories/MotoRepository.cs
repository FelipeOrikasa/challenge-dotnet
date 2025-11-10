using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.Models.Entities;
using Mottu.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        private readonly AppDbContext _context;

        public MotoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Moto moto)
        {
            await _context.Motos.AddAsync(moto);
        }

        public async Task<Moto?> GetByIdAsync(Guid id)
        {
            // Inclui os dados do Pátio relacionado
            return await _context.Motos
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Moto?> GetByPlacaAsync(string placa)
        {
            return await _context.Motos
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Placa == placa);
        }

        public async Task<IEnumerable<Moto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"MotoRepository.GetAllPaginatedAsync: Iniciando consulta - pageNumber: {pageNumber}, pageSize: {pageSize}");
                Console.WriteLine($"MotoRepository.GetAllPaginatedAsync: Iniciando consulta - pageNumber: {pageNumber}, pageSize: {pageSize}");
                
                // Primeiro, tenta sem Include para ver se os dados básicos funcionam
                var motosSemInclude = await _context.Motos
                    .AsNoTracking()
                    .OrderBy(m => m.Placa)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                
                System.Diagnostics.Debug.WriteLine($"MotoRepository.GetAllPaginatedAsync: Encontradas {motosSemInclude.Count} motos sem Include");
                Console.WriteLine($"MotoRepository.GetAllPaginatedAsync: Encontradas {motosSemInclude.Count} motos sem Include");
                
                if (motosSemInclude.Count > 0)
                {
                    var primeira = motosSemInclude.First();
                    System.Diagnostics.Debug.WriteLine($"Primeira moto - Id: {primeira.Id}, Placa: {primeira.Placa}, Modelo: {primeira.Modelo ?? "NULL"}, Ano: {primeira.Ano}, PatioId: {primeira.PatioId}");
                    Console.WriteLine($"Primeira moto - Id: {primeira.Id}, Placa: {primeira.Placa}, Modelo: {primeira.Modelo ?? "NULL"}, Ano: {primeira.Ano}, PatioId: {primeira.PatioId}");
                }
                
                // Agora tenta com Include
                var motos = await _context.Motos
                    .Include(m => m.Patio)
                    .AsNoTracking()
                    .OrderBy(m => m.Placa)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                
                System.Diagnostics.Debug.WriteLine($"MotoRepository.GetAllPaginatedAsync: Encontradas {motos.Count} motos com Include");
                Console.WriteLine($"MotoRepository.GetAllPaginatedAsync: Encontradas {motos.Count} motos com Include");
                
                return motos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MotoRepository.GetAllPaginatedAsync: ERRO - {ex.Message}");
                Console.WriteLine($"MotoRepository.GetAllPaginatedAsync: ERRO - {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Motos.CountAsync();
        }

        public async Task<IEnumerable<Moto>> GetAllByPatioPaginatedAsync(int patioId, int pageNumber, int pageSize)
        {
            return await _context.Motos
                .Where(m => m.PatioId == patioId)
                .Include(m => m.Patio)
                .AsNoTracking()
                .OrderBy(m => m.Placa)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountByPatioAsync(int patioId)
        {
            return await _context.Motos.CountAsync(m => m.PatioId == patioId);
        }

        public void Update(Moto moto)
        {
            _context.Entry(moto).State = EntityState.Modified;
        }

        public void Delete(Moto moto)
        {
            _context.Motos.Remove(moto);
        }
    }
}