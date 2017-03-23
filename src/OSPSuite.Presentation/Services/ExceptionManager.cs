using System;
using System.Reflection;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Maths.Statistics;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Services
{
   public class ExceptionManager : ExceptionManagerBase
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IExceptionView _exceptionView;
      private readonly string _productInfo;

      public ExceptionManager(IDialogCreator dialogCreator, IExceptionView exceptionView, IApplicationConfiguration configuration)
      {
         _dialogCreator = dialogCreator;
         _exceptionView = exceptionView;
         _productInfo = $"{configuration.ProductNameWithTrademark} {configuration.FullVersion}";
         _exceptionView.Initialize($"{_productInfo} - Error", configuration.Icon, _productInfo, configuration.IssueTrackerUrl, configuration.ProductName);
      }

      public override void LogException(Exception ex)
      {
         if (isInfoException(ex))
         {
            var message = ExceptionMessageFrom(ex);
            _dialogCreator.MessageBoxInfo(message);
            this.LogInfo(message);
         }
         else
         {
            showException(ex);
         }
      }

      private void showException(Exception ex)
      {
         var message = ex.FullMessage();
         var stackTrace = ex.FullStackTrace();
         _exceptionView.Display(message, stackTrace, clipboardContentFrom(message, stackTrace));
         this.LogError(ex);
      }

      private string clipboardContentFrom(string message, string stackTrace)
      {
         return $"Application:\n{_productInfo}\n\n{message}\n\nStack trace:\n```\n{stackTrace}\n```";
      }

      private static bool isInfoException(Exception ex)
      {
         if (ex == null)
            return false;

         if (isWrapperException(ex))
            return isInfoException(ex.InnerException);

         if (ex.IsAnImplementationOf<NotFoundException>())
            return false;

         return ex.IsAnImplementationOf<OSPSuiteException>() || ex.IsAnImplementationOf<DistributionException>();
      }

      public static string ExceptionMessageFrom(Exception ex)
      {
         if (isWrapperException(ex))
            return ExceptionMessageFrom(ex.InnerException);

         return $"{ex.FullMessage()}\n{Captions.ContactSupport(Constants.FORUM_SITE)}";
      }

      private static bool isWrapperException(Exception ex)
      {
         return ex.IsAnImplementationOf<TargetInvocationException>() || ex.IsAnImplementationOf<AggregateException>();
      }

      public static string ExceptionMessageWithStackTraceFrom(Exception ex)
      {
         return $"{ExceptionMessageFrom(ex)}\n\nStack trace:\n{ex.FullStackTrace()}";
      }
   }
}