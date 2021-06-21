using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Extensions;
using OSPSuite.SimModel;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Domain
{
   public class SimulationBatchOptions
   {
      //Potentially null
      public string[] VariableParameters { get; set; }

      //Potentially null. This will be used when one parameter only is varied
      public string VariableParameter { get; set; }

      //Potentially null
      public string[] VariableMolecules { get; set; }

      //Potentially null. This will be used when one molecule only is varied
      public string VariableMolecule { get; set; }

      public string[] Parameters => VariableParameters.ToNetArray(VariableParameter);

      public string[] Molecules => VariableMolecules.ToNetArray(VariableMolecule);
   }

   public class SimulationBatchRunValues
   {
      //Id to recognize it when running concurrently
      public string Id { get; set; }

      //Potentially null
      public double[] ParameterValues { get; set; }

      //Potentially null. This will be used when one parameter only is varied
      public double? ParameterValue { get; set; }

      //Potentially null
      public double[] InitialValues { get; set; }

      //Potentially null. This will be used when one molecule only is varied
      public double? InitialValue { get; set; }

      public double[] Values => ParameterValues.ToNetArray(ParameterValue);

      public double[] MoleculeValues => InitialValues.ToNetArray(InitialValue);
   }

   public class SimulationBatch : IDisposable
   {
      private readonly ISimModelBatch _simModelBatch;
      private readonly ISimulationResultsCreator _simulationResultsCreator;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;

      public SimulationBatch(ISimModelBatch simModelBatch,
         ISimulationResultsCreator simulationResultsCreator,
         ISimulationPersistableUpdater simulationPersistableUpdater
      )
      {
         _simModelBatch = simModelBatch;
         _simulationResultsCreator = simulationResultsCreator;
         _simulationPersistableUpdater = simulationPersistableUpdater;
      }

      public void Initialize(IModelCoreSimulation simulation, SimulationBatchOptions simulationBatchOptions)
      {
         _simModelBatch.InitializeWith(simulation, simulationBatchOptions.Parameters, simulationBatchOptions.Molecules);
         //This needs to be done after initialization of the SimModelBatch so that we can check parameters
         validate(simulationBatchOptions);
         _simulationPersistableUpdater.UpdateSimulationPersistable(simulation);
      }

      public void ExportToCPPCode(string outputFolder)
      {
         _simModelBatch.ExportToCPPCode(outputFolder, CodeExportMode.Values);
      }

      private void validate(SimulationBatchOptions simulationBatchOptions)
      {
         validate(simulationBatchOptions.Molecules, _simModelBatch.VariableMoleculePaths);
         validate(simulationBatchOptions.Parameters, _simModelBatch.VariableParameterPaths);
      }

      private void validate(IReadOnlyList<string> entitiesToVary, IReadOnlyList<string> entitiesThatWillBeVaried)
      {
         if (entitiesToVary == null)
            return;

         //It should never me smaller but just in case. 
         if (entitiesToVary.Count <= entitiesThatWillBeVaried.Count)
            return;

         //some entities meant to be varied are missing. Error
         throw new InvalidArgumentException(
            $"Entities {entitiesToVary.Except(entitiesThatWillBeVaried).ToString(", ", "'")} do not exist in the simulation");
      }

      public Task<SimulationResults> RunAsync(SimulationBatchRunValues simulationBatchRunValues)
      {
         return Task.Run(() =>
         {
            _simModelBatch.UpdateParameterValues(simulationBatchRunValues.Values);
            _simModelBatch.UpdateInitialValues(simulationBatchRunValues.MoleculeValues);
            var simulationResults = _simModelBatch.RunSimulation();
            return _simulationResultsCreator.CreateResultsFrom(simulationResults.Results);
         });
      }

      /// <summary>
      ///    Updates the parameter values and species initial values of the simulation and run the simulation synchronously.
      ///    This is really the only method that will be called from R
      /// </summary>
      /// <returns>Results of the simulation run</returns>
      public SimulationResults Run(SimulationBatchRunValues simulationBatchRunValues) =>
         RunAsync(simulationBatchRunValues).Result;

      protected virtual void Cleanup()
      {
         _simModelBatch.Dispose();
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

      ~SimulationBatch()
      {
         Cleanup();
      }

      #endregion
   }
}