using System.Collections.Generic;

namespace OSPSuite.Core.Domain
{
   public interface ICoreSimulationFactory
   {
      ISimulation CreateWithCalculationMethodsFrom(ISimulation templateSimulation, IEnumerable<CalculationMethodWithCompoundName> combination);
   }
}
