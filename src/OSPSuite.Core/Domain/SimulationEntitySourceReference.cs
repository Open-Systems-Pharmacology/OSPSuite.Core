using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public class SimulationEntitySourceReference
   {
      public IObjectBase Source { get; }
      public IBuildingBlock BuildingBlock { get; }
      public Module Module { get; }

      public SimulationEntitySourceReference(IObjectBase source, IBuildingBlock buildingBlock, Module module)
      {
         Source = source;
         BuildingBlock = buildingBlock;
         Module = module;
      }
   }
}