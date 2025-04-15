using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public interface ISimulationEntitySourceReferenceFactory
   {
      IReadOnlyList<SimulationEntitySourceReference> CreateFor(IEnumerable<SimulationEntitySource> entitySources);
   }

   public class SimulationEntitySourceReferenceFactory : ISimulationEntitySourceReferenceFactory
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IContainerTask _containerTask;

      public SimulationEntitySourceReferenceFactory(IBuildingBlockRepository buildingBlockRepository, IContainerTask containerTask)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _containerTask = containerTask;
      }

      public IReadOnlyList<SimulationEntitySourceReference> CreateFor(IEnumerable<SimulationEntitySource> entitySources)
      {
         //cache all building blocks by module, type and name for each retrieval
         var cache = new Cache<string, IBuildingBlock>(onMissingKey: x => null);
         _buildingBlockRepository.All().Each(bb => cache[uniqueKeyFor(bb)] = bb);
         var spStrCache = createSpatialStructureCache();
         var getEntityByPathIn = getEntityByPathInDef(spStrCache);
         return entitySources.Select(entitySource =>
         {
            var buildingBlock = cache[uniqueKeyFor(entitySource)];
            //we return null if the building block is not found to keep the same cardinality
            if (buildingBlock == null)
               return null;

            var entity = getEntityByPathIn(entitySource, buildingBlock);
            if (entity == null)
               return null;

            return new SimulationEntitySourceReference(entity, buildingBlock, buildingBlock.Module);
         }).ToList();
      }

      private Cache<SpatialStructure, PathCache<IEntity>> createSpatialStructureCache()
      {
         var cache = new Cache<SpatialStructure, PathCache<IEntity>>();
         _buildingBlockRepository.All<SpatialStructure>().Each(x => { cache[x] = _containerTask.PathCacheFor(x.SelectMany(topContainer => topContainer.GetAllChildrenAndSelf<IEntity>())); });
         return cache;
      }

      private Func<SimulationEntitySource, IBuildingBlock, IObjectBase> getEntityByPathInDef(Cache<SpatialStructure, PathCache<IEntity>> spStrCache) =>
         (entitySource, buildingBlock) =>
         {
            var sourcePath = entitySource.SourcePath;
            switch (buildingBlock)
            {
               case InitialConditionsBuildingBlock initialConditionsBuilding:
                  return initialConditionsBuilding.FindByPath(sourcePath);
               case ParameterValuesBuildingBlock parameterValuesBuildingBlock:
                  return parameterValuesBuildingBlock.FindByPath(sourcePath);
               case ExpressionProfileBuildingBlock expressionProfileBuildingBlock:
                  return expressionProfileBuildingBlock.FindByPath(sourcePath);
               case IndividualBuildingBlock individualBuildingBlock:
                  return individualBuildingBlock.FindByPath(sourcePath);
               case SpatialStructure spatialStructure:
                  var pathCache = spStrCache[spatialStructure];
                  return pathCache[sourcePath];
               case IBuildingBlock<IBuilder> bb:
                  return bb.FindByName(sourcePath);
               default:
                  return null;
            }
         };

      private string uniqueKeyFor(SimulationEntitySource entitySource)
      {
         return uniqueKeyFor(entitySource.BuildingBlockType, entitySource.BuildingBlockName, entitySource.ModuleName);
      }

      private string uniqueKeyFor(IBuildingBlock buildingBlock)
      {
         var type = buildingBlock.GetType().Name;
         return uniqueKeyFor(type, buildingBlock.Name, buildingBlock.Module?.Name);
      }

      private string uniqueKeyFor(string type, string buildingBlockName, string moduleName) => $"{type}-{buildingBlockName}-{moduleName ?? ""}";
   }
}