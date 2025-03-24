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
         return RunSimulationAsync(simulation, simulationRunOptions: simulationRunOptions).GetAwaiter().GetResult();
      }

      public async Task<SimulationRunResults> RunSimulationAsync(IModelCoreSimulation simulation, CancellationToken cancellationToken = default, SimulationRunOptions simulationRunOptions = null)
      {
         _simulationRunOptions = simulationRunOptions ?? new SimulationRunOptions();
         try
         {
            await loadAndRunSimulationAsync(simulation, cancellationToken);

            return new SimulationRunResults(WarningsFrom(_simModelSimulation), _dataFactory.CreateRepository(simulation, _simModelSimulation));
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

      private Task loadAndRunSimulationAsync(IModelCoreSimulation simulation, CancellationToken cancellationToken) =>
      Task.Run(() =>
         {
            cancellationToken.ThrowIfCancellationRequested();
            loadSimulation(simulation);
            cancellationToken.ThrowIfCancellationRequested();
            FinalizeSimulation(_simModelSimulation);
            cancellationToken.ThrowIfCancellationRequested();
            simulate();
            cancellationToken.ThrowIfCancellationRequested();
         }, cancellationToken);
      
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
            _timer.Stop();
            _timer.Close();
            raiseSimulationProgress(100);
         }
      }
   }
}