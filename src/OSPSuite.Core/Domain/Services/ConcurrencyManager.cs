using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
      /// <param name="action">
      ///    A function running on each worker on each piece of data. It is assume that the action runs on its
      ///    own thread
      /// </param>
      /// <param name="cancellationToken">Cancellation token to cancel the threads</param>
      /// <param name="numberOfCoresToUse">Number of cores to use. Use null to take all cores</param>
      /// <returns>Dictionary binding a result for each input data after running the action on it</returns>
      Task<IDictionary<TData, ConcurrencyManagerResult<TResult>>> RunAsync<TData, TResult>(
         IReadOnlyList<TData> data,
         Func<TData, CancellationToken, TResult> action,
         CancellationToken cancellationToken,
         uint? numberOfCoresToUse = null
      ) where TData : IWithId;
   }

   public class ConcurrencyManager : IConcurrencyManager
   {
      private readonly int _maximumNumberOfCoresToUse;

      public ConcurrencyManager(ICoreUserSettings coreUserSettings)
      {
         _maximumNumberOfCoresToUse = Math.Max(1, coreUserSettings.MaximumNumberOfCoresToUse);
      }

      public async Task<IDictionary<TData, ConcurrencyManagerResult<TResult>>> RunAsync<TData, TResult>
      (
         IReadOnlyList<TData> data,
         Func<TData, CancellationToken, TResult> action,
         CancellationToken cancellationToken,
         uint? numberOfCoresToUse = null
      ) where TData : IWithId
      {
         var results = new ConcurrentDictionary<TData, ConcurrencyManagerResult<TResult>>();

         await Task.Run(() => Parallel.ForEach(data, createParallelOptions(cancellationToken, numberOfCoresToUse == null ? _maximumNumberOfCoresToUse : Convert.ToInt32(numberOfCoresToUse)),
               datum =>
               {
                  cancellationToken.ThrowIfCancellationRequested();
                  try
                  {
                     results.TryAdd(
                        datum, 
                        new ConcurrencyManagerResult<TResult>(datum.Id, action(datum, cancellationToken))
                     );
                  }
                  catch (Exception e)
                  {
                     results.TryAdd(
                        datum,
                        new ConcurrencyManagerResult<TResult>(datum.Id, e.Message)
                     );
                  }
                  
               }), cancellationToken);
         return results;
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