﻿using System;
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
      private ParameterIdentification _parameterIdentification;
      private readonly CancellationTokenSource _cancellationTokenSource;
      private readonly List<IParameterIdentificationRun> _parameterIdentificationRuns;

      public ParameterIdentificationEngine(
         IEventPublisher eventPublisher, 
         IParameterIdentificationRunFactory parameterIdentificationRunFactory, 
         ICoreUserSettings coreUserSettings)
      {
         _eventPublisher = eventPublisher;
         _parameterIdentificationRunFactory = parameterIdentificationRunFactory;
         _coreUserSettings = coreUserSettings;
         _cancellationTokenSource = new CancellationTokenSource();
         _parameterIdentificationRuns = new List<IParameterIdentificationRun>();
      }

      public async Task StartAsync(ParameterIdentification parameterIdentification)
      {
         var token = _cancellationTokenSource.Token;
         _parameterIdentification = parameterIdentification;
         _eventPublisher.PublishEvent(new ParameterIdentificationStartedEvent(parameterIdentification));

         var results = new ConcurrentBag<ParameterIdentificationRunResult>();
         try
         {
            _parameterIdentificationRuns.AddRange(await createParameterIdentificationRuns(token));
            _parameterIdentificationRuns.Each(notifyRun);
            var parallelOptions = createParallelOptions(token);

            await Task.Run(() => Parallel.ForEach(_parameterIdentificationRuns, parallelOptions,
               run =>
               {
                  parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                  results.Add(run.Run(token));
               }), token);

            updateParameterIdentificationResults(results);
         }
         catch (OperationCanceledException)
         {
            var finishedResults = results.Where(runShouldBeKept);
            updateParameterIdentificationResults(finishedResults);
            throw;
         }
         finally
         {
            _parameterIdentificationRuns.Each(x => x.Dispose());
            _eventPublisher.PublishEvent(new ParameterIdentificationTerminatedEvent(parameterIdentification));
            _parameterIdentificationRuns.Clear();
         }
      }

      private void notifyRun(IParameterIdentificationRun parameterIdentificationRun)
      {
         var state = new ParameterIdentificationRunState(parameterIdentificationRun.RunResult, parameterIdentificationRun.BestResult, parameterIdentificationRun.TotalErrorHistory, parameterIdentificationRun.ParametersHistory);
         runStatusChanged(new ParameterIdentificationRunStatusEventArgs(state));
      }

      private bool runShouldBeKept(ParameterIdentificationRunResult runResult)
      {
         return runResult != null && runResult.Status.IsOneOf(RunStatus.RanToCompletion, RunStatus.Canceled);
      }

      private void updateParameterIdentificationResults(IEnumerable<ParameterIdentificationRunResult> finishedResults)
      {
         _parameterIdentification.UpdateResultsWith(finishedResults);
         _eventPublisher.PublishEvent(new ParameterIdentificationResultsUpdatedEvent(_parameterIdentification));
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
         _parameterIdentificationRuns.Each(x => x.Cancel());
         _parameterIdentificationRuns.Clear();
      }

      private async Task<IReadOnlyList<IParameterIdentificationRun>> createParameterIdentificationRuns(CancellationToken cancellationToken)
      {
         var parameterIdentificationRuns = await Task.Run(() => _parameterIdentificationRunFactory.CreateFor(_parameterIdentification, cancellationToken), cancellationToken);

         parameterIdentificationRuns.Each(run => { run.RunStatusChanged += (o, e) => runStatusChanged(e); });

         return parameterIdentificationRuns;
      }

      private void runStatusChanged(ParameterIdentificationRunStatusEventArgs e)
      {
         _eventPublisher.PublishEvent(new ParameterIdentificationIntermediateResultsUpdatedEvent(_parameterIdentification, e.State));
      }

      protected virtual void Cleanup()
      {
         _cancellationTokenSource.Dispose();
         _parameterIdentification = null;
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