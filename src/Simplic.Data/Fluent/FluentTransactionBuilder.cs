using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simplic.Data
{
    /// <inheritdoc />
    public class FluentTransactionBuilder : IFluentTransactionBuilder
    {
        private readonly IList<object> services = new List<object>();
        private ITransaction transaction;

        /// <summary>
        /// Initialize transaction builder
        /// </summary>
        /// <param name="transactionService">Transaction service instance</param>
        public FluentTransactionBuilder(ITransactionService transactionService)
        {
            TransactionService = transactionService;
        }

        /// <inheritdoc />
        public void AddService<TModel, TId>(ITransactionRepository<TModel, TId> service) where TModel : new()
        {
            services.Add(service);
        }

        /// <inheritdoc />
        public ITransactionRepository<TModel, TId> GetService<TModel, TId>() where TModel : new()
        {
            return services.OfType<ITransactionRepository<TModel, TId>>().FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<ITransaction> GetTransaction()
        {
            if (transaction == null)
                transaction = await TransactionService.CreateAsync();

            return transaction;
        }

        /// <inheritdoc />
        public ITransactionService TransactionService { get; }

        /// <inheritdoc />
        public IList<Func<Task>> Tasks { get; } = new List<Func<Task>>();
    }
}
