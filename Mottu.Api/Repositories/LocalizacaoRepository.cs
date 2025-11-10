using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.Models.Entities;
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

        public async Task<Localizacao?> GetByIdAsync(Guid id)
        {
            return await _context.Localizacoes
                .Include(l => l.Sensor)
                .ThenInclude(s => s.Patio) // Inclui o Pátio a partir do Sensor
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Localizacao>> GetAllByMotoPaginatedAsync(Guid motoId, int pageNumber, int pageSize)
        {
            // Nota: Localizacao não tem relação direta com Moto no modelo atual
            // Buscar através do Sensor que está no mesmo Patio da Moto
            return await _context.Localizacoes
                .Include(l => l.Sensor)
                .ThenInclude(s => s.Patio)
                .Where(l => l.Sensor != null && l.Sensor.Patio != null && l.Sensor.Patio.Motos != null && l.Sensor.Patio.Motos.Any(m => m.Id == motoId))
                .AsNoTracking()
                .OrderByDescending(l => l.Timestamp) // Ordena pelos registros mais recentes primeiro
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountByMotoAsync(Guid motoId)
        {
            return await _context.Localizacoes
                .CountAsync(l => l.Sensor != null && l.Sensor.Patio != null && l.Sensor.Patio.Motos != null && l.Sensor.Patio.Motos.Any(m => m.Id == motoId));
        }

        public void Delete(Localizacao localizacao)
        {
            _context.Localizacoes.Remove(localizacao);
        }
    }
}