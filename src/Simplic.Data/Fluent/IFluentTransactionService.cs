using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    public interface IFluentTransactionService
    {
        IFluentTransactionBuilder BeginTransaction();
    }
}
