using System;
using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimModelBatch : IDisposable
   {
      void InitializeWith(IModelCoreSimulation modelCoreSimulation, IReadOnlyList<string> variableParameterPaths,
         IReadOnlyList<string> variableMoleculePaths, bool calculateSensitivities = false, string simulationResultsName = null);

      void InitializeForSensitivity();

      SimulationRunResults RunSimulation();

      void UpdateParameterValue(string parameterPath, double value);
      void UpdateParameterValues(IReadOnlyList<double> parameterValues);

      void UpdateInitialValue(string moleculePath, double value);
      void UpdateInitialValues(IReadOnlyList<double> initialValues);

      void StopSimulation();
      double[] SensitivityValuesFor(string outputPath, string parameterPath);

      /// <summary>
      ///    Path of parameters that will be effectively varied. (we can assume here that all parameters effectively exist in the
      ///    simulation)
      /// </summary>
      IReadOnlyList<string> VariableParameterPaths { get; }

      /// <summary>
      ///    Path of molecules that will be effectively varied. (we can assume here that all molecules effectively exist in the
      ///    simulation)
      /// </summary>
      IReadOnlyList<string> VariableMoleculePaths { get; }
   }
}