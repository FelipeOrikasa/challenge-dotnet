using Mottu.Api.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o repositório da entidade Patio.
    /// Abstrai as operações de acesso a dados para os pátios.
    /// </summary>
    public interface IPatioRepository
    {
        /// <summary>
        /// Adiciona um novo pátio de forma assíncrona.
        /// </summary>
        /// <param name="patio">A entidade Pátio a ser adicionada.</param>
        Task AddAsync(Patio patio);

        /// <summary>
        /// Busca um pátio pelo seu ID de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID do pátio.</param>
        /// <returns>O pátio encontrado ou nulo se não existir.</returns>
        Task<Patio?> GetByIdAsync(int id);

        /// <summary>
        /// Busca todos os pátios de forma paginada e assíncrona.
        /// </summary>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Uma coleção de pátios.</returns>
        Task<IEnumerable<Patio>> GetAllPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retorna a contagem total de pátios de forma assíncrona.
        /// </summary>
        /// <returns>O número total de pátios.</returns>
        Task<int> GetCountAsync();

        /// <summary>
        /// Busca todos os pátios de uma filial específica, de forma paginada.
        /// </summary>
        /// <param name="filialId">O ID da filial.</param>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Uma coleção de pátios da filial especificada.</returns>
        Task<IEnumerable<Patio>> GetAllByFilialPaginatedAsync(int filialId, int pageNumber, int pageSize);

        /// <summary>
        /// Retorna a contagem total de pátios em uma filial específica.
        /// </summary>
        /// <param name="filialId">O ID da filial.</param>
        /// <returns>O número total de pátios na filial.</returns>
        Task<int> GetCountByFilialAsync(int filialId);

        /// <summary>
        /// Marca um pátio para atualização.
        /// </summary>
        /// <param name="patio">A entidade Pátio a ser atualizada.</param>
        void Update(Patio patio);

        /// <summary>
        /// Marca um pátio para remoção.
        /// </summary>
        /// <param name="patio">A entidade Pátio a ser removida.</param>
        void Delete(Patio patio);
    }
}