using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public class SimulationEntitySourceReference
   {
      /// <summary>
      ///    Reference to the entity in the project (being seen as the source of the entity in the simulation)
      /// </summary>
      public IEntity Source { get; }

      public IBuildingBlock BuildingBlock { get; }
      public Module Module { get; }

      /// <summary>
      ///    Reference to the actual entity in the simulation (e.g. parameter, container, etc.)
      /// </summary>
      public IEntity SimulationEntity { get; }

      public SimulationEntitySourceReference(IEntity source, IBuildingBlock buildingBlock, Module module, IEntity simulationEntity)
      {
         Source = source;
         BuildingBlock = buildingBlock;
         Module = module;
         SimulationEntity = simulationEntity;
      }
   }

   public class SimulationEntitySourceReferenceCache : Cache<IEntity, SimulationEntitySourceReference>
   {
      public SimulationEntitySourceReferenceCache() : base(onMissingKey: x => null, getKey: x => x.SimulationEntity)
      {
      }
   }
}