using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.SimModel;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public abstract class SimModelManagerBase
   {
      protected ISimModelExporter _simModelExporter;
      private readonly ISimModelSimulationFactory _simModelSimulationFactory;
      public event EventHandler Terminated = delegate { };

      protected SimModelManagerBase(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory)
      {
         _simModelExporter = simModelExporter;
         _simModelSimulationFactory = simModelSimulationFactory;
      }

      /// <summary>
      ///    Create the xml for simmodel based on the <paramref name="simulation" />
      /// </summary>
      protected string CreateSimulationExport(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode, IReadOnlyList<string> variableMoleculePaths = null)
      {
         return _simModelExporter.ExportSimModelXml(simulation, simModelExportMode, variableMoleculePaths);
      }

      /// <summary>
      ///    Create the xml for simmodel based on the <paramref name="simulation" />
      /// </summary>
      protected Task<string> CreateSimulationExportAsync(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode, IReadOnlyList<string> variableMoleculePaths = null)
      {
         return Task.Run(() => _simModelExporter.ExportSimModelXml(simulation, simModelExportMode, variableMoleculePaths));
      }

      /// <summary>
      ///    Performs any actions necessary to make the simulation run ready
      /// </summary>
      protected void FinalizeSimulation(Simulation simModelSimulation)
      {
         simModelSimulation.FinalizeSimulation();
      }

      protected async Task FinalizeSimulationAsync(Simulation simModelSimulation)
      {
         await Task.Run(() => simModelSimulation.FinalizeSimulation());
      }

      protected Simulation CreateSimulation(string simulationExport, Action<SimulationOptions> configureSimulationAction = null)
      {
         var simulation = _simModelSimulationFactory.Create();
         configureSimulationAction?.Invoke(simulation.Options);
         simulation.LoadFromXMLString(simulationExport);
         return simulation;
      }

      protected void RaiseTerminated(object sender, EventArgs eventArgs)
      {
         Terminated(sender, eventArgs);
      }

      protected IReadOnlyList<ParameterProperties> SetVariableParameters(Simulation simulation, IReadOnlyList<string> variableParameterPaths, bool calculateSensitivities)
      {
         if (variableParameterPaths == null)
            return Array.Empty<ParameterProperties>();
         
         var parametersToBeVaried = variableParameterPaths.Join(
            simulation.ParameterProperties,
            path => path,
            p => p.Path,
            (path, p) => p
         ).ToList();

         parametersToBeVaried.Each(p => p.CalculateSensitivity = calculateSensitivities);

         simulation.VariableParameters = parametersToBeVaried;
         return parametersToBeVaried;
      }

      protected IReadOnlyList<SpeciesProperties> SetVariableMolecules(Simulation simulation, IReadOnlyList<string> variableMoleculePaths)
      {

         if (variableMoleculePaths == null)
            return Array.Empty<SpeciesProperties>();

         var moleculeToBeVaried = variableMoleculePaths.Join(
            simulation.SpeciesProperties,
            path => path,
            m => m.Path,
            (path, m) => m
         ).ToList();
         simulation.VariableSpecies = moleculeToBeVaried;
         return moleculeToBeVaried;
      }

      protected IEnumerable<SolverWarning> WarningsFrom(Simulation simModelSimulation) => simModelSimulation.SolverWarnings;
   }
}