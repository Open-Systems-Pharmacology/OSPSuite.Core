using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public interface ISimulation : ILazyLoadable, IAnalysable, IUsesObservedData, IModelCoreSimulation
   {
      IEnumerable<CurveChart> Charts { get; }

      OutputMappings AllOutputMappings { get; set; }
   }
}