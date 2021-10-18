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
      Task RunAsync<TData, TResult>(
         int numberOfCoresToUse,
         IReadOnlyList<TData> data,
         Func<TData, string> idFunc,
         Func<int, TData, CancellationToken, TResult> action,
         CancellationToken cancellationToken,
         ConcurrentDictionary<TData, ConcurrencyManagerResult<TResult>> results
      );
   }

   public class ConcurrencyManager : IConcurrencyManager
   {
      private readonly int _maximumNumberOfCoresToUse = Math.Max(1, Environment.ProcessorCount - 1);

      public async Task RunAsync<TData, TResult>
      (int numberOfCoresToUse,
         IReadOnlyList<TData> data,
         Func<TData, string> idFunc,
         Func<int, TData, CancellationToken, TResult> action,
         CancellationToken cancellationToken,
         ConcurrentDictionary<TData, ConcurrencyManagerResult<TResult>> results)
      {
         if (data.Count == 0) return;

         if (numberOfCoresToUse <= 0)
            numberOfCoresToUse = _maximumNumberOfCoresToUse;

         var concurrentData = new ConcurrentQueue<TData>(data);
         numberOfCoresToUse = Math.Min(numberOfCoresToUse, concurrentData.Count);

         //Splits the action based in the number of cores available. 
         //No thread will be created here. If the actions are all running on the same thread, the effect of the concurrency execution will be inexistent
         await Task.Run(
            () => 
            Parallel.ForEach(
               Enumerable.Range(0, numberOfCoresToUse), 
               createParallelOptions(cancellationToken, numberOfCoresToUse), 
               coreId => 
               {
                  //While there is data left
                  while (concurrentData.TryDequeue(out var datum))
                  {
                     ConcurrencyManagerResult<TResult> result = null;
                     //Invoke the action on it and store the result. We assume here that each action runs on its own thread.
                     try
                     {
                        cancellationToken.ThrowIfCancellationRequested();
                        result = new ConcurrencyManagerResult<TResult>
                        (
                           id: idFunc(datum),
                           result: action(coreId, datum, cancellationToken)
                        );
                     }
                     catch (Exception e)
                     {
                        result = new ConcurrencyManagerResult<TResult>
                        (
                           id: idFunc(datum),
                           errorMessage: e.ExceptionMessage(addContactSupportInfo: false)
                        );
                     }
                     finally
                     {
                        results.TryAdd(datum, result);
                     }
                  }
               })
         );
      }

      private ParallelOptions createParallelOptions(CancellationToken token, int numberOfCoresToUse)
      {
         return new ParallelOptions()
         {
            CancellationToken = token,
            MaxDegreeOfParallelism = numberOfCoresToUse
         };
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