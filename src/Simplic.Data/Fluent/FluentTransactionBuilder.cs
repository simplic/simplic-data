using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    public class FluentTransactionBuilder : IFluentTransactionBuilder
    {
        private readonly IList<object> services;
        private ITransaction transaction;

        public FluentTransactionBuilder(ITransactionService transactionService)
        {
            services = new List<object>();
            TransactionService = transactionService;
        }


        public void AddService<T, I>(ITransactionRepository<T, I> service) where T : new()
        {
            services.Add(service);
        }

        public ITransactionRepository<T, I> GetService<T, I>() where T : new()
        {
            return (ITransactionRepository<T, I>)services.FirstOrDefault();
        }

        public async Task<ITransaction> GetTransaction()
        {
            if (transaction == null)
                transaction = await TransactionService.CreateAsync();

            return transaction;
        }

        public ITransactionService TransactionService { get; }

        public IList<Func<Task>> Tasks { get; } = new List<Func<Task>>();
    }
}
