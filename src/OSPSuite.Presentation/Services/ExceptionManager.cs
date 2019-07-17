using System;
using OSPSuite.Core;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Services
{
   public class ExceptionManager : ExceptionManagerBase
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IExceptionView _exceptionView;
      private readonly ILogger _logger;
      private readonly string _productInfo;

      public ExceptionManager(IDialogCreator dialogCreator, IExceptionView exceptionView, IApplicationConfiguration configuration, ILogger logger)
      {
         _dialogCreator = dialogCreator;
         _exceptionView = exceptionView;
         _logger = logger;
         _productInfo = $"{configuration.ProductNameWithTrademark} {configuration.FullVersion}";
         _exceptionView.Initialize($"{_productInfo} - Error", configuration.Icon, configuration.IssueTrackerUrl, configuration.ProductName);
      }

      public override void LogException(Exception ex)
      {
         if (ex.IsInfoException())
         {
            var message = ex.ExceptionMessage();
            _dialogCreator.MessageBoxInfo(message);
            _logger.AddInfo(message);
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
         _logger.AddError(message);
      }

      private string clipboardContentFrom(string message, string stackTrace)
      {
         return $"Application:\n{_productInfo}\n\n{message}\n\nStack trace:\n```\n{stackTrace}\n```";
      }
   }
}