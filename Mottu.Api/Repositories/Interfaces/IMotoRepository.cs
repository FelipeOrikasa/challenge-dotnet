using Mottu.Api.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o repositório da entidade Moto.
    /// Abstrai as operações de acesso a dados para as motos.
    /// </summary>
    public interface IMotoRepository
    {
        /// <summary>
        /// Adiciona uma nova moto de forma assíncrona.
        /// </summary>
        /// <param name="moto">A entidade Moto a ser adicionada.</param>
        Task AddAsync(Moto moto);

        /// <summary>
        /// Busca uma moto pelo seu ID de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID da moto.</param>
        /// <returns>A moto encontrada ou nulo se não existir.</returns>
        Task<Moto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Busca uma moto pela sua placa de forma assíncrona.
        /// </summary>
        /// <param name="placa">A placa da moto.</param>
        /// <returns>A moto encontrada ou nulo se não existir.</returns>
        Task<Moto?> GetByPlacaAsync(string placa);

        /// <summary>
        /// Busca todas as motos de forma paginada e assíncrona.
        /// </summary>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Uma coleção de motos.</returns>
        Task<IEnumerable<Moto>> GetAllPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retorna a contagem total de motos de forma assíncrona.
        /// </summary>
        /// <returns>O número total de motos.</returns>
        Task<int> GetCountAsync();

        /// <summary>
        /// Busca todas as motos de um pátio específico, de forma paginada.
        /// </summary>
        /// <param name="patioId">O ID do pátio.</param>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Uma coleção de motos do pátio especificado.</returns>
        Task<IEnumerable<Moto>> GetAllByPatioPaginatedAsync(int patioId, int pageNumber, int pageSize);

        /// <summary>
        /// Retorna a contagem total de motos em um pátio específico.
        /// </summary>
        /// <param name="patioId">O ID do pátio.</param>
        /// <returns>O número total de motos no pátio.</returns>
        Task<int> GetCountByPatioAsync(int patioId);

        /// <summary>
        /// Marca uma moto para atualização.
        /// </summary>
        /// <param name="moto">A entidade Moto a ser atualizada.</param>
        void Update(Moto moto);

        /// <summary>
        /// Marca uma moto para remoção.
        /// </summary>
        /// <param name="moto">A entidade Moto a ser removida.</param>
        void Delete(Moto moto);
    }
}