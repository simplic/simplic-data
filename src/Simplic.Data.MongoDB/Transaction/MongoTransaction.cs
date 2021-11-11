using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Simplic.Data.MongoDB
{
    internal class MongoTransaction : ITransaction, IDisposable
    {
        public void Dispose()
        {
            Session?.Dispose();
        }

        internal IClientSessionHandle Session { get; set; }
    }
}
