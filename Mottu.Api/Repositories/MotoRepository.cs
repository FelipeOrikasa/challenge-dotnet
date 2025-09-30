using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
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

        public async Task<Moto?> GetByIdAsync(int id)
        {
            // Inclui os dados do Pátio relacionado
            return await _context.Motos
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.MotoId == id);
        }

        public async Task<Moto?> GetByPlacaAsync(string placa)
        {
            return await _context.Motos
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Placa == placa);
        }

        public async Task<IEnumerable<Moto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _context.Motos
                .Include(m => m.Patio)
                .AsNoTracking()
                .OrderBy(m => m.Modelo)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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
                .OrderBy(m => m.Modelo)
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