using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;

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

      public NeighborhoodCollectionToContainerMapper(
         IObjectBaseFactory objectBaseFactory,
         INeighborhoodBuilderToNeighborhoodMapper neighborhoodMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _neighborhoodMapper = neighborhoodMapper;
      }

      public IContainer MapFrom(ModelConfiguration modelConfiguration)
      {
         var (_, simulationBuilder) = modelConfiguration;
         var moleculeNames = simulationBuilder.AllPresentFloatingMoleculeNames();

         var neighborhoodsParentContainer = _objectBaseFactory.Create<IContainer>()
            .WithMode(ContainerMode.Logical)
            .WithName(Constants.NEIGHBORHOODS);

         var startValuesForFloatingMolecules = presentMoleculesCachedByContainerPath(moleculeNames, simulationBuilder);

         var moleculeNamesCopyProperties = simulationBuilder.AllPresentXenobioticFloatingMoleculeNames();

         //we use a cache to ensure that we are replacing neighborhoods defined in multiple structures
         var neighborhoodCache = new ObjectBaseCache<Neighborhood>();
         var allNeighborhoodBuilder = simulationBuilder.SpatialStructureAndMergeBehaviors.SelectMany(x => x.spatialStructure.Neighborhoods);
         neighborhoodCache.AddRange(allNeighborhoodBuilder.Select(nb =>
               _neighborhoodMapper.MapFrom(nb, moleculeNamesFor(nb, startValuesForFloatingMolecules), moleculeNamesCopyProperties, modelConfiguration))
            //can be null if the neighbors are not defined in the merged spatial structure
            .Where(x => x != null));

         neighborhoodsParentContainer.AddChildren(neighborhoodCache);
         return neighborhoodsParentContainer;
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
      private IEnumerable<string> moleculeNamesFor(NeighborhoodBuilder neighborhoodBuilder, ICache<string, List<string>> moleculesStartValuesForFloatingMolecules)
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