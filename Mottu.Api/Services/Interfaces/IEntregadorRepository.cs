using Mottu.Api.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories.Interfaces
{
    /// <summary>
    /// Define operações de persistência de dados para a entidade Entregador.
    /// </summary>
    public interface IEntregadorRepository
    {
        Task<Entregador?> GetByIdAsync(Guid id);
        Task<IEnumerable<Entregador>> GetAllAsync();
        Task AddAsync(Entregador entregador);
        Task UpdateAsync(Entregador entregador);
        Task DeleteAsync(Guid id);
        
        // Métodos de busca específicos
        Task<Entregador?> GetByCNPJAsync(string cnpj);
        Task<Entregador?> GetByCNHAsync(string cnh);
    }
}