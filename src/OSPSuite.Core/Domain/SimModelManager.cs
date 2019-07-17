//TODO SIMMODEL


//using System;
//using System.Timers;
//using OSPSuite.Core.Domain.Data;
//using OSPSuite.Core.Domain.Services;
//using OSPSuite.Core.Serialization.SimModel.Services;
//
//namespace OSPSuite.Core.Domain
//{
//   public class SimModelManager : SimModelManagerBase, ISimModelManager
//   {
//      /// <summary>
//      ///    Maximum Time that one simulation run should need to
//      /// </summary>
//      private readonly double _executionTimeLimit;
//
//      private readonly Timer _timer;
//      private readonly IDataFactory _dataFactory;
//      private ISimulation _simModelSimulation;
//      private SimulationRunOptions _simulationRunOptions;
//      protected bool _canceled;
//      public event EventHandler<SimulationProgressEventArgs> SimulationProgress = delegate { };
//
//      public void StopSimulation()
//      {
//         _canceled = true;
//         if (_simModelSimulation == null) return;
//         _simModelSimulation.Cancel();
//      }
//
//      public SimModelManager(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IDataFactory dataFactory) : base(simModelExporter, simModelSimulationFactory)
//      {
//         _dataFactory = dataFactory;
//         _executionTimeLimit = 0;
//         _timer = new Timer {Interval = 1000};
//         _timer.Elapsed += onTimeElapsed;
//      }
//
//      private void onTimeElapsed(object sender, ElapsedEventArgs e)
//      {
//         if (_canceled) return;
//         raiseSimulationProgress(_simModelSimulation.Progress);
//      }
//
//      private void raiseSimulationProgress(int progress)
//      {
//         SimulationProgress(this, new SimulationProgressEventArgs(progress));
//      }
//
//      /// <summary>
//      ///    Simulates the active simulation
//      /// </summary>
//      public SimulationRunResults RunSimulation(IModelCoreSimulation simulation)
//      {
//         return RunSimulation(simulation, new SimulationRunOptions());
//      }
//
//      public SimulationRunResults RunSimulation(IModelCoreSimulation simulation, SimulationRunOptions simulationRunOptions)
//      {
//         try
//         {
//            _simulationRunOptions = simulationRunOptions;
//            _canceled = false;
//            doIfNotCanceled(() => loadSimulation(simulation));
//            doIfNotCanceled(() => FinalizeSimulation(_simModelSimulation));
//            doIfNotCanceled(simulate);
//
//            return new SimulationRunResults(!_canceled, WarningsFrom(_simModelSimulation.SolverWarnings), getResults(simulation));
//         }
//         finally
//         {
//            _simModelSimulation = null;
//            RaiseTerminated(this, EventArgs.Empty);
//         }
//      }
//
//      private void loadSimulation(IModelCoreSimulation simulation)
//      {
//         var xml = CreateSimulationExport(simulation, _simulationRunOptions.SimModelExportMode);
//         _simModelSimulation = CreateSimulation(xml);
//      }
//
//      private DataRepository getResults(IModelCoreSimulation simulation)
//      {
//         if (_canceled)
//            return new DataRepository();
//
//         return _dataFactory.CreateRepository(simulation, _simModelSimulation);
//      }
//
//      /// <summary>
//      ///    Starts a CoreSimulation run
//      /// </summary>
//      private void simulate()
//      {
//         _simModelSimulation.ShowProgress = true;
//         _simModelSimulation.ExecutionTimeLimit = _executionTimeLimit;
//         _simModelSimulation.CheckForNegativeValues = _simulationRunOptions.CheckForNegativeValues;
//
//         try
//         {
//            _timer.Start();
//            _simModelSimulation.RunSimulation();
//         }
//         finally
//         {
//            _timer.Stop();
//            _timer.Close();
//            raiseSimulationProgress(100);
//         }
//      }
//
//      protected void doIfNotCanceled(Action actionToExecute)
//      {
//         if (_canceled) return;
//         actionToExecute();
//      }
//   }
//}