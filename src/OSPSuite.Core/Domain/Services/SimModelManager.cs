using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.SimModel;
using static OSPSuite.Assets.Captions;
using Timer = System.Timers.Timer;

namespace OSPSuite.Core.Domain.Services
{
   public class SimModelManager : SimModelManagerBase, ISimModelManager
   {
      /// <summary>
      ///    Maximum Time that one simulation run should need to
      /// </summary>
      private readonly double _executionTimeLimit;

      private readonly Timer _timer;
      private readonly IDataFactory _dataFactory;
      private Simulation _simModelSimulation;
      private SimulationRunOptions _simulationRunOptions;
      private static CancellationTokenSource _globalCancellationTokenSource = new CancellationTokenSource();
      private static readonly object _lock = new object();

      public event EventHandler<SimulationProgressEventArgs> SimulationProgress = delegate { };

      public void StopSimulation()
      {
         lock (_lock)
         {
            _globalCancellationTokenSource.Cancel();
            _globalCancellationTokenSource.Dispose();
            _globalCancellationTokenSource = new CancellationTokenSource();
         }
      }

      public SimModelManager(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IDataFactory dataFactory) : base(simModelExporter, simModelSimulationFactory)
      {
         _dataFactory = dataFactory;
         _executionTimeLimit = 0;
         _timer = new Timer { Interval = 1000 };
         _timer.Elapsed += onTimeElapsed;
      }

      private void onTimeElapsed(object sender, ElapsedEventArgs e)
      {
         if (_globalCancellationTokenSource.IsCancellationRequested) return;
         raiseSimulationProgress(_simModelSimulation.Progress);
      }

      private void raiseSimulationProgress(int progress)
      {
         SimulationProgress(this, new SimulationProgressEventArgs(progress));
      }

      public SimulationRunResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         return RunSimulationAsync(simulation, simulationRunOptions).GetAwaiter().GetResult();
      }

      public async Task<SimulationRunResults> RunSimulationAsync(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         var cancellationToken = _globalCancellationTokenSource.Token;

         try
         {
            _simulationRunOptions = simulationRunOptions ?? new SimulationRunOptions();

            await doIfNotCanceledAsync(() => loadSimulationAsync(simulation), cancellationToken);
            await doIfNotCanceledAsync(() => FinalizeSimulationAsync(_simModelSimulation), cancellationToken);
            await doIfNotCanceledAsync(() => simulateAsync(cancellationToken), cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
               return new SimulationRunResults(WarningsFrom(_simModelSimulation), getResults(simulation));

            return new SimulationRunResults(WarningsFrom(_simModelSimulation), SimulationWasCanceled);
         }
         finally
         {
            _simModelSimulation?.Dispose();
            _simModelSimulation = null;
            RaiseTerminated(this, EventArgs.Empty);
         }
      }

      private void loadSimulation(IModelCoreSimulation simulation)
      {
         var xml = CreateSimulationExport(simulation, _simulationRunOptions.SimModelExportMode);
         _simModelSimulation = CreateSimulation(xml);
      }

      private async Task loadSimulationAsync(IModelCoreSimulation simulation)
      {
         await Task.Run(() => loadSimulation(simulation)); // No need for a token here, it's a lightweight operation
      }

      private DataRepository getResults(IModelCoreSimulation simulation)
      {
         return _globalCancellationTokenSource.IsCancellationRequested
            ? new DataRepository()
            : _dataFactory.CreateRepository(simulation, _simModelSimulation);
      }

      /// <summary>
      ///    Starts a CoreSimulation run asynchronously
      /// </summary>
      private async Task simulateAsync(CancellationToken cancellationToken)
      {
         var options = _simModelSimulation.Options;
         options.ShowProgress = true;
         options.ExecutionTimeLimit = _executionTimeLimit;
         options.CheckForNegativeValues = _simulationRunOptions.CheckForNegativeValues;

         try
         {
            _timer.Start();
            await Task.Run(() =>
            {
               cancellationToken.ThrowIfCancellationRequested();

               _simModelSimulation.RunSimulation();

               cancellationToken.ThrowIfCancellationRequested();
            }, cancellationToken);
         }
         catch (TaskCanceledException)
         {
         }
         finally
         {
            _timer.Stop();
            _timer.Close();
            raiseSimulationProgress(100);
         }
      }

      protected async Task doIfNotCanceledAsync(Func<Task> asyncAction, CancellationToken cancellationToken)
      {
         if (cancellationToken.IsCancellationRequested) return;
         await asyncAction();
      }
   }
}