using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Simplic.Data.NoSql;
using MongoDB.Driver;

namespace Simplic.Data.MongoDB
{
    /// <summary>
    /// Base implementation of a mongo db read only repository.
    /// </summary>
    /// <typeparam name="TId">The type of the id.</typeparam>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    public abstract class MongoReadOnlyRepositoryBase<TId, TDocument, TFilter> : IReadOnlyRepository<TId, TDocument, TFilter>
        where TDocument : IDocument<TId>
        where TFilter : IFilter<TId>, new()
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        protected readonly IMongoContext Context;

        /// <summary>
        /// Getss the collection.
        /// </summary>
        protected IMongoCollection<TDocument> Collection;

        /// <summary>
        /// Initializes a new instance of <see cref="MongoReadOnlyRepositoryBase{TId, TDocument, TFilter}"/>.
        /// </summary>
        /// <param name="context"></param>
        protected MongoReadOnlyRepositoryBase(IMongoContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MongoReadOnlyRepositoryBase{TId, TDocument, TFilter}"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configurationKey"></param>
        protected MongoReadOnlyRepositoryBase(IMongoContext context, string configurationKey)
        {
            Context = context;
            context.SetConfiguration(configurationKey);
        }

        /// <summary>
        /// Initializes a new collection.
        /// </summary>
        /// <returns></returns>
        protected async Task Initialize()
        {
            if (Collection == null)
            {
                Collection = Context.GetCollection<TDocument>(GetCollectionName());
            }

            await Task.CompletedTask; // TODO change Initialize signature and all call sites to sync versions
        }

        /// <summary>
        /// Gets the collection name.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCollectionName() => typeof(TDocument).Name;

        ///<inheritdoc/>
        public virtual async Task<TDocument> GetAsync(TId id)
        {
            await Initialize();

            var data = await GetByFilterAsync(new TFilter { Id = id });

            return data.SingleOrDefault();
        }

        ///<inheritdoc/>
        public virtual async Task<IEnumerable<TDocument>> GetAllAsync()
        {
            await Initialize();

            return await GetByFilterAsync(new TFilter());
        }

        ///<inheritdoc/>
        public virtual async Task<IEnumerable<TDocument>> GetByFilterAsync(TFilter filter)
        {
            await Initialize();

            return (await Collection.FindAsync(BuildFilterQuery(filter)))
                .ToEnumerable();
        }

        /// <summary>
        /// Disposes the context.
        /// </summary>
        public void Dispose()
        {
            Context?.Dispose();
        }

        /// <summary>
        /// Gets an enumerable of filter definitions.
        /// </summary>
        /// <param name="filter">A filter.</param>
        /// <returns>A list of filter definitions.</returns>
        protected virtual IEnumerable<FilterDefinition<TDocument>> GetFilterQueries(TFilter filter)
        {
            return new List<FilterDefinition<TDocument>>();
        }

        /// <summary>
        /// Builds a filter query.
        /// </summary>
        /// <param name="filter">A filter.</param>
        /// <returns>A filter definition for the document.</returns>
        protected FilterDefinition<TDocument> BuildFilterQuery(TFilter filter)
        {
            var filterQueries = GetFilterQueries(filter).ToList();
            var builder = Builders<TDocument>.Filter;

            // compare reference types with null and value types with defaults
            // https://stackoverflow.com/a/864860/4315106
            var isIdHasValue = !EqualityComparer<TId>.Default.Equals(filter.Id, default);
            if (isIdHasValue)
            {
                filterQueries.Add(builder.Eq(d => d.Id, filter.Id));
            }

            return filterQueries.Any()
                ? builder.And(filterQueries)
                : builder.Empty;
        }

        /// <summary>
        /// Finds the documents matching the filter.
        /// </summary>
        /// <param name="predicate">The filter predicate</param>
        /// <param name="skip">Number of skipped entities</param>
        /// <param name="limit">Number of requested entities</param>
        /// <param name="sortField">Sort field</param>
        /// <param name="isAscending">Ascending or Descending sort</param>
        /// <returns><typeparamref name="TDocument"/> entities matching the search criteria</returns>
        public virtual async Task<IEnumerable<TDocument>> FindAsync(TFilter predicate, int? skip, int? limit, string sortField = "", bool isAscending = true)
        {
            await Initialize();

            SortDefinition<TDocument> sort = null;
            if (!string.IsNullOrWhiteSpace(sortField))
                sort = isAscending
                    ? Builders<TDocument>.Sort.Ascending(sortField)
                    : Builders<TDocument>.Sort.Descending(sortField);

            return Collection.Find(BuildFilterQuery(predicate)).Sort(sort).Skip(skip).Limit(limit).ToList();
        }
    }
}
