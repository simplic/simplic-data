using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data.MongoDB
{
    public class MongoDbTransactionService : ITransactionService
    {
        private readonly MongoContext context;

        protected MongoDbTransactionService(IMongoContext context)
        {
            this.context = (MongoContext)context;
        }

        protected MongoDbTransactionService(IMongoContext context, string configurationKey)
        {
            this.context = (MongoContext)context;
            context.SetConfiguration(configurationKey);
        }

        public async Task AbortAsync(ITransaction transaction)
        {
            if (transaction is MongoTransaction mongoTransaction)
                await mongoTransaction.Session.AbortTransactionAsync();
        }

        public async Task CommitAsync(ITransaction transaction)
        {
            if (transaction is MongoTransaction mongoTransaction)
                await mongoTransaction.Session.CommitTransactionAsync();
        }

        public async Task<ITransaction> CreateAsync()
        {
            return new MongoTransaction
            {
                Session = await context.MongoClient.StartSessionAsync()
            };
        }
    }
}
