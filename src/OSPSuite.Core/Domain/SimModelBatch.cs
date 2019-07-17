//TODO SIMMODEL



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using OSPSuite.Core.Domain.Data;
//using OSPSuite.Core.Domain.Services;
//using OSPSuite.Core.Serialization.SimModel.Services;
//using OSPSuite.Utility.Collections;
//using OSPSuite.Utility.Extensions;
//
//namespace OSPSuite.Core.Domain
//{
//   public class SimModelBatch : SimModelManagerBase, ISimModelBatch
//   {
//      private readonly IDataFactory _dataFactory;
//      private IModelCoreSimulation _modelCoreSimulation;
//      private IReadOnlyList<string> _variableParameterPaths;
//      private ISimulation _simModelSimulation;
//      private readonly Cache<string, double> _parameterValueCache;
//      private IList<IParameterProperties> _allVariableParameters;
//      private string _simulationResultsName;
//      private bool _calculateSensitivities;
//
//      public SimModelBatch(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IDataFactory dataFactory) : base(simModelExporter, simModelSimulationFactory)
//      {
//         _dataFactory = dataFactory;
//         _parameterValueCache = new Cache<string, double>();
//      }
//
//      public void InitializeWith(IModelCoreSimulation modelCoreSimulation, IReadOnlyList<string> variableParameterPaths, bool calculateSensitivities = false, string simulationResultsName = null)
//      {
//         _modelCoreSimulation = modelCoreSimulation;
//         _variableParameterPaths = variableParameterPaths;
//         _simulationResultsName = simulationResultsName;
//         _calculateSensitivities = calculateSensitivities;
//         _simModelSimulation = createAndFinalizeSimulation();
//         _simModelSimulation.CheckForNegativeValues = false;
//         _allVariableParameters = _simModelSimulation.VariableParameters;
//      }
//
//      public void InitializeForSensitivity()
//      {
//         InitializeWith(_modelCoreSimulation, _variableParameterPaths, calculateSensitivities: true, simulationResultsName: _simulationResultsName);
//      }
//
//      private ISimulation createAndFinalizeSimulation()
//      {
//         var simulationExport = CreateSimulationExport(_modelCoreSimulation, SimModelExportMode.Optimized);
//         var simulation = CreateSimulation(simulationExport);
//         setVariableParameters(simulation);
//         FinalizeSimulation(simulation);
//         return simulation;
//      }
//
//      private void setVariableParameters(ISimulation simulation)
//      {
//         var allParameters = simulation.ParameterProperties;
//         var parametersToBeVaried = allParameters.Where(p => _variableParameterPaths.Contains(p.Path)).ToList();
//
//         parametersToBeVaried.Each(p => p.CalculateSensitivity = _calculateSensitivities);
//
//         simulation.VariableParameters = parametersToBeVaried;
//      }
//
//      public SimulationRunResults RunSimulation()
//      {
//         try
//         {
//            updateParameterValues();
//            _simModelSimulation.RunSimulation();
//            return new SimulationRunResults(success: true, warnings: WarningsFrom(_simModelSimulation.SolverWarnings), results: getResults());
//         }
//         catch (Exception e)
//         {
//            return new SimulationRunResults(success: false, warnings: WarningsFrom(_simModelSimulation.SolverWarnings), results: new DataRepository(), error: e.FullMessage());
//         }
//         finally
//         {
//            _parameterValueCache.Clear();
//         }
//      }
//
//      public void UpdateParameterValue(string path, double value)
//      {
//         _parameterValueCache[path] = value;
//      }
//
//      public void StopSimulation()
//      {
//         _simModelSimulation?.Cancel();
//      }
//
//      public double[] SensitivityValuesFor(string outputPath, string parameterPath)
//      {
//         return _simModelSimulation.SensitivityValuesByPathFor(outputPath, parameterPath);
//      }
//
//      public void Clear()
//      {
//         _modelCoreSimulation = null;
//         _simModelSimulation = null;
//         _parameterValueCache.Clear();
//         _allVariableParameters.Clear();
//      }
//
//      private DataRepository getResults()
//      {
//         return _dataFactory.CreateRepository(_modelCoreSimulation, _simModelSimulation, _simulationResultsName);
//      }
//
//      private void updateParameterValues()
//      {
//         foreach (var variableParameter in _allVariableParameters)
//         {
//            variableParameter.Value = _parameterValueCache[variableParameter.Path];
//         }
//
//         _simModelSimulation.SetParameterValues(_allVariableParameters);
//      }
//   }
//}