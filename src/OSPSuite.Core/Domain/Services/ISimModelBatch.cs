using System;
using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimModelBatch : IDisposable
   {
      void InitializeWith(IModelCoreSimulation modelCoreSimulation, IReadOnlyList<string> variableParameterPaths,
         IReadOnlyList<string> variableSpeciePaths, bool calculateSensitivities = false, string simulationResultsName = null);

      void InitializeForSensitivity();
 
      SimulationRunResults RunSimulation();
      
      void UpdateParameterValue(string parameterPath, double value);
      void UpdateInitialValue(string speciesPath, double value);

      void StopSimulation();
      double[] SensitivityValuesFor(string outputPath, string parameterPath);
   }
}