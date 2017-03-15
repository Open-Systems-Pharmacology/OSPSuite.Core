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

      public ExceptionManager(IDialogCreator dialogCreator, IExceptionView exceptionView, IApplicationConfiguration configuration)
      {
         _dialogCreator = dialogCreator;
         _exceptionView = exceptionView;
         var emailSubject = $"Error in {configuration.ProductNameWithTrademark} {configuration.FullVersion}";
         _exceptionView.Initialize($"{configuration.ProductName}-Error", configuration.Icon, emailSubject, Constants.SUPPORT_EMAIL);
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
            _exceptionView.Display(ex);
            this.LogError(ex);
         }
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

         return string.Format("{0}\n{1}", ex.FullMessage(), Captions.ContactSupport(Constants.SUPPORT_EMAIL));
      }

      private static bool isWrapperException(Exception ex)
      {
         return ex.IsAnImplementationOf<TargetInvocationException>() || ex.IsAnImplementationOf<AggregateException>();
      }

      public static string ExceptionMessageWithStackTraceFrom(Exception ex)
      {
         return string.Format("{0}\n\nStack trace:\n{1}", ExceptionMessageFrom(ex), ex.FullStackTrace());
      }
   }
}