using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    public static class FluentTransactionExtension
    {
        public static IFluentTransactionBuilder AddService<K, T, I>(this IFluentTransactionBuilder builder, K service) where K : ITransactionRepository<T, I>
                                                                                                                       where T : new()
        {
            builder.AddService(service);

            return builder;
        }

        public static IFluentTransactionBuilder Create<K, T, I>(this IFluentTransactionBuilder builder, Func<ITransactionRepository<T, I>, T> func) where K : ITransactionRepository<T, I>
                                                                                                                                                    where T : new()
        {
            var service = builder.GetService<T, I>();
            var item = func(service);

            builder.Tasks.Add(async () => await service.CreateAsync(item, await builder.GetTransaction()));

            return builder;
        }

        public static IFluentTransactionBuilder Update<K, T, I>(this IFluentTransactionBuilder builder, Func<ITransactionRepository<T, I>, T> func) where K : ITransactionRepository<T, I>
                                                                                                                                                    where T : new()
        {
            var service = builder.GetService<T, I>();
            var item = func(service);

            builder.Tasks.Add(async () => await service.UpdateAsync(item, await builder.GetTransaction()));

            return builder;
        }

        public static IFluentTransactionBuilder Delete<K, T, I>(this IFluentTransactionBuilder builder, Func<ITransactionRepository<T, I>, I> func) where K : ITransactionRepository<T, I>
                                                                                                                                                    where T : new()
        {
            var service = builder.GetService<T, I>();
            var id = func(service);

            builder.Tasks.Add(async () => await service.DeleteAsync(id, await builder.GetTransaction()));

            return builder;
        }

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
        }

        public static async Task AbortAsync(this IFluentTransactionBuilder builder)
        {
            await builder.TransactionService.AbortAsync(await builder.GetTransaction());
        }
    }
}
