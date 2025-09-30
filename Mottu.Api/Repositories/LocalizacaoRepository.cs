using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories
{
    public class LocalizacaoRepository : ILocalizacaoRepository
    {
        private readonly AppDbContext _context;

        public LocalizacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Localizacao localizacao)
        {
            await _context.Localizacoes.AddAsync(localizacao);
        }

        public async Task<Localizacao?> GetByIdAsync(int id)
        {
            return await _context.Localizacoes
                .Include(l => l.Moto)
                .Include(l => l.Sensor)
                .ThenInclude(s => s.Patio) // Inclui o Pátio a partir do Sensor
                .FirstOrDefaultAsync(l => l.LocalizacaoId == id);
        }

        public async Task<IEnumerable<Localizacao>> GetAllByMotoPaginatedAsync(int motoId, int pageNumber, int pageSize)
        {
            return await _context.Localizacoes
                .Where(l => l.MotoId == motoId)
                .Include(l => l.Moto)
                .Include(l => l.Sensor)
                .ThenInclude(s => s.Patio)
                .AsNoTracking()
                .OrderByDescending(l => l.DataHora) // Ordena pelos registros mais recentes primeiro
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountByMotoAsync(int motoId)
        {
            return await _context.Localizacoes.CountAsync(l => l.MotoId == motoId);
        }

        public void Delete(Localizacao localizacao)
        {
            _context.Localizacoes.Remove(localizacao);
        }
    }
}