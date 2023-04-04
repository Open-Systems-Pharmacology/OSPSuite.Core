using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
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
   public interface INeighborhoodCollectionToContainerMapper : IMapper<ModelConfiguration, IContainer>
   {
   }

   public class NeighborhoodCollectionToContainerMapper : INeighborhoodCollectionToContainerMapper
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
         var (_, simulationConfiguration) = modelConfiguration;
         var moleculeNames = simulationConfiguration.AllPresentFloatingMoleculeNames();

         var neighborhoodsParentContainer = _objectBaseFactory.Create<IContainer>()
            .WithMode(ContainerMode.Logical)
            .WithName(Constants.NEIGHBORHOODS);

         var startValuesForFloatingMolecules = presentMoleculesCachedByContainerPath(moleculeNames, simulationConfiguration);

         var moleculeNamesCopyProperties = simulationConfiguration.AllPresentXenobioticFloatingMoleculeNames();

         simulationConfiguration.SpatialStructures.SelectMany(x => x.Neighborhoods).Each(nb =>
            neighborhoodsParentContainer.Add(_neighborhoodMapper.MapFrom(nb,
               moleculeNamesFor(nb, startValuesForFloatingMolecules),
               moleculeNamesCopyProperties, modelConfiguration)));

         return neighborhoodsParentContainer;
      }

      private ICache<string, List<string>> presentMoleculesCachedByContainerPath(IEnumerable<string> namesOfFloatingMolecules, SimulationConfiguration simulationConfiguration)
      {
         var startValues = simulationConfiguration.AllPresentMoleculeValuesFor(namesOfFloatingMolecules).ToList();

         var moleculeStartValuesPerContainer = new Cache<string, List<string>>();

         foreach (var msv in startValues)
         {
            List<string> moleculeNames;
            var path = msv.ContainerPath.ToString();

            if (moleculeStartValuesPerContainer.Contains(path))
            {
               moleculeNames = moleculeStartValuesPerContainer[path];
            }
            else
            {
               moleculeNames = new List<string>();
               moleculeStartValuesPerContainer.Add(path, moleculeNames);
            }

            moleculeNames.Add(msv.MoleculeName);
         }

         return moleculeStartValuesPerContainer;
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