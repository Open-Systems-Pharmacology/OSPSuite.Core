﻿using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
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
      private Simulation _simModelSimulation;
      private readonly Cache<string, double> _parameterValueCache = new Cache<string, double>();
      private readonly Cache<string, double> _initialValueCache = new Cache<string, double>();
      private IReadOnlyList<ParameterProperties> _allVariableParameters;
      private IReadOnlyList<SpeciesProperties> _allVariableMolecules;
      private string _simulationResultsName;
      private bool _calculateSensitivities;

      /// <summary>
      ///    This property is only required by the export of a model to C++ and must be set to true in this case BEFORE
      ///    the SimModel simulation is loaded from XML.
      /// </summary>
      public bool KeepXMLNodeInSimModelSimulation { get; set; }

      public bool CheckForNegativeValues { get; set; }

      public bool TreatConstantMoleculesAsParameters { get; set; } = true;

      public SimModelBatch(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IDataFactory dataFactory) : base(
         simModelExporter, simModelSimulationFactory)
      {
         _dataFactory = dataFactory;
         KeepXMLNodeInSimModelSimulation = false;
      }

      public IReadOnlyList<string> VariableParameterPaths { get; private set; }

      public IReadOnlyList<string> VariableMoleculePaths { get; private set; }

      public void InitializeWith(IModelCoreSimulation modelCoreSimulation, IReadOnlyList<string> variableParameterPaths,
         IReadOnlyList<string> variableMoleculePaths, bool calculateSensitivities = false, string simulationResultsName = null)
      {
         _modelCoreSimulation = modelCoreSimulation;
         _simulationResultsName = simulationResultsName;
         _calculateSensitivities = calculateSensitivities;
         _simModelSimulation = createAndFinalizeSimulation(variableParameterPaths, variableMoleculePaths);
      }

      public void InitializeForSensitivity()
      {
         InitializeWith(_modelCoreSimulation, VariableParameterPaths, VariableMoleculePaths, calculateSensitivities: true,
            simulationResultsName: _simulationResultsName);
      }

      private Simulation createAndFinalizeSimulation(IReadOnlyList<string> variableParameterPaths, IReadOnlyList<string> variableMoleculePaths)
      {
         var simulationExport = CreateSimulationExport(_modelCoreSimulation, SimModelExportMode.Optimized, variableMoleculePaths);
         var simulation = CreateSimulation(simulationExport, x =>
         {
            x.CheckForNegativeValues = CheckForNegativeValues;
            x.KeepXMLNodeAsString = KeepXMLNodeInSimModelSimulation;
         });
         setVariableParameters(simulation, variableParameterPaths);
         setVariableMolecules(simulation, variableMoleculePaths);
         FinalizeSimulation(simulation);
         return simulation;
      }

      private void setVariableParameters(Simulation simulation, IReadOnlyList<string> variableParameterPaths)
      {
         _allVariableParameters = SetVariableParameters(simulation, variableParameterPaths, _calculateSensitivities);
         VariableParameterPaths = _allVariableParameters.Select(x => x.Path).ToList();
      }

      private void setVariableMolecules(Simulation simulation, IReadOnlyList<string> variableMoleculePaths)
      {
         _allVariableMolecules = SetVariableMolecules(simulation, variableMoleculePaths);
         VariableMoleculePaths = _allVariableMolecules.Select(x => x.Path).ToList();
      }

      public SimulationRunResults RunSimulation()
      {
         try
         {
            updateSimulationValues();
            _simModelSimulation.RunSimulation();
            var hasResults = simulationHasResults(_simModelSimulation);

            return createSimulationRunResults();
         }
         catch (Exception e)
         {
            return createSimulationRunResults(e.FullMessage());
         }
         finally
         {
            _parameterValueCache.Clear();
            _initialValueCache.Clear();
         }
      }

      SimulationRunResults createSimulationRunResults(string errorFromException = null)
      {
         var hasResults = simulationHasResults(_simModelSimulation);
         var warnings = WarningsFrom(_simModelSimulation);
         var error = errorFromException ?? (hasResults ? null : Error.SimulationDidNotProduceResults);

         
         if (error == null)
            return new SimulationRunResults(warnings, getResults());

         return new SimulationRunResults(warnings, error);
      }

      private bool simulationHasResults(Simulation simModelSimulation) => simModelSimulation.AllValues.Any();

      public void UpdateParameterValue(string parameterPath, double value) => _parameterValueCache[parameterPath] = value;

      public void UpdateParameterValues(IReadOnlyList<double> parameterValues)
      {
         updateValues(parameterValues, VariableParameterPaths, _parameterValueCache, "parameter values");
      }

      public void UpdateInitialValue(string moleculePath, double value) => _initialValueCache[moleculePath] = value;

      public void UpdateInitialValues(IReadOnlyList<double> initialValues)
      {
         updateValues(initialValues, VariableMoleculePaths, _initialValueCache, "initial values");
      }

      private void updateValues(IReadOnlyList<double> values, IReadOnlyList<string> variablePaths, ICache<string, double> valuesCache,
         string updatedValues)
      {
         var count = values?.Count ?? 0;
         if (variablePaths.Count != count)
            throw new InvalidArgumentException($"No the expected number of {updatedValues} ({variablePaths.Count} vs {count})");

         //this should never be null here
         if (values == null)
            return;

         variablePaths.Each((path, i) => valuesCache[path] = values[i]);
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
         _initialValueCache.Clear();
      }

      private DataRepository getResults()
      {
         return _dataFactory.CreateRepository(_modelCoreSimulation, _simModelSimulation, _simulationResultsName);
      }

      private void updateSimulationValues()
      {
         _allVariableParameters.Each(p => p.Value = _parameterValueCache[p.Path]);
         _simModelSimulation.SetParameterValues();

         _allVariableMolecules.Each(p => p.InitialValue = _initialValueCache[p.Path]);
         _simModelSimulation.SetSpeciesValues();
      }

      public void ExportToCPPCode(string outputFolder, CodeExportMode exportMode, string modelName)
      {
         _simModelSimulation.ExportToCode(outputFolder, CodeExportLanguage.Cpp, exportMode, modelName);
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