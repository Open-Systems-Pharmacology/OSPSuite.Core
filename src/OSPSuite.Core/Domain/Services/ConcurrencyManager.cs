using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public class ConcurrencyManagerResult<TResult>
   {
      public string Id { get; }
      public bool Succeeded { get; }
      public string ErrorMessage { get; }
      public TResult Result { get; }

      public ConcurrencyManagerResult(string id, TResult result)
      {
         Id = id;
         Succeeded = true;
         ErrorMessage = "";
         Result = result;
      }

      public ConcurrencyManagerResult(string id, string errorMessage)
      {
         Id = id;
         Succeeded = false;
         ErrorMessage = errorMessage;
         Result = default;
      }
   }

   public interface IConcurrencyManager
   {
      /// <summary>
      /// </summary>
      /// <typeparam name="TData">Data type to consume by the worker function</typeparam>
      /// <typeparam name="TResult">Data produced by the worker function</typeparam>
      /// <param name="data">List of data to consume by the workers</param>
      /// <param name="func">
      ///    A function running on each worker on each piece of data. Needs to throw exceptions if the <paramref name="func"/> is not successful.
      ///    Exceptions are captured per item and returned as failed results — the batch continues.
      /// </param>
      /// <param name="cancellationToken">Cancellation token to cancel the threads</param>
      /// <param name="numberOfCoresToUse">Maximum number of items processed concurrently. Must be at least 1, or -1 to use all available cores</param>
      /// <returns>Dictionary binding a result for each input data after running the action on it</returns>
      Task<IDictionary<TData, ConcurrencyManagerResult<TResult>>> RunAsync<TData, TResult>(
         IReadOnlyList<TData> data,
         Func<TData, CancellationToken, TResult> func,
         CancellationToken cancellationToken,
         int numberOfCoresToUse
      ) where TData : IWithId;

      /// <summary>
      /// </summary>
      /// <typeparam name="TData">Data type to consume by the worker function</typeparam>
      /// <param name="data">List of data to consume by the workers</param>
      /// <param name="action">
      ///    An action running on each worker on each piece of data. Unlike the overload returning results, exceptions
      ///    thrown by the action are not isolated per item: they abort the batch and surface as an <see cref="AggregateException"/>.
      ///    Catch inside the action to keep the batch running on a per-item failure.
      /// </param>
      /// <param name="cancellationToken">Cancellation token to cancel the threads</param>
      /// <param name="numberOfCoresToUse">Maximum number of items processed concurrently. Must be at least 1, or -1 to use all available cores</param>
      Task RunAsync<TData>(
         IReadOnlyList<TData> data,
         Action<TData, CancellationToken> action,
         CancellationToken cancellationToken,
         int numberOfCoresToUse
      );
   }

   public class ConcurrencyManager : IConcurrencyManager
   {
      private readonly IObjectTypeResolver _objectTypeResolver;

      public ConcurrencyManager(IObjectTypeResolver objectTypeResolver)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      public async Task<IDictionary<TData, ConcurrencyManagerResult<TResult>>> RunAsync<TData, TResult>
      (
         IReadOnlyList<TData> data,
         Func<TData, CancellationToken, TResult> func,
         CancellationToken cancellationToken,
         int numberOfCoresToUse
      ) where TData : IWithId
      {
         var results = new ConcurrentDictionary<TData, ConcurrencyManagerResult<TResult>>();

         verifyUniqueIdsAreUsed(data);

         await Task.Run(() => Parallel.ForEach(data, createParallelOptions(cancellationToken, numberOfCoresToUse),
            datum =>
            {
               cancellationToken.ThrowIfCancellationRequested();
               try
               {
                  results.TryAdd(
                     datum,
                     new ConcurrencyManagerResult<TResult>(datum.Id, func(datum, cancellationToken))
                  );
               }
               catch (Exception e)
               {
                  results.TryAdd(
                     datum,
                     new ConcurrencyManagerResult<TResult>(datum.Id, e.FullMessage())
                  );
               }
            }), cancellationToken);
         return results;
      }

      public Task RunAsync<TData>(IReadOnlyList<TData> data, Action<TData, CancellationToken> action, CancellationToken cancellationToken, int numberOfCoresToUse)
      {
         return Task.Run(() => Parallel.ForEach(data, createParallelOptions(cancellationToken, numberOfCoresToUse),
            datum =>
            {
               cancellationToken.ThrowIfCancellationRequested();
               action(datum, cancellationToken);
            }), cancellationToken);
      }

      private void verifyUniqueIdsAreUsed<TData>(IReadOnlyList<TData> data) where TData : IWithId
      {
         var duplicates = data.GroupBy(x => x.Id).Where(g => g.Count() > 1).Select(x => x.Key).ToList();
         if (!duplicates.Any())
            return;

         throw new NotUniqueIdException(duplicates[0], _objectTypeResolver.TypeFor<TData>());
      }

      private ParallelOptions createParallelOptions(CancellationToken token, int maximumNumberOfCoresToUse)
      {
         return new ParallelOptions()
         {
            CancellationToken = token,
            MaxDegreeOfParallelism = maximumNumberOfCoresToUse
         };
      }
   }
}