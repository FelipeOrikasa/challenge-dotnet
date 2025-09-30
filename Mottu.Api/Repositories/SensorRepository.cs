using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.Models;
using Mottu.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly AppDbContext _context;

        public SensorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Sensor sensor)
        {
            await _context.Sensores.AddAsync(sensor);
        }

        public async Task<Sensor?> GetByIdAsync(int id)
        {
            // Inclui os dados do Pátio relacionado para que o objeto venha completo.
            return await _context.Sensores
                .Include(s => s.Patio)
                .FirstOrDefaultAsync(s => s.SensorId == id);
        }

        public async Task<IEnumerable<Sensor>> GetAllByPatioPaginatedAsync(int patioId, int pageNumber, int pageSize)
        {
            return await _context.Sensores
                .Where(s => s.PatioId == patioId) // Filtra os sensores pelo pátio
                .Include(s => s.Patio)
                .AsNoTracking()
                .OrderBy(s => s.Descricao)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountByPatioAsync(int patioId)
        {
            return await _context.Sensores.CountAsync(s => s.PatioId == patioId);
        }

        public void Update(Sensor sensor)
        {
            _context.Entry(sensor).State = EntityState.Modified;
        }

        public void Delete(Sensor sensor)
        {
            _context.Sensores.Remove(sensor);
        }
    }
}