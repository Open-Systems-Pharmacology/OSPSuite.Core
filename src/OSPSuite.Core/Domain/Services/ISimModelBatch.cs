using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimModelBatch
   {
      void InitializeWith(IModelCoreSimulation modelCoreSimulation, IReadOnlyList<string> variableParameterPaths, bool calculateSensitivities = false, string simulationResultsName = null);
      void InitializeForSensitivity();
      SimulationRunResults RunSimulation();
      void UpdateParameterValue(string path, double value);
      void StopSimulation();
      double[] SensitivityValuesFor(string outputPath, string parameterPath);
      void Clear();
   }
}