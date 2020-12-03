using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.R.MinimalImplementations
{
   public class RQuantityPathToQuantityDisplayPathMapper : QuantityPathToQuantityDisplayPathMapper
   {
      public RQuantityPathToQuantityDisplayPathMapper(IObjectPathFactory objectPathFactory, IPathToPathElementsMapper pathToPathElementsMapper,
         IDataColumnToPathElementsMapper dataColumnToPathElementsMapper) : base(objectPathFactory, pathToPathElementsMapper,
         dataColumnToPathElementsMapper)
      {
      }

      protected override IEnumerable<PathElementId> DefaultPathElementsToUse(bool addSimulationName, PathElements pathElements)
      {
         if (addSimulationName)
            yield return PathElementId.Simulation;

         yield return PathElementId.Molecule;

         yield return PathElementId.Container;
         yield return PathElementId.BottomCompartment;
         yield return PathElementId.Name;
      }
   }
}