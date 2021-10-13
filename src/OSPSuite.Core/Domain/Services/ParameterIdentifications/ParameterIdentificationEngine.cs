using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationEngine : IDisposable
   {
      Task StartAsync(ParameterIdentification parameterIdentification);
      void Stop();
   }

   public class ParameterIdentificationEngine : IParameterIdentificationEngine
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly IParameterIdentificationRunFactory _parameterIdentificationRunFactory;
      private readonly ICoreUserSettings _coreUserSettings;
      private readonly CancellationTokenSource _cancellationTokenSource;
      private readonly IConcurrencyManager _concurrencyManager;

      public ParameterIdentificationEngine(IEventPublisher eventPublisher, IParameterIdentificationRunFactory parameterIdentificationRunFactory, ICoreUserSettings coreUserSettings, IConcurrencyManager concurrentManager)
      {
         _eventPublisher = eventPublisher;
         _parameterIdentificationRunFactory = parameterIdentificationRunFactory;
         _coreUserSettings = coreUserSettings;
         _cancellationTokenSource = new CancellationTokenSource();
         _concurrencyManager = concurrentManager;
      }

      public async Task StartAsync(ParameterIdentification parameterIdentification)
      {
         var token = _cancellationTokenSource.Token;
         _eventPublisher.PublishEvent(new ParameterIdentificationStartedEvent(parameterIdentification));

         var parameterIdentificationRuns = new List<IParameterIdentificationRun>();
         try
         {
            parameterIdentificationRuns.AddRange(await createParameterIdentificationRuns(parameterIdentification, token));
            parameterIdentificationRuns.Each((pir) => notifyRun(parameterIdentification, pir));
            var parallelOptions = createParallelOptions(token);

            var results = (await 
               _concurrencyManager.RunAsync(
                  _coreUserSettings.MaximumNumberOfCoresToUse, 
                  parameterIdentificationRuns, 
                  run => Guid.NewGuid().ToString(), 
                  (core, run, ct) => Task.Run(() => run.Run(ct), ct), 
                  token
               )).Select(r => r.Value.Result);
               
            updateParameterIdentificationResults(parameterIdentification, results);
         }
         /*catch (OperationCanceledException)
         {
            var finishedResults = results.Where(runShouldBeKept);
            updateParameterIdentificationResults(parameterIdentification, finishedResults);
            throw;
         }*/
         finally
         {
            parameterIdentificationRuns.Each(x => x.Dispose());
            _eventPublisher.PublishEvent(new ParameterIdentificationTerminatedEvent(parameterIdentification));
         }
      }

      private void notifyRun(ParameterIdentification parameterIdentification, IParameterIdentificationRun parameterIdentificationRun)
      {
         var state = new ParameterIdentificationRunState(parameterIdentificationRun.RunResult, parameterIdentificationRun.BestResult, parameterIdentificationRun.TotalErrorHistory, parameterIdentificationRun.ParametersHistory);
         runStatusChanged(parameterIdentification, new ParameterIdentificationRunStatusEventArgs(state));
      }

      private bool runShouldBeKept(ParameterIdentificationRunResult runResult)
      {
         return runResult != null && runResult.Status.IsOneOf(RunStatus.RanToCompletion, RunStatus.Canceled);
      }

      private void updateParameterIdentificationResults(ParameterIdentification parameterIdentification, IEnumerable<ParameterIdentificationRunResult> finishedResults)
      {
         parameterIdentification.UpdateResultsWith(finishedResults);
         _eventPublisher.PublishEvent(new ParameterIdentificationResultsUpdatedEvent(parameterIdentification));
      }

      private ParallelOptions createParallelOptions(CancellationToken token)
      {
         return new ParallelOptions
         {
            CancellationToken = token,
            MaxDegreeOfParallelism = _coreUserSettings.MaximumNumberOfCoresToUse
         };
      }

      public void Stop()
      {
         _cancellationTokenSource.Cancel();
      }

      private async Task<IReadOnlyList<IParameterIdentificationRun>> createParameterIdentificationRuns(ParameterIdentification parameterIdentification, CancellationToken cancellationToken)
      {
         var parameterIdentificationRuns = await Task.Run(() => _parameterIdentificationRunFactory.CreateFor(parameterIdentification, cancellationToken), cancellationToken);

         parameterIdentificationRuns.Each(run => { run.RunStatusChanged += (o, e) => runStatusChanged(parameterIdentification, e); });

         return parameterIdentificationRuns;
      }

      private void runStatusChanged(ParameterIdentification parameterIdentification, ParameterIdentificationRunStatusEventArgs e)
      {
         _eventPublisher.PublishEvent(new ParameterIdentificationIntermediateResultsUpdatedEvent(parameterIdentification, e.State));
      }

      protected virtual void Cleanup()
      {
         _cancellationTokenSource.Dispose();
      }

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~ParameterIdentificationEngine()
      {
         Cleanup();
      }

      #endregion
   }
}