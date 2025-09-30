using Mottu.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o repositório da entidade Filial.
    /// Abstrai as operações de acesso a dados (CRUD) para as filiais.
    /// </summary>
    public interface IFilialRepository
    {
        /// <summary>
        /// Adiciona uma nova filial de forma assíncrona.
        /// </summary>
        /// <param name="filial">A entidade Filial a ser adicionada.</param>
        Task AddAsync(Filial filial);

        /// <summary>
        /// Busca uma filial pelo seu ID de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID da filial.</param>
        /// <returns>A filial encontrada ou nulo se não existir.</returns>
        Task<Filial?> GetByIdAsync(int id);

        /// <summary>
        /// Busca todas as filiais de forma paginada e assíncrona.
        /// </summary>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Uma coleção de filiais.</returns>
        Task<IEnumerable<Filial>> GetAllPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retorna a contagem total de filiais de forma assíncrona.
        /// </summary>
        /// <returns>O número total de filiais.</returns>
        Task<int> GetCountAsync();

        /// <summary>
        /// Marca uma filial para atualização.
        /// A persistência ocorrerá quando SaveChanges for chamado.
        /// </summary>
        /// <param name="filial">A entidade Filial a ser atualizada.</param>
        void Update(Filial filial);

        /// <summary>
        /// Marca uma filial para remoção.
        /// A persistência ocorrerá quando SaveChanges for chamado.
        /// </summary>
        /// <param name="filial">A entidade Filial a ser removida.</param>
        void Delete(Filial filial);
    }
}