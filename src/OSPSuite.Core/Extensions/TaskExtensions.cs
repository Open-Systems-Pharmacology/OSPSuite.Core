using System;
using System.Threading.Tasks;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Extensions
{
   public static class TaskExtensions
   {
      public static void SecureContinueWith(this Task task, Action<Task> continuationAction)
      {
         task.ContinueWith(continuationAction, TaskContinuationOptions.NotOnFaulted);
         task.ContinueWith(t => IoC.Resolve<IExceptionManager>().LogException(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
      }
      
      public static void SecureContinueWith<TResult>(this Task<TResult> task, Action<Task<TResult>> continuationAction)
      {
         task.ContinueWith(continuationAction, TaskContinuationOptions.NotOnFaulted);
         task.ContinueWith(t => IoC.Resolve<IExceptionManager>().LogException(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
      }
      
      public static async Task SecureAwait<TObject>(this TObject caller, Func<TObject, Task> func)
      {
         try
         {
            await func(caller);
         }
         catch (Exception ex)
         {
            IoC.Resolve<IExceptionManager>().LogException(ex);
         }
      }

      public static async Task<TResult> SecureAwait<TObject, TResult>(this TObject caller, Func<TObject, Task<TResult>> func)
      {
         var result = default(TResult);
         try
         {
            result = await func(caller);
         }
         catch (Exception ex)
         {
            IoC.Resolve<IExceptionManager>().LogException(ex);
         }
         return result;
      }
   }
}