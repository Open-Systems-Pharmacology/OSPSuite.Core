using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Extensions;
using OSPSuite.SimModel;
using OSPSuite.Utility.Exceptions;
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

      //Defaults to false. If true, it will trigger the calculation of sensitivity
      public bool CalculateSensitivity { get; set; } = false;
   }

   public class SimulationBatchRunValues
   {
      //Id to recognize it when running concurrently
      public string Id { get; }

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

      public SimulationBatchRunValues()
      {
         Id = generateId();
      }

      private string generateId() => Guid.NewGuid().ToString();
   }

   public class SimulationBatch : IDisposable
   {
      private readonly ISimModelBatch _simModelBatch;
      private readonly ISimulationResultsCreator _simulationResultsCreator;
      private SimulationBatchOptions _simulationBatchOptions;

      public SimulationBatch(ISimModelBatch simModelBatch,
         ISimulationResultsCreator simulationResultsCreator
      )
      {
         _simModelBatch = simModelBatch;
         _simulationResultsCreator = simulationResultsCreator;
      }

      public void Initialize(IModelCoreSimulation simulation, SimulationBatchOptions simulationBatchOptions, SimulationRunOptions simulationRunOptions)
      {
         _simModelBatch.CheckForNegativeValues = simulation?.Settings?.Solver?.CheckForNegativeValues ?? true;

         //SimModel optionally caches XML used for loading a simulation as string.
         //This XML string was used e.g. by the old Matlab-/R-Toolbox when saving a simulation to XML.
         //C++ export also depends on the original XML string at the moment (not quite clear why).
         //Because per default XML is NOT cached, we need to set the KeepXML-option to true BEFORE loading a simulation.
         _simModelBatch.KeepXMLNodeInSimModelSimulation = true;
         _simModelBatch.InitializeWith(simulation, simulationBatchOptions.Parameters, simulationBatchOptions.Molecules, simulationBatchOptions.CalculateSensitivity);
         //This needs to be done after initialization of the SimModelBatch so that we can check parameters
         validate(simulationBatchOptions);
         _simulationBatchOptions = simulationBatchOptions;
      }

      /// <summary>
      ///    Export model as C++ code; keep parameters and initial values set in InitializeWith as variable
      /// </summary>
      /// <param name="outputFolder">Model .cpp file will be created here</param>
      /// <param name="fullMode">
      ///    If true: all parameters will be set as to be varied before export (will only have effect if SimModel simulation was
      ///    not finalized yet
      ///    If false: parameters will be simplified (where possible)
      /// </param>
      /// <param name="modelName">
      ///    If empty (default): model will be named Standard and exported to Standard.cpp
      ///    Otherwise: model will be named to "modelName" in the C++ code and exported to modelName.cpp.
      ///    modelName must be both valid file name AND valid C++ identifier in such a case
      /// </param>
      public void ExportToCPPCode(string outputFolder, bool fullMode, string modelName = "")
      {
         var exportMode = fullMode ? CodeExportMode.Formula : CodeExportMode.Values;
         _simModelBatch.ExportToCPPCode(outputFolder, exportMode, modelName);
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
         return Task.Run(() => Run(simulationBatchRunValues));
      }

      /// <summary>
      ///    Updates the parameter values and species initial values of the simulation and run the simulation synchronously.
      ///    This is really the only method that will be called from R
      /// </summary>
      /// <returns>Results of the simulation run</returns>
      public SimulationResults Run(SimulationBatchRunValues simulationBatchRunValues)
      {
         _simModelBatch.UpdateParameterValues(simulationBatchRunValues.Values);
         _simModelBatch.UpdateInitialValues(simulationBatchRunValues.MoleculeValues);
         var simulationRunResults = _simModelBatch.RunSimulation();

         if (!simulationRunResults.Success)
            throw new OSPSuiteException(simulationRunResults.Error);

         if (!_simulationBatchOptions.CalculateSensitivity)
            return _simulationResultsCreator.CreateResultsFrom(simulationRunResults.Results);

         return _simulationResultsCreator.CreateResultsWithSensitivitiesFrom(simulationRunResults.Results, _simModelBatch, _simulationBatchOptions.Parameters);
      }

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