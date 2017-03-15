using System;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationRunner
   {
      Task Run(ParameterIdentification parameterIdentification);
      void Stop();
      bool IsRunning { get; }
   }

   public class ParameterIdentificationRunner : IParameterIdentificationRunner
   {
      private readonly IParameterIdentificationEngineFactory _parameterIdentificationEngineFactory;
      private readonly IDialogCreator _dialogCreator;
      private readonly IEntityValidationTask _entityValidationTask;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private IParameterIdentificationEngine _parameterIdentificationEngine;

      public bool IsRunning => _parameterIdentificationEngine != null;

      public ParameterIdentificationRunner(IParameterIdentificationEngineFactory parameterIdentificationEngineFactory,  IDialogCreator dialogCreator, 
         IEntityValidationTask entityValidationTask, IOSPSuiteExecutionContext executionContext)
      {
         _parameterIdentificationEngineFactory = parameterIdentificationEngineFactory;
         _dialogCreator = dialogCreator;
         _entityValidationTask = entityValidationTask;
         _executionContext = executionContext;
      }

      public async Task Run(ParameterIdentification parameterIdentification)
      {
         if (!_entityValidationTask.Validate(parameterIdentification))
            return;

         try
         {
            if (IsRunning)
               throw new OSPSuiteException(Error.CannotStartTwoConcurrentParameterIdentifications);

            using (_parameterIdentificationEngine = _parameterIdentificationEngineFactory.Create())
            {
               var begin = SystemTime.UtcNow();
               await _parameterIdentificationEngine.StartAsync(parameterIdentification);
               var end = SystemTime.UtcNow();
               var timeSpent = end - begin;
               _dialogCreator.MessageBoxInfo(Captions.ParameterIdentification.ParameterIdentificationFinished(timeSpent.ToDisplay()));
            }
         }
         catch (OperationCanceledException)
         {
            _dialogCreator.MessageBoxInfo(Captions.ParameterIdentification.ParameterIdentificationCanceled);
         }
         finally
         {
            _executionContext.ProjectChanged();
            _parameterIdentificationEngine = null;
         }
      }

      public void Stop()
      {
         _parameterIdentificationEngine?.Stop();
      }
   }
}