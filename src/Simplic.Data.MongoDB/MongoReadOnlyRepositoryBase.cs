using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Simplic.Data.NoSql;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using System.Threading;

namespace Simplic.Data.MongoDB
{
    public abstract class MongoReadOnlyRepositoryBase<TId, TDocument, TFilter> : IReadOnlyRepository<TId, TDocument, TFilter>
        where TDocument : IDocument<TId>
        where TFilter : IFilter<TId>, new()
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<TDocument> Collection;

        protected MongoReadOnlyRepositoryBase(IMongoContext context)
        {
            Context = context;
        }

        protected MongoReadOnlyRepositoryBase(IMongoContext context, string configurationKey)
        {
            Context = context;
            context.SetConfiguration(configurationKey);
        }

        protected async Task Initialize()
        {
            if (Collection == null)
            {
                Collection = Context.GetCollection<TDocument>(GetCollectionName());
            }

            await Task.CompletedTask; // TODO change Initialize signature and all call sites to sync versions
        }

        protected virtual string GetCollectionName() => typeof(TDocument).Name;

        public virtual async Task<TDocument> GetAsync(TId id)
        {
            await Initialize();

            var data = await GetByFilterAsync(new TFilter { Id = id });

            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TDocument>> GetAllAsync()
        {
            await Initialize();

            return await GetByFilterAsync(new TFilter());
        }

        public virtual async Task<IEnumerable<TDocument>> GetByFilterAsync(TFilter filter)
        {
            await Initialize();

            return (await Collection.FindAsync(BuildFilterQuery(filter)))
                .ToEnumerable();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        protected virtual IEnumerable<FilterDefinition<TDocument>> GetFilterQueries(TFilter filter)
        {
            return new List<FilterDefinition<TDocument>>();
        }

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
        /// <param name="limit">Number of requested entities</param>
        /// <returns><see cref="TDocument"/> entities matching the search criteria</returns>
        public virtual async Task<IEnumerable<TDocument>> FindAsync(Func<TDocument, bool> predicate, int? limit)
        {
            await Initialize();

            var expression = Expression.Lambda<Func<TDocument, bool>>(Expression.Call(predicate.Method));
            var response = new List<TDocument>();
            using (var cursor = await Collection.FindAsync(expression, cancellationToken: CancellationToken.None))
            {
                while (await cursor.MoveNextAsync())
                {
                    if (limit.HasValue)
                    {
                        var requestedElements = cursor.Current.ToList().Take(limit.Value);
                        limit -= requestedElements.Count();
                        response.AddRange(requestedElements);
                        if (limit.Value == 0)
                            break;
                    }
                    else
                        response.AddRange(cursor.Current);
                }
            }
            return response;
        }
    }
}
