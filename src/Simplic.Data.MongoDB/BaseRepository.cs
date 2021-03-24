﻿using System.Threading.Tasks;
using MongoDB.Driver;
using Simplic.Data.NoSql;

namespace Simplic.Data.MongoDB
{
    public class BaseRepository<TId, TDocument, TFilter> : BaseReadOnlyRepository<TId, TDocument, TFilter>, IRepository<TId, TDocument, TFilter>
        where TDocument : IDocument<TId>
        where TFilter : IFilter<TId>, new()
    {
        protected BaseRepository(IMongoContext context) : base(context)
        {
        }

        public virtual async Task CreateAsync(TDocument document)
        {
            await Initialize();
            Context.AddCommand(() => Collection.InsertOneAsync(document));
        }

        public virtual async Task UpdateAsync(TDocument document)
        {
            await Initialize();
            Context.AddCommand(() => Collection.ReplaceOneAsync(GetFilterById(document.Id), document));
        }

        public virtual async Task DeleteAsync(TId id)
        {
            await Initialize();

            var document = await GetByIdAsync(id);
            if (document != null)
            {
                document.IsDeleted = true;
                await UpdateAsync(document);
            }
        }

        public virtual async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
        }

        protected FilterDefinition<TDocument> GetFilterById(TId id)
        {
            return BuildFilterQuery(new TFilter
            {
                Id = id
            });
        }
    }
}
