using System;
using System.Threading.Tasks;

namespace Simplic.Data
{
    /// <summary>
    /// Contains all fluent operations as extension method
    /// </summary>
    public static class FluentTransactionExtension
    {
        /// <summary>
        /// Add a service to the actual fluent builder
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <typeparam name="TModel">Object type</typeparam>
        /// <typeparam name="TId">Object unique id type</typeparam>
        /// <param name="builder">Actual builder instance</param>
        /// <param name="service">Service to register/add</param>
        /// <returns>The builder instance that was passed to the method</returns>
        public static IFluentTransactionBuilder AddService<TService, TModel, TId>(this IFluentTransactionBuilder builder, TService service) where TService : ITransactionRepository<TModel, TId>
                                                                                                                       where TModel : new()
        {
            builder.AddService(service);

            return builder;
        }

        /// <summary>
        /// Calls the create method from a service
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <typeparam name="TModel">Object type</typeparam>
        /// <typeparam name="TId">Object unique id type</typeparam>
        /// <param name="builder">Actual builder instance</param>
        /// <param name="func">Delegate for getting the data to create</param>
        /// <returns>The builder instance that was passed to the method</returns>
        public static IFluentTransactionBuilder Create<TService, TModel, TId>(this IFluentTransactionBuilder builder, Func<ITransactionRepository<TModel, TId>, TModel> func) where TService : ITransactionRepository<TModel, TId>
                                                                                                                                                    where TModel : new()
        {
            var service = builder.GetService<TModel, TId>();
            var item = func(service);

            builder.Tasks.Add(async () => await service.CreateAsync(item, await builder.GetTransaction()));

            return builder;
        }

        /// <summary>
        /// Calls the update method from a service
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <typeparam name="TModel">Object type</typeparam>
        /// <typeparam name="TId">Object unique id type</typeparam>
        /// <param name="builder">Actual builder instance</param>
        /// <param name="func">Delegate for getting the data to update</param>
        /// <returns>The builder instance that was passed to the method</returns>
        public static IFluentTransactionBuilder Update<TService, TModel, TId>(this IFluentTransactionBuilder builder, Func<ITransactionRepository<TModel, TId>, TModel> func) where TService : ITransactionRepository<TModel, TId>
                                                                                                                                                    where TModel : new()
        {
            var service = builder.GetService<TModel, TId>();
            var item = func(service);

            builder.Tasks.Add(async () => await service.UpdateAsync(item, await builder.GetTransaction()));

            return builder;
        }

        /// <summary>
        /// Calls the delete method from a service
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <typeparam name="TModel">Object type</typeparam>
        /// <typeparam name="TId">Object unique id type</typeparam>
        /// <param name="builder">Actual builder instance</param>
        /// <param name="func">Delegate for getting the id of the data to delete</param>
        /// <returns>The builder instance that was passed to the method</returns>
        public static IFluentTransactionBuilder Delete<TService, TModel, TId>(this IFluentTransactionBuilder builder, Func<ITransactionRepository<TModel, TId>, TId> func) where TService : ITransactionRepository<TModel, TId>
                                                                                                                                                    where TModel : new()
        {
            var service = builder.GetService<TModel, TId>();
            var id = func(service);

            builder.Tasks.Add(async () => await service.DeleteAsync(id, await builder.GetTransaction()));

            return builder;
        }

        /// <summary>
        /// Commit all operations
        /// </summary>
        /// <param name="builder">Actual builder instance</param>
        public static async Task CommitAsync(this IFluentTransactionBuilder builder)
        {
            foreach (var task in builder.Tasks)
            {
                try
                {
                    await task();
                }
                catch (Exception)
                {
                    await builder.TransactionService.AbortAsync(await builder.GetTransaction());

                    throw;
                }
            }

            await builder.TransactionService.CommitAsync(await builder.GetTransaction());
            builder.Tasks.Clear();
        }

        /// <summary>
        /// Abort the actual transaction and undo changes
        /// </summary>
        /// <param name="builder">Actual builder instance</param>
        public static async Task AbortAsync(this IFluentTransactionBuilder builder)
        {
            await builder.TransactionService.AbortAsync(await builder.GetTransaction());
            builder.Tasks.Clear();
        }
    }
}
