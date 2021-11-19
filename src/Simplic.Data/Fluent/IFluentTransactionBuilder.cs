using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    public interface IFluentTransactionBuilder
    {
        ITransactionRepository<T, I> GetService<T, I>() where T : new();

        void AddService<T, I>(ITransactionRepository<T, I> service) where T : new();

        Task<ITransaction> GetTransaction();

        ITransactionService TransactionService { get; }

        IList<Func<Task>> Tasks { get; }
    }
}
