using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.SimModel;
using static OSPSuite.Assets.Captions;
using Timer = System.Timers.Timer;

namespace OSPSuite.Core.Domain.Services
{
   public class SimModelManager : SimModelManagerBase, ISimModelManager
   {
      private readonly double _executionTimeLimit;
      private readonly Timer _timer;
      private readonly IDataFactory _dataFactory;
      private Simulation _simModelSimulation;
      private SimulationRunOptions _simulationRunOptions;
      private CancellationTokenSource _cancellationTokenSource;

      public event EventHandler<SimulationProgressEventArgs> SimulationProgress = delegate { };

      public SimModelManager(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IDataFactory dataFactory) : base(simModelExporter, simModelSimulationFactory)
      {
         _dataFactory = dataFactory;
         _executionTimeLimit = 0;
         _timer = new Timer { Interval = 1000 };
         _timer.Elapsed += onTimeElapsed;
      }

      public void StopSimulation()
      {
         _cancellationTokenSource?.Cancel();
      }

      private void onTimeElapsed(object sender, ElapsedEventArgs e)
      {
         if (_simModelSimulation == null)
            return;

         raiseSimulationProgress(_simModelSimulation.Progress);
      }

      private void raiseSimulationProgress(int progress)
      {
         SimulationProgress(this, new SimulationProgressEventArgs(progress));
      }

      public SimulationRunResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         var cts = new CancellationTokenSource();
         try
         {
            return RunSimulationAsync(simulation, cts, simulationRunOptions).GetAwaiter().GetResult();
         }
         finally
         {
            cts.Dispose();
         }
      }

      public async Task<SimulationRunResults> RunSimulationAsync(IModelCoreSimulation simulation, CancellationTokenSource cts, SimulationRunOptions simulationRunOptions = null)
      {
         _simulationRunOptions = simulationRunOptions ?? new SimulationRunOptions();
         _cancellationTokenSource = cts;
         var cancellationToken = cts.Token;
         try
         {
            await loadAndRunSimulationAsync(simulation, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
               return new SimulationRunResults(WarningsFrom(_simModelSimulation), _dataFactory.CreateRepository(simulation, _simModelSimulation));

            return new SimulationRunResults(WarningsFrom(_simModelSimulation), SimulationWasCanceled);
         }
         catch (OperationCanceledException)
         {
            return new SimulationRunResults(WarningsFrom(_simModelSimulation), SimulationWasCanceled);
         }
         finally
         {
            _simModelSimulation?.Dispose();
            _simModelSimulation = null;
            RaiseTerminated(this, EventArgs.Empty);
         }
      }

      private async Task loadAndRunSimulationAsync(IModelCoreSimulation simulation, CancellationToken cancellationToken)
      {
         await doIfNotCanceledAsync(() => loadSimulationAsync(simulation), cancellationToken);
         await doIfNotCanceledAsync(() => FinalizeSimulationAsync(_simModelSimulation), cancellationToken);
         await doIfNotCanceledAsync(() => simulateAsync(cancellationToken), cancellationToken);
      }

      private void loadSimulation(IModelCoreSimulation simulation)
      {
         var xml = CreateSimulationExport(simulation, _simulationRunOptions.SimModelExportMode);
         _simModelSimulation = CreateSimulation(xml);
      }

      private async Task loadSimulationAsync(IModelCoreSimulation simulation)
      {
         await Task.Run(() => loadSimulation(simulation));
      }

      protected async Task FinalizeSimulationAsync(Simulation simModelSimulation)
      {
         await Task.Run(() => simModelSimulation.FinalizeSimulation());
      }

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