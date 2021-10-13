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

      public static Task SecureAwait<TObject>(this TObject caller, Func<TObject, Task> func)
      {
         Func<Task> fun = () => func(caller);
         return fun.DoWithinExceptionHandler();
      }

      public static Task<TResult> SecureAwait<TObject, TResult>(this TObject caller, Func<TObject, Task<TResult>> func)
      {
         Func<Task<TResult>> fun = () => func(caller);
         return fun.DoWithinExceptionHandler();
      }
   }
}