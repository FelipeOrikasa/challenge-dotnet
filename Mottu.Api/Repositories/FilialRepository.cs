using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories
{
    public class FilialRepository : IFilialRepository
    {
        private readonly AppDbContext _context;

        public FilialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Filial filial)
        {
            await _context.Filiais.AddAsync(filial);
        }

        public async Task<Filial?> GetByIdAsync(int id)
        {
            return await _context.Filiais.FindAsync(id);
        }

        public async Task<IEnumerable<Filial>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _context.Filiais
                .AsNoTracking()
                .OrderBy(f => f.NomeFilial)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Filiais.CountAsync();
        }

        public void Update(Filial filial)
        {
            // Marca a entidade como modificada.
            _context.Entry(filial).State = EntityState.Modified;
        }

        public void Delete(Filial filial)
        {
            // Marca a entidade para remoção.
            _context.Filiais.Remove(filial);
        }
    }
}