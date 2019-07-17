//TODO SIMMODEL



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using OSPSuite.Core.Serialization.SimModel.Services;
//
//namespace OSPSuite.Core.Domain
//{
//   public abstract class SimModelManagerBase
//   {
//      protected ISimModelExporter _simModelExporter;
//      private readonly ISimModelSimulationFactory _simModelSimulationFactory;
//      public event EventHandler Terminated = delegate { };
//
//      protected SimModelManagerBase(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory)
//      {
//         _simModelExporter = simModelExporter;
//         _simModelSimulationFactory = simModelSimulationFactory;
//      }
//
//      /// <summary>
//      ///    Create the xml for simmodel based on the <paramref name="simulation" />
//      /// </summary>
//      protected string CreateSimulationExport(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode)
//      {
//         return _simModelExporter.Export(simulation, simModelExportMode);
//      }
//
//      /// <summary>
//      ///    Create the xml for simmodel based on the <paramref name="simulation" />
//      /// </summary>
//      protected Task<string> CreateSimulationExportAsync(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode)
//      {
//         return Task.Run(() => _simModelExporter.Export(simulation, simModelExportMode));
//      }
//
//      /// <summary>
//      ///    Performs any actions necessary to make the simulation run ready
//      /// </summary>
//      protected void FinalizeSimulation(ISimulation simModelSimulation)
//      {
//         simModelSimulation.FinalizeSimulation();
//      }
//
//      protected ISimulation CreateSimulation(string simulationExport)
//      {
//         var simulation = _simModelSimulationFactory.Create();
//         simulation.LoadFromXMLString(simulationExport);
//         return simulation;
//      }
//
//      protected void RaiseTerminated(object sender, EventArgs eventArgs)
//      {
//         Terminated(sender, eventArgs);
//      }
//
//      protected IEnumerable<SolverWarning> WarningsFrom(IEnumerable<ISolverWarning> warnings) =>
//         warnings.Select(x => new SolverWarning {Warning = x.Warning, OutputTime = x.OutputTime});
//   }
//}