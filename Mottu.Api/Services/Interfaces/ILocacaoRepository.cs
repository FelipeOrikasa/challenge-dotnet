using Mottu.Api.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories.Interfaces
{
    /// <summary>
    /// Define operações de persistência de dados para a entidade Locacao.
    /// </summary>
    public interface ILocacaoRepository
    {
        Task<Locacao?> GetByIdAsync(Guid id);
        Task<IEnumerable<Locacao>> GetAllAsync();
        Task AddAsync(Locacao locacao);
        Task UpdateAsync(Locacao locacao);
        
        // Método de busca específico
        Task<Locacao?> GetActiveByEntregadorIdAsync(Guid entregadorId);
        Task<Locacao?> GetActiveByMotoIdAsync(Guid motoId);
    }
}