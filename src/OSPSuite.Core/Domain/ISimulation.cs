using System.Collections.Generic;
using OSPSuite.Core.Chart;

namespace OSPSuite.Core.Domain
{
   public interface ISimulation : ILazyLoadable, IAnalysable, IUsesObservedData, IModelCoreSimulation
   {
      IEnumerable<CurveChart> Charts { get; }

      /// <summary>
      ///    Name of all compounds used in the simulation
      /// </summary>
      IReadOnlyList<string> CompoundNames { get; }

      IEnumerable<T> All<T>() where T : class, IEntity;
   }
}