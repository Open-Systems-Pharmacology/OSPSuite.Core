using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public interface ISimulationEntitySourceReferenceFactory
   {
      /// <summary>
      ///    Returns the simulation entity source reference cache for the given simulation and entity sources. If entity sources
      ///    are not provided, all entity sources in the simulation will be used.
      /// </summary>
      SimulationEntitySourceReferenceCache CreateFor(IModelCoreSimulation simulation, IEnumerable<SimulationEntitySource> entitySources = null);
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

      public SimulationEntitySourceReferenceCache CreateFor(IModelCoreSimulation simulation, IEnumerable<SimulationEntitySource> entitySources = null)
      {
         //cache all building blocks by module, type and name for each retrieval
         var buildingBlockCache = new Cache<string, IBuildingBlock>(onMissingKey: x => null);
         var entitySourcesToUse = entitySources ?? simulation.EntitySources;
         var simulationEntityCache = _containerTask.CacheAllChildren<IEntity>(simulation.Model.Root);
         _buildingBlockRepository.All().Each(bb => buildingBlockCache[uniqueKeyFor(bb)] = bb);
         var spatialStructureCache = createSpatialStructureCache();
         var getEntityByPathIn = getEntityByPathInDef(spatialStructureCache);
         var sourceReferenceCache = new SimulationEntitySourceReferenceCache();

         entitySourcesToUse.Each(entitySource =>
         {
            var entity = simulationEntityCache[entitySource.SimulationEntityPath];
            if (entity == null)
               return;

            var buildingBlock = buildingBlockCache[uniqueKeyFor(entitySource)];
            //we return null if the building block is not found to keep the same cardinality
            if (buildingBlock == null)
               return;

            var source = getEntityByPathIn(entitySource, buildingBlock);
            if (source == null)
               return;

            sourceReferenceCache.Add(new SimulationEntitySourceReference(source, buildingBlock, buildingBlock.Module, entity));
         });

         return sourceReferenceCache;
      }

      private Cache<SpatialStructure, PathCache<IEntity>> createSpatialStructureCache()
      {
         var cache = new Cache<SpatialStructure, PathCache<IEntity>>();
         _buildingBlockRepository.All<SpatialStructure>().Each(x => { cache[x] = _containerTask.PathCacheFor(x.SelectMany(topContainer => topContainer.GetAllChildrenAndSelf<IEntity>())); });
         return cache;
      }

      private Func<SimulationEntitySource, IBuildingBlock, IEntity> getEntityByPathInDef(Cache<SpatialStructure, PathCache<IEntity>> spStrCache) =>
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
               case IEnumerable<IBuilder> bb:
               {
                  //The path could potentially be the name of the source or a parameter in the source
                  var sourcePathArray = sourcePath.ToObjectPath();
                  var container = bb.FindByName(sourcePathArray[0]);
                  if (sourcePathArray.Count == 1)
                     return container;

                  sourcePathArray.RemoveFirst();
                  return sourcePathArray.TryResolve<IEntity>(container);
               }
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