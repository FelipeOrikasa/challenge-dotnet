using Mottu.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o repositório da entidade Sensor.
    /// </summary>
    public interface ISensorRepository
    {
        /// <summary>
        /// Adiciona um novo sensor.
        /// </summary>
        Task AddAsync(Sensor sensor);

        /// <summary>
        /// Busca um sensor pelo seu ID.
        /// </summary>
        Task<Sensor?> GetByIdAsync(int id);

        /// <summary>
        /// Busca todos os sensores de um pátio específico, de forma paginada.
        /// </summary>
        Task<IEnumerable<Sensor>> GetAllByPatioPaginatedAsync(int patioId, int pageNumber, int pageSize);

        /// <summary>
        /// Retorna a contagem total de sensores em um pátio específico.
        /// </summary>
        Task<int> GetCountByPatioAsync(int patioId);

        /// <summary>
        /// Atualiza um sensor existente.
        /// </summary>
        void Update(Sensor sensor);

        /// <summary>
        /// Remove um sensor.
        /// </summary>
        void Delete(Sensor sensor);
    }
}