using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.SimModel;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public class SimModelBatch : SimModelManagerBase, ISimModelBatch
   {
      private readonly IDataFactory _dataFactory;
      private IModelCoreSimulation _modelCoreSimulation;
      private IReadOnlyList<string> _variableParameterPaths;
      private Simulation _simModelSimulation;
      private readonly Cache<string, double> _parameterValueCache;
      private List<ParameterProperties> _allVariableParameters;
      private string _simulationResultsName;
      private bool _calculateSensitivities;

      public SimModelBatch(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IDataFactory dataFactory) : base(simModelExporter, simModelSimulationFactory)
      {
         _dataFactory = dataFactory;
         _parameterValueCache = new Cache<string, double>();
      }

      public void InitializeWith(IModelCoreSimulation modelCoreSimulation, IReadOnlyList<string> variableParameterPaths, bool calculateSensitivities = false, string simulationResultsName = null)
      {
         _modelCoreSimulation = modelCoreSimulation;
         _variableParameterPaths = variableParameterPaths;
         _simulationResultsName = simulationResultsName;
         _calculateSensitivities = calculateSensitivities;
         _simModelSimulation = createAndFinalizeSimulation();
         _allVariableParameters = _simModelSimulation.VariableParameters.ToList();
      }

      public void InitializeForSensitivity()
      {
         InitializeWith(_modelCoreSimulation, _variableParameterPaths, calculateSensitivities: true, simulationResultsName: _simulationResultsName);
      }

      private Simulation createAndFinalizeSimulation()
      {
         var simulationExport = CreateSimulationExport(_modelCoreSimulation, SimModelExportMode.Optimized);
         var simulation = CreateSimulation(simulationExport, x => x.CheckForNegativeValues = false);
         setVariableParameters(simulation);
         FinalizeSimulation(simulation);
         return simulation;
      }

      private void setVariableParameters(Simulation simulation) => SetVariableParameters(simulation, _variableParameterPaths, _calculateSensitivities);

      public SimulationRunResults RunSimulation()
      {
         try
         {
            updateParameterValues();
            _simModelSimulation.RunSimulation();
            return new SimulationRunResults(success: true, warnings: WarningsFrom(_simModelSimulation), results: getResults());
         }
         catch (Exception e)
         {
            return new SimulationRunResults(success: false, warnings: WarningsFrom(_simModelSimulation), results: new DataRepository(), error: e.FullMessage());
         }
         finally
         {
            _parameterValueCache.Clear();
         }
      }

      public void UpdateParameterValue(string path, double value)
      {
         _parameterValueCache[path] = value;
      }

      public void StopSimulation()
      {
         _simModelSimulation?.Cancel();
      }

      public double[] SensitivityValuesFor(string outputPath, string parameterPath)
      {
         return _simModelSimulation.SensitivityValuesByPathFor(outputPath, parameterPath);
      }

      protected virtual void Cleanup()
      {
         _modelCoreSimulation = null;
         _simModelSimulation?.Dispose();
         _simModelSimulation = null;
         _parameterValueCache.Clear();
         _allVariableParameters.Clear();
      }

      private DataRepository getResults()
      {
         return _dataFactory.CreateRepository(_modelCoreSimulation, _simModelSimulation, _simulationResultsName);
      }

      private void updateParameterValues()
      {
         foreach (var variableParameter in _allVariableParameters)
         {
            variableParameter.Value = _parameterValueCache[variableParameter.Path];
         }

         _simModelSimulation.SetParameterValues();
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

      ~SimModelBatch()
      {
         Cleanup();
      }

      #endregion
   }
}