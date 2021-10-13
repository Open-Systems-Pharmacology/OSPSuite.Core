using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Core.Extensions;

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
      /// <param name="numberOfCoresToUse">Number of cores to use. Use 0 or negative to take all cores</param>
      /// <param name="data">List of data to consume by the workers</param>
      /// <param name="idFunc"></param>
      /// <param name="action">
      ///    A function running on each worker on each piece of data. It is assume that the action runs on its
      ///    own thread
      /// </param>
      /// <param name="cancellationToken">Cancellation token to cancel the threads</param>
      /// <returns>Dictionary binding a result for each input data after running the action on it</returns>
      Task<IReadOnlyDictionary<TData, ConcurrencyManagerResult<TResult>>> RunAsync<TData, TResult>(
         int numberOfCoresToUse,
         IReadOnlyList<TData> data,
         Func<TData, string> idFunc,
         Func<int, TData, CancellationToken, Task<TResult>> action,
         CancellationToken cancellationToken
      );
   }

   public class ConcurrencyManager : IConcurrencyManager
   {
      private readonly int _maximumNumberOfCoresToUse = Math.Max(1, Environment.ProcessorCount - 1);

      public async Task<IReadOnlyDictionary<TData, ConcurrencyManagerResult<TResult>>> RunAsync<TData, TResult>
      (int numberOfCoresToUse,
         IReadOnlyList<TData> data,
         Func<TData, string> idFunc,
         Func<int, TData, CancellationToken, Task<TResult>> action,
         CancellationToken cancellationToken)
      {
         if (numberOfCoresToUse <= 0)
            numberOfCoresToUse = _maximumNumberOfCoresToUse;
         numberOfCoresToUse = Math.Min(_maximumNumberOfCoresToUse, numberOfCoresToUse);

         var concurrentData = new ConcurrentQueue<TData>(data);
         numberOfCoresToUse = Math.Min(numberOfCoresToUse, concurrentData.Count);

         var results = new ConcurrentDictionary<TData, ConcurrencyManagerResult<TResult>>();

         //Splits the action based in the number of cores available. 
         //No thread will be created here. If the actions are all running on the same thread, the effect of the concurrency execution will be inexistent

         var tasks = Enumerable.Range(0, numberOfCoresToUse).Select(async coreId =>
         {
            //While there is data left
            while (concurrentData.TryDequeue(out var datum))
            {
               cancellationToken.ThrowIfCancellationRequested();

               //Invoke the action on it and store the result. We assume here that each action runs on its own thread.
               var result = await returnWithExceptionHandling(
                  coreId,
                  action,
                  datum,
                  idFunc,
                  cancellationToken
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
         Func<int, TData, CancellationToken, Task<TResult>> task,
         TData data,
         Func<TData, string> idFunc,
         CancellationToken cancellationToken
      )
      {
         try
         {
            return new ConcurrencyManagerResult<TResult>
            (
               id: idFunc(data),
               result: await task(coreId, data, cancellationToken)
            );
         }
         catch (Exception e)
         {
            return new ConcurrencyManagerResult<TResult>
            (
               id: idFunc(data),
               errorMessage: e.ExceptionMessage(addContactSupportInfo: false)
            );
         }
      }
   }
}