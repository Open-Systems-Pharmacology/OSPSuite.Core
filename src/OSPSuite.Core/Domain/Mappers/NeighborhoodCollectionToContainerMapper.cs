using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Returns a result that includes a top container named "NEIGHBORHOODS" with all mapped neighborhoods,
   ///    a list of all invalid neighborhoods that could not be created because one or both neighbors were not found in the
   ///    simulation spatial structure, and a list of all neighborhoods that were removed because a module redefines them
   ///    without neighbors
   /// </summary>
   internal interface INeighborhoodCollectionToContainerMapper : IMapper<ModelConfiguration, NeighborhoodMapResult>
   {
   }

   internal class NeighborhoodCollectionToContainerMapper : INeighborhoodCollectionToContainerMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly INeighborhoodBuilderToNeighborhoodMapper _neighborhoodMapper;
      private readonly IContainerMergeTask _containerMergeTask;

      public NeighborhoodCollectionToContainerMapper(
         IObjectBaseFactory objectBaseFactory,
         INeighborhoodBuilderToNeighborhoodMapper neighborhoodMapper,
         IContainerMergeTask containerMergeTask)
      {
         _objectBaseFactory = objectBaseFactory;
         _neighborhoodMapper = neighborhoodMapper;
         _containerMergeTask = containerMergeTask;
      }

      public NeighborhoodMapResult MapFrom(ModelConfiguration modelConfiguration)
      {
         var (_, simulationBuilder) = modelConfiguration;

         var neighborhoodsParentContainer = _objectBaseFactory.Create<IContainer>()
            .WithMode(ContainerMode.Logical)
            .WithName(Constants.NEIGHBORHOODS);
         var neighborhoodMapResult = new NeighborhoodMapResult(neighborhoodsParentContainer);

         var allSpatialStructureAndMergeBehaviors = simulationBuilder.SpatialStructureAndMergeBehaviors;
         if (!allSpatialStructureAndMergeBehaviors.Any())
            return neighborhoodMapResult;

         var mapToNeighborhood = mapToNeighborhoodDef(modelConfiguration);

         //neighborhoods defined without neighbors represent a removal and are not mapped into the model
         IReadOnlyList<(NeighborhoodBuilder builder, Neighborhood neighborhood)> allNeighborHoodsFrom(SpatialStructure spatialStructure) =>
            spatialStructure.Neighborhoods.Where(x => !x.HasNoNeighbors).Select(x => (builder: x, neighborhood: mapToNeighborhood(x))).ToList();

         IReadOnlyList<Neighborhood> definedNeighborhoods(IReadOnlyList<(NeighborhoodBuilder builder, Neighborhood neighborhood)> allNeighborhoods) => 
            allNeighborhoods.Where(x => x.neighborhood != null).Select(x => x.neighborhood).ToList();
         
         IReadOnlyList<NeighborhoodBuilder> invalidNeighborhoods(IReadOnlyList<(NeighborhoodBuilder builder, Neighborhood neighborhood)> allNeighborhoods) => 
            allNeighborhoods.Where(x => x.neighborhood == null).Select(x => x.builder).ToList();
         
         IReadOnlyList<NeighborhoodBuilder> neighborhoodsWithoutNeighbors(SpatialStructure spatialStructure) => 
            spatialStructure.Neighborhoods.Where(x => x.HasNoNeighbors).ToList();

         //we use a cache to ensure that we are replacing neighborhoods defined in multiple structures
         var firstSpatialStructure = allSpatialStructureAndMergeBehaviors[0].spatialStructure;
         var allOtherSpatialStructuresWithMergeBehavior = allSpatialStructureAndMergeBehaviors.Skip(1).ToList();

         //first step: Add the neighborhoods from the first structure
         //a neighborhood without neighbors in the first structure has nothing to remove and is simply ignored
         var allNeighbors = allNeighborHoodsFrom(firstSpatialStructure);
         neighborhoodsParentContainer.AddChildren(definedNeighborhoods(allNeighbors));

         invalidNeighborhoods(allNeighbors).Each(x => neighborhoodMapResult.AddInvalid(x, firstSpatialStructure));

         //now merge all other neighborhoods
         allOtherSpatialStructuresWithMergeBehavior
            .Select(x => new { x.mergeBehavior, neighborhoods = allNeighborHoodsFrom(x.spatialStructure), x.spatialStructure })
            .Each(x =>
            {
               mergeNeighborhoodsInStructure(neighborhoodsParentContainer, definedNeighborhoods(x.neighborhoods), x.mergeBehavior);
               invalidNeighborhoods(x.neighborhoods).Each(n => neighborhoodMapResult.AddInvalid(n, x.spatialStructure));
               neighborhoodsWithoutNeighbors(x.spatialStructure).Each(n => removeNeighborhood(neighborhoodsParentContainer, n, x.spatialStructure, neighborhoodMapResult));
            });

         return neighborhoodMapResult;
      }

      /// <summary>
      ///    Removes the neighborhood named after <paramref name="neighborhoodWithoutNeighbors" /> from
      ///    <paramref name="neighborhoods" /> if it was created by a previously merged spatial structure. Redefining a
      ///    neighborhood without neighbors is the only way for a module to remove a neighborhood defined in another module.
      ///    This applies whatever the merge behavior of the module.
      /// </summary>
      private void removeNeighborhood(IContainer neighborhoods, NeighborhoodBuilder neighborhoodWithoutNeighbors, SpatialStructure spatialStructure, NeighborhoodMapResult neighborhoodMapResult)
      {
         var existingNeighborhood = neighborhoods.GetSingleChildByName(neighborhoodWithoutNeighbors.Name);
         if (existingNeighborhood == null)
            return;

         neighborhoods.RemoveChild(existingNeighborhood);
         neighborhoodMapResult.AddRemoved(neighborhoodWithoutNeighbors, spatialStructure);
      }

      private void mergeNeighborhoodsInStructure(IContainer neighborhoods, IReadOnlyList<Neighborhood> neighborhoodsToMerge, MergeBehavior mergeBehavior)
      {
         neighborhoodsToMerge.Each(neighborhoodToMerge =>
         {
            if (mergeBehavior == MergeBehavior.Extend)
            {
               var mergeNeighbor = _containerMergeTask.AddOrMergeContainer(neighborhoods, neighborhoodToMerge) as Neighborhood;
               updateNeighbors(mergeNeighbor, neighborhoodToMerge);
            }
            else
               _containerMergeTask.AddOrReplaceInContainer(neighborhoods, neighborhoodToMerge);
         });
      }

      private void updateNeighbors(Neighborhood neighborhoodToUpdate, Neighborhood neighborhoodToMerge)
      {
         if (neighborhoodToUpdate == null)
            return;

         neighborhoodToUpdate.FirstNeighbor = neighborhoodToMerge.FirstNeighbor;
         neighborhoodToUpdate.SecondNeighbor = neighborhoodToMerge.SecondNeighbor;
      }

      private Func<NeighborhoodBuilder, Neighborhood> mapToNeighborhoodDef(ModelConfiguration modelConfiguration)
      {
         var (_, simulationBuilder) = modelConfiguration;
         var moleculeNames = simulationBuilder.AllPresentFloatingMoleculeNames();
         var startValuesForFloatingMolecules = presentMoleculesCachedByContainerPath(moleculeNames, simulationBuilder);
         var moleculeNamesCopyProperties = simulationBuilder.AllPresentXenobioticFloatingMoleculeNames();

         return neighborhoodBuilder => _neighborhoodMapper.MapFrom(neighborhoodBuilder, moleculeNamesFor(neighborhoodBuilder, startValuesForFloatingMolecules), moleculeNamesCopyProperties, modelConfiguration);
      }

      private ICache<string, List<string>> presentMoleculesCachedByContainerPath(IEnumerable<string> namesOfFloatingMolecules, SimulationBuilder simulationBuilder)
      {
         var initialConditions = simulationBuilder.AllPresentMoleculeValuesFor(namesOfFloatingMolecules).ToList();

         var initialConditionsPerContainer = new Cache<string, List<string>>();

         foreach (var initialCondition in initialConditions)
         {
            List<string> moleculeNames;
            var path = initialCondition.ContainerPath.ToString();

            if (initialConditionsPerContainer.Contains(path))
            {
               moleculeNames = initialConditionsPerContainer[path];
            }
            else
            {
               moleculeNames = new List<string>();
               initialConditionsPerContainer.Add(path, moleculeNames);
            }

            moleculeNames.Add(initialCondition.MoleculeName);
         }

         return initialConditionsPerContainer;
      }

      /// <summary>
      ///    Returns molecules which will be created in both neighbors of the neighborhood
      /// </summary>
      private IReadOnlyList<string> moleculeNamesFor(NeighborhoodBuilder neighborhoodBuilder, ICache<string, List<string>> moleculesStartValuesForFloatingMolecules)
      {
         // undefined neighbor paths cannot contain any molecule
         if (!neighborhoodBuilder.HasDefinedNeighborPaths)
            return new List<string>();

         var pathToFirstNeighbor = neighborhoodBuilder.FirstNeighborPath.PathAsString;
         var pathToSecondNeighbor = neighborhoodBuilder.SecondNeighborPath.PathAsString;

         // check if both neighbors has at least 1 molecule (if not - return empty list)
         if (!moleculesStartValuesForFloatingMolecules.Contains(pathToFirstNeighbor) ||
             !moleculesStartValuesForFloatingMolecules.Contains(pathToSecondNeighbor))
            return new List<string>();

         return moleculesStartValuesForFloatingMolecules[pathToFirstNeighbor]
            .Intersect(moleculesStartValuesForFloatingMolecules[pathToSecondNeighbor]).ToList();
      }
   }

   internal class NeighborhoodMapResult
   {
      private readonly List<(NeighborhoodBuilder builder, SpatialStructure buildingBlock)> _invalidNeighborhoods;
      private readonly List<(NeighborhoodBuilder builder, SpatialStructure buildingBlock)> _removedNeighborhoods;
      private readonly IContainer _container;

      public NeighborhoodMapResult(IContainer container)
      {
         _container = container;
         _invalidNeighborhoods = new List<(NeighborhoodBuilder builder, SpatialStructure buildingBlock)>();
         _removedNeighborhoods = new List<(NeighborhoodBuilder builder, SpatialStructure buildingBlock)>();
      }

      /// <summary>
      ///    Adds the <paramref name="builder" /> and the source <paramref name="buildingBlock" /> to the list of invalid
      ///    neighborhoods (at least one neighbor could not be found in the simulation)
      /// </summary>
      public void AddInvalid(NeighborhoodBuilder builder, SpatialStructure buildingBlock)
      {
         _invalidNeighborhoods.Add((builder, buildingBlock));
      }

      /// <summary>
      ///    Adds the <paramref name="builder" /> and the source <paramref name="buildingBlock" /> to the list of removed
      ///    neighborhoods (the neighborhood was removed from the simulation because <paramref name="builder" /> defines no
      ///    neighbors)
      /// </summary>
      public void AddRemoved(NeighborhoodBuilder builder, SpatialStructure buildingBlock)
      {
         _removedNeighborhoods.Add((builder, buildingBlock));
      }

      internal void Deconstruct(
         out IContainer container,
         out IReadOnlyList<(NeighborhoodBuilder builder, SpatialStructure buildingBlock)> invalidNeighborhoods,
         out IReadOnlyList<(NeighborhoodBuilder builder, SpatialStructure buildingBlock)> removedNeighborhoods)
      {
         container = _container;
         invalidNeighborhoods = _invalidNeighborhoods;
         removedNeighborhoods = _removedNeighborhoods;
      }
   }
}