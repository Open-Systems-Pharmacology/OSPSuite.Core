using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public interface ISimulation : ILazyLoadable, IAnalysable, IUsesObservedData, IModelCoreSimulation, IWithHasChanged
   {
      IEnumerable<CurveChart> Charts { get; }

      OutputMappings OutputMappings { get; set; }

      //ResultsDataRepository will be null for Population Simulations and should never be called for them
      DataRepository ResultsDataRepository { get; set; }
   }
}