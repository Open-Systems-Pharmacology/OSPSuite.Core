using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OSPSuite.Core.Domain.Services
{
   public class ConcurrencyManagerResult<TResult> where TResult : class
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
         Result = null;
      }
   }

   public interface IConcurrencyManager
   {
      /// <summary>
      /// </summary>
      /// <typeparam name="TData">Data type to consume by the worker function</typeparam>
      /// <typeparam name="TResult">Data produced by the worker function</typeparam>
      /// <param name="numberOfCoresToUse">Number of cores to use. Use 0 or negative to take all cores</param>
      /// <param name="cancellationToken">Cancellation token to cancel the threads</param>
      /// <param name="data">List of data to consume by the workers</param>
      /// <param name="action">A function to run on each worker on each piece of data</param>
      /// <returns>Dictionary binding a result for each input data after running the action on it</returns>
      Task<IReadOnlyDictionary<TData, ConcurrencyManagerResult<TResult>>> RunAsync<TData, TResult>
      (
         int numberOfCoresToUse,
         CancellationToken cancellationToken,
         IReadOnlyList<TData> data,
         Func<TData, string> id,
         Func<int, CancellationToken, TData, Task<TResult>> action
      ) where TResult : class;
   }

   public class ConcurrencyManager : IConcurrencyManager
   {
      private readonly int _maximumNumberOfCoresToUse = Math.Max(1, Environment.ProcessorCount - 1);

      public async Task<IReadOnlyDictionary<TData, ConcurrencyManagerResult<TResult>>> RunAsync<TData, TResult>
      (
         int numberOfCoresToUse,
         CancellationToken cancellationToken,
         IReadOnlyList<TData> data,
         Func<TData, string> id,
         Func<int, CancellationToken, TData, Task<TResult>> action
      ) where TResult : class
      {
         if (numberOfCoresToUse <= 0)
            numberOfCoresToUse = _maximumNumberOfCoresToUse;
         var concurrentData = new ConcurrentQueue<TData>(data);
         numberOfCoresToUse = Math.Min(numberOfCoresToUse, concurrentData.Count);

         var results = new ConcurrentDictionary<TData, ConcurrencyManagerResult<TResult>>();

         //Starts one task per core <= THIS IS NOT QUITE RIGHT I BELIEVE
         var tasks = Enumerable.Range(0, numberOfCoresToUse).Select(async coreIndex =>
         {
            //While there is data left
            while (concurrentData.TryDequeue(out var datum))
            {
               cancellationToken.ThrowIfCancellationRequested();

               //Invoke the action on it and store the result
               var result = await returnWithExceptionHandling(
                  coreIndex,
                  cancellationToken,
                  action,
                  datum,
                  id
               );
               results.TryAdd(datum, result);
            }
         });

         await Task.WhenAll(tasks);
         //all tasks are completed. Can return results

         return results;
      }

      private async Task<ConcurrencyManagerResult<TResult>> returnWithExceptionHandling<TData, TResult>
      (
         int coreId,
         CancellationToken cancellationToken,
         Func<int, CancellationToken, TData, Task<TResult>> task,
         TData input, Func<TData, string> id
      ) where TResult : class
      {
         try
         {
            return new ConcurrencyManagerResult<TResult>
            (
               id(input),
               result: await task.Invoke(coreId, cancellationToken, input)
            );
         }
         catch (Exception e)
         {
            return new ConcurrencyManagerResult<TResult>
            (
               id(input),
               errorMessage: e.Message
            );
         }
      }
   }
}