using System;
using System.Reflection;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Qualification;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Extensions
{
   public static class ExceptionExtensions
   {
      public static string ExceptionMessage(this Exception ex, bool addContactSupportInfo = true)
      {
         if (IsWrapperException(ex))
            return ExceptionMessage(ex.InnerException, addContactSupportInfo);

         if (!addContactSupportInfo)
            return ex.FullMessage();

         return $"{ex.FullMessage()}{Environment.NewLine}{Environment.NewLine}{Captions.ContactSupport(Constants.FORUM_SITE)}";
      }

      public static bool IsWrapperException(this Exception ex) =>
         ex.IsAnImplementationOf<TargetInvocationException>() || ex.IsAnImplementationOf<AggregateException>();

      public static string ExceptionMessageWithStackTrace(this Exception ex, bool addContactSupportInfo = true) =>
         $"{ExceptionMessage(ex, addContactSupportInfo)}{Environment.NewLine}{Environment.NewLine}Stack trace:{Environment.NewLine}{ex.FullStackTrace()}";

      public static bool IsInfoException(this Exception ex)
      {
         if (ex == null)
            return false;

         if (ex.IsWrapperException())
            return IsInfoException(ex.InnerException);

         if (ex.IsAnImplementationOf<NotFoundException>() || ex.IsAnImplementationOf<QualificationRunException>())
            return false;

         return ex.IsAnImplementationOf<OSPSuiteException>();
      }

      public static bool IsOSPSuiteException(this Exception ex)
      {
         if (ex == null)
            return false;

         if (ex.IsWrapperException())
            return IsOSPSuiteException(ex.InnerException);

         return ex.IsAnImplementationOf<OSPSuiteException>();
      }

      public static Task DoWithinExceptionHandler(this object callerObject, Func<Task> actionToExecute)
         => DoWithinExceptionHandler(actionToExecute);

      public static async Task DoWithinExceptionHandler(this Func<Task> actionToExecute)
      {
         try
         {
            await actionToExecute();
         }
         catch (Exception ex)
         {
            IoC.Resolve<IExceptionManager>().LogException(ex);
         }
      }

      public static Task<TResult> DoWithinExceptionHandler<TResult>(this object callerObject, Func<Task<TResult>> actionToExecute)
         => DoWithinExceptionHandler(actionToExecute);

      public static async Task<TResult> DoWithinExceptionHandler<TResult>(this Func<Task<TResult>> actionToExecute)
      {
         var result = default(TResult);
         try
         {
            result = await actionToExecute();
         }
         catch (Exception ex)
         {
            IoC.Resolve<IExceptionManager>().LogException(ex);
         }

         return result;
      }
   }
}