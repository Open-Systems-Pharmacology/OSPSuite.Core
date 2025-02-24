using System;
using System.Timers;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.SimModel;
using static OSPSuite.Assets.Captions;

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
      protected bool _canceled;
      public event EventHandler<SimulationProgressEventArgs> SimulationProgress = delegate { };

      public void StopSimulation()
      {
         _canceled = true;
         _simModelSimulation?.Cancel();
      }

      public SimModelManager(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IDataFactory dataFactory) : base(simModelExporter, simModelSimulationFactory)
      {
         _dataFactory = dataFactory;
         _executionTimeLimit = 0;
         _timer = new Timer {Interval = 1000};
         _timer.Elapsed += onTimeElapsed;
      }

      private void onTimeElapsed(object sender, ElapsedEventArgs e)
      {
         if (_canceled) return;
         raiseSimulationProgress(_simModelSimulation.Progress);
      }

      private void raiseSimulationProgress(int progress)
      {
         SimulationProgress(this, new SimulationProgressEventArgs(progress));
      }

      public SimulationRunResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions = null)
      {
         try
         {
            _simulationRunOptions = simulationRunOptions ?? new SimulationRunOptions();
            _canceled = false;
            doIfNotCanceled(() => loadSimulation(simulation));
            doIfNotCanceled(() => FinalizeSimulation(_simModelSimulation));
            doIfNotCanceled(() => simulate(simulation));

            if(!_canceled)
               return new SimulationRunResults(WarningsFrom( _simModelSimulation), getResults(simulation));

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

      private DataRepository getResults(IModelCoreSimulation simulation)
      {
         if (_canceled)
            return new DataRepository();

         return _dataFactory.CreateRepository(simulation, _simModelSimulation);
      }

      /// <summary>
      ///    Starts a CoreSimulation run
      /// </summary>
      private void simulate(IModelCoreSimulation simulation)
      {
         var options = _simModelSimulation.Options;
         options.ShowProgress = true;
         options.ExecutionTimeLimit = _executionTimeLimit;
         options.CheckForNegativeValues = simulation.Settings.Solver.CheckForNegativeValues;
         options.AutoReduceTolerances = simulation.Settings.Solver.AutoReduceTolerances;

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

      protected void doIfNotCanceled(Action actionToExecute)
      {
         if (_canceled) return;
         actionToExecute();
      }
   }
}