using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisRunner
   {
      Task Run(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions = null);
      void Stop();
      bool IsRunning { get; }
   }

   public class SensitivityAnalysisRunner : ISensitivityAnalysisRunner
   {
      private readonly ISensitivityAnalysisEngineFactory _sensitivityAnalysisEngineFactory;
      private readonly IDialogCreator _dialogCreator;
      private readonly IEntityValidationTask _entityValidationTask;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly ICoreUserSettings _coreUserSettings;
      private ISensitivityAnalysisEngine _sensitivityAnalysisEngine;
      private readonly ConcurrentDictionary<SensitivityAnalysis, CancellationTokenSource> _cancellationTokenSources = new ConcurrentDictionary<SensitivityAnalysis, CancellationTokenSource>();

      public bool IsRunning => _sensitivityAnalysisEngine != null;

      public SensitivityAnalysisRunner(
         ISensitivityAnalysisEngineFactory sensitivityAnalysisEngineFactory,
         IDialogCreator dialogCreator,
         IEntityValidationTask entityValidationTask,
         IOSPSuiteExecutionContext executionContext,
         ICoreUserSettings coreUserSettings
      )
      {
         _sensitivityAnalysisEngineFactory = sensitivityAnalysisEngineFactory;
         _dialogCreator = dialogCreator;
         _entityValidationTask = entityValidationTask;
         _executionContext = executionContext;
         _coreUserSettings = coreUserSettings;
      }

      public async Task Run(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunOptions runOptions = null)
      {
         if (!_entityValidationTask.Validate(sensitivityAnalysis))
            return;

         try
         {
            var cts = new CancellationTokenSource();
            if (IsRunning)
               throw new OSPSuiteException(Error.CannotStartTwoConcurrentSensitivityAnalyses);

            if (!_cancellationTokenSources.TryAdd(sensitivityAnalysis, cts)) //this will prevent from running one that is already running
               throw new OSPSuiteException(Error.CannotStartTwoConcurrentSensitivityAnalyses);

            var options = runOptions ?? new SensitivityAnalysisRunOptions { NumberOfCoresToUse = _coreUserSettings.MaximumNumberOfCoresToUse };
            using (_sensitivityAnalysisEngine = _sensitivityAnalysisEngineFactory.Create())
            {
               var begin = SystemTime.UtcNow();
               await _sensitivityAnalysisEngine.StartAsync(sensitivityAnalysis, options, cts.Token);
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
         foreach (var sensitivityAnalysis in _cancellationTokenSources.Keys.ToList())
         {
            if (_cancellationTokenSources.TryRemove(sensitivityAnalysis, out var cts))
            {
               cts.Cancel();
               cts.Dispose();
            }
         }
      }
   }
}