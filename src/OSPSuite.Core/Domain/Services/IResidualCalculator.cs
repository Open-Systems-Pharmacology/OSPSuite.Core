using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services
{
   public interface IResidualCalculator
   {
      void Initialize(RemoveLLOQMode removeLLOQMode);
      ResidualsResult Calculate(IReadOnlyList<SimulationRunResults> simulationsResults, IReadOnlyList<OutputMapping> allOutputMappings);
      ResidualsResult Calculate(DataRepository simulationResultRepository, IReadOnlyList<OutputMapping> allOutputMappings);
   }
}