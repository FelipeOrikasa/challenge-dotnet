using Mottu.Api.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mottu.Api.Repositories.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para o repositório da entidade Localizacao.
    /// Abstrai as operações de acesso aos dados de histórico de localização.
    /// </summary>
    public interface ILocalizacaoRepository
    {
        /// <summary>
        /// Adiciona um novo registro de localização.
        /// </summary>
        /// <param name="localizacao">A entidade Localizacao a ser adicionada.</param>
        Task AddAsync(Localizacao localizacao);

        /// <summary>
        /// Busca todos os registros de localização de uma moto específica, de forma paginada.
        /// </summary>
        /// <param name="motoId">O ID da moto.</param>
        /// <param name="pageNumber">O número da página.</param>
        /// <param name="pageSize">O tamanho da página.</param>
        /// <returns>Uma coleção de registros de localização da moto especificada.</returns>
        Task<IEnumerable<Localizacao>> GetAllByMotoPaginatedAsync(Guid motoId, int pageNumber, int pageSize);

        /// <summary>
        /// Retorna a contagem total de registros de localização para uma moto específica.
        /// </summary>
        /// <param name="motoId">O ID da moto.</param>
        /// <returns>O número total de registros de localização para a moto.</returns>
        Task<int> GetCountByMotoAsync(Guid motoId);

        /// <summary>
        /// Remove um registro de localização (operação administrativa).
        /// </summary>
        /// <param name="localizacao">A entidade Localizacao a ser removida.</param>
        void Delete(Localizacao localizacao);

        /// <summary>
        /// Busca um registro de localização pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do registro de localização.</param>
        /// <returns>O registro de localização encontrado ou nulo.</returns>
        Task<Localizacao?> GetByIdAsync(Guid id);
    }
}