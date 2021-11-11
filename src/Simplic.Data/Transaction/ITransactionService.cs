using System.Threading.Tasks;

namespace Simplic.Data
{
    public interface ITransactionService
    {
        Task<ITransaction> CreateAsync();

        Task CommitAsync(ITransaction transaction);

        Task AbortAsync(ITransaction transaction);
    }
}
