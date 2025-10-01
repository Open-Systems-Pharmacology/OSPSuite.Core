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
      private static readonly SemaphoreSlim NativeGate = new SemaphoreSlim(1, 1);

      private readonly double _executionTimeLimit;
      private readonly Timer _timer;
      private readonly IDataFactory _dataFactory;
      private Simulation _simModelSimulation;
      private SimulationRunOptions _simulationRunOptions;

      public event EventHandler<SimulationProgressEventArgs> SimulationProgress = delegate { };

      public SimModelManager(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IDataFactory dataFactory) : base(simModelExporter, simModelSimulationFactory)
      {
         _dataFactory = dataFactory;
         _executionTimeLimit = 0;
         _timer = new Timer { Interval = 1000 };
         _timer.Elapsed += onTimeElapsed;
      }

      private void onTimeElapsed(object sender, ElapsedEventArgs e)
      {
         var sim = _simModelSimulation;
         if (sim == null) return;

         if (!NativeGate.Wait(0)) return; // skip if we cannot get the gate immediately (to avoid overlapping calls)
         try
         {
            raiseSimulationProgress(sim.Progress);
         }
         finally
         {
            NativeGate.Release();
         }
      }

      private void raiseSimulationProgress(int progress)
      {
         SimulationProgress(this, new SimulationProgressEventArgs(progress));
      }

      public SimulationRunResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         return RunSimulationAsync(simulation, simulationRunOptions: simulationRunOptions).GetAwaiter().GetResult();
      }

      public async Task<SimulationRunResults> RunSimulationAsync(IModelCoreSimulation simulation, CancellationToken cancellationToken = default, SimulationRunOptions simulationRunOptions = null)
      {
         _simulationRunOptions = simulationRunOptions ?? new SimulationRunOptions();

         try
         {
            await loadAndRunSimulationAsync(simulation, cancellationToken);

            return new SimulationRunResults(
               WarningsFrom(_simModelSimulation),
               _dataFactory.CreateRepository(simulation, _simModelSimulation));
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

      private async Task loadAndRunSimulationAsync(IModelCoreSimulation simulation, CancellationToken ct)
      {
         await NativeGate.WaitAsync(ct).ConfigureAwait(false);
         try
         {
            ct.ThrowIfCancellationRequested();
            loadSimulation(simulation);
            ct.ThrowIfCancellationRequested();
            FinalizeSimulation(_simModelSimulation);
            ct.ThrowIfCancellationRequested();

            using (ct.Register(() => _simModelSimulation?.Cancel()))
            {
               await Task.Run(simulate, ct).ConfigureAwait(false);
            }

            ct.ThrowIfCancellationRequested();
         }
         finally
         {
            NativeGate.Release();
         }
      }

      private void loadSimulation(IModelCoreSimulation simulation)
      {
         var xml = CreateSimulationExport(simulation, _simulationRunOptions.SimModelExportMode);
         _simModelSimulation = CreateSimulation(xml);
      }

      private void simulate()
      {
         var options = _simModelSimulation.Options;
         options.ShowProgress = true;
         options.ExecutionTimeLimit = _executionTimeLimit;
         options.CheckForNegativeValues = _simulationRunOptions.CheckForNegativeValues;

         try
         {
            _timer.Start();
            _simModelSimulation.RunSimulation();
         }
         finally
         {
            _timer.Elapsed -= onTimeElapsed;
            _timer.Stop();
            _timer.Dispose();
            raiseSimulationProgress(100);
         }
      }
   }
}