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
   ///    Returns a result that includes a top container named "NEIGHBORHOODS" with all mapped neighborhoods
   ///    and a list of all neighborhoods that were not included because one or both neighbors were not found in the
   ///    simulation spatial structure
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

         IReadOnlyList<(NeighborhoodBuilder builder, Neighborhood neighborhood)> allNeighborHoodsFrom(SpatialStructure spatialStructure) =>
            spatialStructure.Neighborhoods.Select(x => (builder: x, neighborhood: mapToNeighborhood(x))).ToList();

         IReadOnlyList<Neighborhood> definedNeighborhoods(IReadOnlyList<(NeighborhoodBuilder builder, Neighborhood neighborhood)> allNeighborhoods) => allNeighborhoods.Where(x => x.neighborhood != null).Select(x => x.neighborhood).ToList();
         IReadOnlyList<NeighborhoodBuilder> notDefinedNeighborhoods(IReadOnlyList<(NeighborhoodBuilder builder, Neighborhood neighborhood)> allNeighborhoods) => allNeighborhoods.Where(x => x.neighborhood == null).Select(x => x.builder).ToList();

         //we use a cache to ensure that we are replacing neighborhoods defined in multiple structures
         var firstSpatialStructure = allSpatialStructureAndMergeBehaviors[0].spatialStructure;
         var allOtherSpatialStructuresWithMergeBehavior = allSpatialStructureAndMergeBehaviors.Skip(1).ToList();

         //first step: Add the neighborhoods from the first structure
         var allNeighbors = allNeighborHoodsFrom(firstSpatialStructure);
         neighborhoodsParentContainer.AddChildren(definedNeighborhoods(allNeighbors));

         notDefinedNeighborhoods(allNeighbors).Each(x => neighborhoodMapResult.Add(x, firstSpatialStructure));

         //now merge all other neighborhoods
         allOtherSpatialStructuresWithMergeBehavior
            .Select(x => new { x.mergeBehavior, neighborhoods = allNeighborHoodsFrom(x.spatialStructure), x.spatialStructure })
            .Each(x =>
            {
               mergeNeighborhoodsInStructure(neighborhoodsParentContainer, definedNeighborhoods(x.neighborhoods), x.mergeBehavior);
               notDefinedNeighborhoods(x.neighborhoods).Each(n => neighborhoodMapResult.Add(n, x.spatialStructure));
            });

         return neighborhoodMapResult;
      }

      private void mergeNeighborhoodsInStructure(IContainer neighborhoods, IReadOnlyList<Neighborhood> neighborhoodsToMerge, MergeBehavior mergeBehavior)
      {
         neighborhoodsToMerge.Each(neighborhoodToMerge =>
         {
            if (mergeBehavior == MergeBehavior.Extend)
               _containerMergeTask.AddOrMergeContainer(neighborhoods, neighborhoodToMerge);
            else
               _containerMergeTask.AddOrReplaceInContainer(neighborhoods, neighborhoodToMerge);

            updateNeighbors(neighborhoods, neighborhoodToMerge);
         });
      }

      private void updateNeighbors(IContainer neighborhoods, Neighborhood neighborhoodToMerge)
      {
         var neighborhoodToUpdate = neighborhoods.GetAllChildren<Neighborhood>().FirstOrDefault(x => string.Equals(x.Name, neighborhoodToMerge.Name));
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
      private readonly List<(NeighborhoodBuilder builder, SpatialStructure buildingBlock)> _ignoredNeighborhoods;
      private readonly IContainer _container;

      public NeighborhoodMapResult(IContainer container)
      {
         _container = container;
         _ignoredNeighborhoods = new List<(NeighborhoodBuilder builder, SpatialStructure buildingBlock)>();
      }

      /// <summary>
      ///    Adds the ignored <paramref name="builder" /> and the source <paramref name="buildingBlock" /> to the list of ignored
      ///    neighborhoods
      /// </summary>
      public void Add(NeighborhoodBuilder builder, SpatialStructure buildingBlock)
      {
         _ignoredNeighborhoods.Add((builder, buildingBlock));
      }

      internal void Deconstruct(out IContainer container, out IReadOnlyList<(NeighborhoodBuilder builder, SpatialStructure buildingBlock)> ignoredNeighborhoods)
      {
         container = _container;
         ignoredNeighborhoods = _ignoredNeighborhoods;
      }
   }
}