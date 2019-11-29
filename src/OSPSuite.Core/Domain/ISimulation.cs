using System.Collections.Generic;
using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Domain
{
   public interface ISimulation : ILazyLoadable, IAnalysable, IUsesObservedData, IModelCoreSimulation
   {
      IEnumerable<CurveChart> Charts { get; }
   }
}