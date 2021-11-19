using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simplic.Data
{
    /// <inheritdoc />
    public class FluentTransactionBuilder : IFluentTransactionBuilder
    {
        private readonly IList<object> services;
        private ITransaction transaction;

        /// <summary>
        /// Initialize transaction builder
        /// </summary>
        /// <param name="transactionService">Transaction service instance</param>
        public FluentTransactionBuilder(ITransactionService transactionService)
        {
            services = new List<object>();
            TransactionService = transactionService;
        }

        /// <inheritdoc />
        public void AddService<T, I>(ITransactionRepository<T, I> service) where T : new()
        {
            services.Add(service);
        }

        /// <inheritdoc />
        public ITransactionRepository<T, I> GetService<T, I>() where T : new()
        {
            return (ITransactionRepository<T, I>)services.FirstOrDefault();
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
