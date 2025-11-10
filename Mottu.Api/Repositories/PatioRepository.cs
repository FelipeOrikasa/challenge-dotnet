using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.Models.Entities;
using Mottu.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories
{
    public class PatioRepository : IPatioRepository
    {
        private readonly AppDbContext _context;

        public PatioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Patio patio)
        {
            await _context.Patios.AddAsync(patio);
        }



        public async Task<Patio?> GetByIdAsync(int id)
        {
            // Usa .Include() para carregar os dados da Filial relacionada na mesma consulta.
            return await _context.Patios
                .Include(p => p.Filial)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Patio>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _context.Patios
                .Include(p => p.Filial)
                .AsNoTracking()
                .OrderBy(p => p.NomePatio)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Patios.CountAsync();
        }

        public async Task<IEnumerable<Patio>> GetAllByFilialPaginatedAsync(int filialId, int pageNumber, int pageSize)
        {
            return await _context.Patios
                .Where(p => p.FilialId == filialId) // Filtra os pátios pelo ID da filial
                .Include(p => p.Filial)
                .AsNoTracking()
                .OrderBy(p => p.NomePatio)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountByFilialAsync(int filialId)
        {
            return await _context.Patios.CountAsync(p => p.FilialId == filialId);
        }

        public void Update(Patio patio)
        {
            _context.Entry(patio).State = EntityState.Modified;
        }

        public void Delete(Patio patio)
        {
            _context.Patios.Remove(patio);
        }
    }
}