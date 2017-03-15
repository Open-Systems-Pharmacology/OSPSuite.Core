using System;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisRunner
   {
      Task Run(SensitivityAnalysis sensitivityAnalysis);
      void Stop();
      bool IsRunning { get; }
   }

   public class SensitivityAnalysisRunner : ISensitivityAnalysisRunner
   {
      private readonly ISensitivityAnalysisEngineFactory _sensitivityAnalysisEngineFactory;
      private readonly IDialogCreator _dialogCreator;
      private readonly IEntityValidationTask _entityValidationTask;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private ISensitivityAnalysisEngine _sensitivityAnalysisEngine;

      public bool IsRunning => _sensitivityAnalysisEngine != null;

      public SensitivityAnalysisRunner(ISensitivityAnalysisEngineFactory sensitivityAnalysisEngineFactory,  IDialogCreator dialogCreator,
         IEntityValidationTask entityValidationTask, IOSPSuiteExecutionContext executionContext)
      {
         _sensitivityAnalysisEngineFactory = sensitivityAnalysisEngineFactory;
         _dialogCreator = dialogCreator;
         _entityValidationTask = entityValidationTask;
         _executionContext = executionContext;
      }

      public async Task Run(SensitivityAnalysis sensitivityAnalysis)
      {
         if (!_entityValidationTask.Validate(sensitivityAnalysis))
            return;

         try
         {
            if (IsRunning)
               throw new OSPSuiteException(Error.CannotStartTwoConcurrentSensitivityAnalyses);

            using (_sensitivityAnalysisEngine = _sensitivityAnalysisEngineFactory.Create())
            {
               var begin = SystemTime.UtcNow();
               await _sensitivityAnalysisEngine.StartAsync(sensitivityAnalysis);
               var end = SystemTime.UtcNow();
               var timeSpent = end - begin;
               _dialogCreator.MessageBoxInfo(Captions.SensitivityAnalysis.SensitivityAnalysisFinished(timeSpent.ToDisplay()));
            }
         }
         catch (OperationCanceledException)
         {
            _dialogCreator.MessageBoxInfo(Captions.SensitivityAnalysis.SensitivityAnalysisCanceled);
         }
         finally
         {
            _executionContext.ProjectChanged();
            _sensitivityAnalysisEngine = null;
         }
      }

      public void Stop()
      {
         _sensitivityAnalysisEngine?.Stop();
      }
   }
}