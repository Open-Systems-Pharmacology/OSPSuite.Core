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
   ///    Maps collection of neighborhood builder objects.
   ///    <para></para>
   ///    Creates Top-Container named "NEIGHBORHOODS", all mapped neighborhoods
   ///    <para></para>
   ///    are added as children of this top container
   /// </summary>
   internal interface INeighborhoodCollectionToContainerMapper : IMapper<ModelConfiguration, IContainer>
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

      public IContainer MapFrom(ModelConfiguration modelConfiguration)
      {
         var (_, simulationBuilder) = modelConfiguration;

         var neighborhoodsParentContainer = _objectBaseFactory.Create<IContainer>()
            .WithMode(ContainerMode.Logical)
            .WithName(Constants.NEIGHBORHOODS);

         var allSpatialStructureAndMergeBehaviors = simulationBuilder.SpatialStructureAndMergeBehaviors;
         if (!allSpatialStructureAndMergeBehaviors.Any())
            return neighborhoodsParentContainer;

         var mapToNeighborhood = mapToNeighborhoodDef(modelConfiguration);

         IReadOnlyList<Neighborhood> mapNeighborhoods(SpatialStructure spatialStructure) =>
            spatialStructure.Neighborhoods.Select(mapToNeighborhood).Where(x => x != null).ToList();

         //we use a cache to ensure that we are replacing neighborhoods defined in multiple structures
         var firstSpatialStructure = allSpatialStructureAndMergeBehaviors[0].spatialStructure;
         var allOtherSpatialStructuresWithMergeBehavior = allSpatialStructureAndMergeBehaviors.Skip(1).ToList();


         //first step: Add the neighborhoods from the first structure
         neighborhoodsParentContainer.AddChildren(mapNeighborhoods(firstSpatialStructure));

         //now merge all other neighborhoods
         allOtherSpatialStructuresWithMergeBehavior
            .Select(x => new {x.mergeBehavior, neighborhoods = mapNeighborhoods(x.spatialStructure)})
            .Each(x => mergeNeighborhoodsInStructure(neighborhoodsParentContainer, x.neighborhoods, x.mergeBehavior));

         return neighborhoodsParentContainer;
      }

      private void mergeNeighborhoodsInStructure(IContainer neighborhoods, IReadOnlyList<Neighborhood> neighborhoodsToMerge, MergeBehavior mergeBehavior)
      {
         neighborhoodsToMerge.Each(neighborhoodToMerge =>
         {
            if (mergeBehavior == MergeBehavior.Extend)
               _containerMergeTask.AddOrMergeContainer(neighborhoods, neighborhoodToMerge);
            else
               _containerMergeTask.AddOrReplaceInContainer(neighborhoods, neighborhoodToMerge);
         });
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
}