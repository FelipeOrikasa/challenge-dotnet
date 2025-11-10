using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories.Interfaces
{
    /// <summary>
    /// Interface genérica que define as operações básicas de CRUD para todas as entidades.
    /// </summary>
    /// <typeparam name="TEntity">A entidade que o repositório manipula.</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
    }
}