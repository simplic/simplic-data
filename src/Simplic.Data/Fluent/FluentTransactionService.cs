using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    public class FluentTransactionService : IFluentTransactionService
    {
        private readonly ITransactionService transactionService;

        public FluentTransactionService(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public IFluentTransactionBuilder BeginTransaction()
        {
            var builder = new FluentTransactionBuilder(transactionService);

            return builder;
        }
    }
}
