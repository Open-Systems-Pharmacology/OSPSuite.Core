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
      protected string CreateSimulationExport(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode)
      {
         return _simModelExporter.Export(simulation, simModelExportMode);
      }

      /// <summary>
      ///    Create the xml for simmodel based on the <paramref name="simulation" />
      /// </summary>
      protected Task<string> CreateSimulationExportAsync(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode)
      {
         return Task.Run(() => _simModelExporter.Export(simulation, simModelExportMode));
      }

      /// <summary>
      ///    Performs any actions necessary to make the simulation run ready
      /// </summary>
      protected void FinalizeSimulation(Simulation simModelSimulation)
      {
         simModelSimulation.FinalizeSimulation();
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
         var allParameters = simulation.ParameterProperties;
         var parametersToBeVaried = allParameters.Where(p => variableParameterPaths.Contains(p.Path)).ToList();

         parametersToBeVaried.Each(p => p.CalculateSensitivity = calculateSensitivities);

         simulation.VariableParameters = parametersToBeVaried;
         return parametersToBeVaried;
      }

      protected IReadOnlyList<SpeciesProperties> SetVariableSpecies(Simulation simulation, IReadOnlyList<string> variableSpeciesPaths)
      {
         var allSpecies = simulation.SpeciesProperties;
         var speciesToBeVaried = allSpecies.Where(p => variableSpeciesPaths.Contains(p.Path)).ToList();
         simulation.VariableSpecies = speciesToBeVaried;
         return speciesToBeVaried;
      }

      protected IEnumerable<SolverWarning> WarningsFrom(Simulation simModelSimulation) => simModelSimulation.SolverWarnings;
   }
}