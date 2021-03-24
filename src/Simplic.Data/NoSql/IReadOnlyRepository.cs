using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simplic.Data.NoSql
{
    /// <summary>
    /// Basic read only repository
    /// </summary>
    /// <typeparam name="TId">PK (ID) type</typeparam>
    /// <typeparam name="TDocument">Entity type</typeparam>
    /// <typeparam name="TFilter">Filter type</typeparam>
    public interface IReadOnlyRepository<TId, TDocument, TFilter> : IDisposable
        where TDocument : IDocument<TId>
        where TFilter : IFilter<TId>
    {
        /// <summary>
        /// Get an entity by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        Task<TDocument> GetAsync(TId id);

        /// <summary>
        /// Get all entities from data source
        /// </summary>
        /// <returns>Enumerable of entities</returns>
        Task<IEnumerable<TDocument>> GetAllAsync();


        /// <summary>
        /// Get entities by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Enumerable of entities</returns>
        Task<IEnumerable<TDocument>> GetByFilterAsync(TFilter filter);
    }
}
