using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IQuantityPathToQuantityDisplayPathMapper
   {
      string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, bool addSimulationName = false);
      string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, string simulationName);
      string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, IEnumerable<PathElement> pathElementsToUse);
      string DisplayPathAsStringFor(IQuantity quantity, bool addSimulationName = false);
      string DisplayPathAsStringFor(IQuantity quantity, IEnumerable<PathElement> pathElementsToUse);
   }
}