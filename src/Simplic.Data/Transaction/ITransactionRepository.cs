using System.Threading.Tasks;

namespace Simplic.Data
{
    /// <summary>
    /// Repository for managing transactions.
    /// </summary>
    public interface ITransactionRepository<TModel, TId> where TModel : new()
    {
        /// <summary>
        /// Asynchronously adds creation of a new object object to the transaction.
        /// <para> Adds a created event to the event transaction.</para>
        /// </summary>
        /// <param name="obj">Object to create.</param>
        /// <param name="transaction">Transaction to add to.</param>
        /// <returns>Task.</returns>
        Task CreateAsync(TModel obj, ITransaction transaction);

        /// <summary>
        /// Asynchronously adds deletion of an object object to the transaction.
        /// </summary>
        /// <param name="id">Identifier of object to delete.</param>
        /// <param name="transaction">Transaction to add to.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(TId id, ITransaction transaction);

        /// <summary>
        /// Asynchronously adds update of an object object to the transaction.
        /// </summary>
        /// <param name="obj">Object to update.</param>
        /// <param name="transaction">Transaction to add to.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(TModel obj, ITransaction transaction);
    }
}
