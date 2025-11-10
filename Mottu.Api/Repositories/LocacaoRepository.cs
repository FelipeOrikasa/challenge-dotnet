using Microsoft.EntityFrameworkCore;
using Mottu.Api.Data;
using Mottu.Api.Models.Entities;
using Mottu.Api.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories.Implementations
{
    /// <summary>
    /// Implementação do repositório de Locacao.
    /// </summary>
    public class LocacaoRepository : BaseRepository<Locacao>, ILocacaoRepository
    {
        public LocacaoRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Busca uma locação ativa (DataTerminoEfetiva é NULL) pelo ID da Moto.
        /// </summary>
        public async Task<Locacao?> GetActiveByMotoIdAsync(Guid motoId)
        {
            return await _context.Set<Locacao>()
                .FirstOrDefaultAsync(l => l.MotoId == motoId && l.DataTerminoEfetiva == null);
        }

        /// <summary>
        /// Busca uma locação ativa (DataTerminoEfetiva é NULL) pelo ID do Entregador.
        /// </summary>
        public async Task<Locacao?> GetActiveByEntregadorIdAsync(Guid entregadorId)
        {
            return await _context.Set<Locacao>()
                .FirstOrDefaultAsync(l => l.EntregadorId == entregadorId && l.DataTerminoEfetiva == null);
        }
    }
}

