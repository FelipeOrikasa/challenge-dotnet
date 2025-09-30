using System.Collections.Generic;

namespace Mottu.Api.DTOs.Shared
{
    /// <summary>
    /// Representa um resultado paginado genérico para ser retornado pela API.
    /// Encapsula a lista de itens da página atual e os metadados da paginação.
    /// </summary>
    /// <typeparam name="T">O tipo do DTO contido na lista de itens.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// A lista de itens para a página atual.
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// O número da página atual.
        /// </summary>
        /// <example>1</example>
        public int PageNumber { get; }

        /// <summary>
        /// A quantidade de itens por página.
        /// </summary>
        /// <example>10</example>
        public int PageSize { get; }

        /// <summary>
        /// A contagem total de itens em todas as páginas.
        /// </summary>
        /// <example>50</example>
        public int TotalCount { get; }

        /// <summary>
        /// O número total de páginas disponíveis.
        /// </summary>
        /// <example>5</example>
        public int TotalPages { get; }

        /// <summary>
        /// Construtor para o resultado paginado.
        /// </summary>
        /// <param name="items">A lista de itens da página atual.</param>
        /// <param name="totalCount">A contagem total de itens.</param>
        /// <param name="pageNumber">O número da página atual.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        public PagedResult(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}