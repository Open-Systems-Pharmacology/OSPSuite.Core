using System.Collections.Generic;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IResidualCalculator
   {
      void Initialize(RemoveLLOQMode removeLLOQMode);
      ResidualsResult Calculate(IReadOnlyList<SimulationRunResults> simulationsResults, IReadOnlyList<OutputMapping> allOutputMappings);
   }
}