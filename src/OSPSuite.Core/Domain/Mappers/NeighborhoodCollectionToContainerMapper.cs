using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   /// Maps collection of neighborhood builder objects.<para></para>
   /// Creates Top-Container named "NEIGHBORHOODS", all mapped neighborhoods <para></para>
   /// are added as childs of this top container
   /// </summary>
   public interface INeighborhoodCollectionToContainerMapper : IBuilderMapper<IModel, IContainer>
   {
   }

   public class NeighborhoodCollectionToContainerMapper : INeighborhoodCollectionToContainerMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly INeighborhoodBuilderToNeighborhoodMapper _neighborhoodMapper;
      private readonly IObjectPathFactory _objectPathFactory;

      public NeighborhoodCollectionToContainerMapper(IObjectBaseFactory objectBaseFactory,
                                                     INeighborhoodBuilderToNeighborhoodMapper neighborhoodMapper,
                                                     IObjectPathFactory objectPathFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _neighborhoodMapper = neighborhoodMapper;
         _objectPathFactory = objectPathFactory;
      }

      public IContainer MapFrom(IModel model, IBuildConfiguration buildConfiguration)
      {
         var moleculeNames = buildConfiguration.AllPresentFloatingMoleculeNames().ToList();

         var neighborhoodsParentContainer = _objectBaseFactory.Create<IContainer>()
            .WithMode(ContainerMode.Logical)
            .WithName(Constants.NEIGHBORHOODS);

         var startValuesForFloatingMolecules = presentMoleculesCachedByContainerPath(moleculeNames, buildConfiguration);

         var moleculeNamesCopyProperties = buildConfiguration.AllPresentXenobioticFloatingMoleculeNames().ToList();

         buildConfiguration.SpatialStructure.Neighborhoods.Each(nb =>
               neighborhoodsParentContainer.Add(_neighborhoodMapper.MapFrom(nb, 
                                                                                 model, 
                                                                                 buildConfiguration,
                                                                                 moleculeNamesFor(nb, startValuesForFloatingMolecules),
                                                                                 moleculeNamesCopyProperties)));

         return neighborhoodsParentContainer;
      }

      private ICache<string, IList<string>> presentMoleculesCachedByContainerPath(IEnumerable<string> namesOfFloatingMolecules, IBuildConfiguration buildConfiguration)
      {
         var startValues =
            buildConfiguration.MoleculeStartValues.Where(msv => (msv.IsPresent &&
                                                                 namesOfFloatingMolecules.Contains(msv.MoleculeName))).ToList();

         var moleculeStartValuesPerContainer = new Cache<string, IList<string>>();

         foreach (var msv in startValues)
         {
            IList<string> moleculeNames;
            var path = msv.ContainerPath.ToString();

            if (moleculeStartValuesPerContainer.Contains(path))
            {
               moleculeNames = moleculeStartValuesPerContainer[path];
            }
            else
            {
               moleculeNames=new List<string>();
               moleculeStartValuesPerContainer.Add(path, moleculeNames);
            }

            moleculeNames.Add(msv.MoleculeName);
         }

         return moleculeStartValuesPerContainer;
      }

      /// <summary>
      /// Returns molecules which will be created in both neighbours of the neighbourhood
      /// </summary>
      private IEnumerable<string> moleculeNamesFor(INeighborhoodBuilder neighborhoodBuilder,
                                                   ICache<string, IList<string>> moleculesStartValuesForFloatingMolecules)
      {
         var pathToFirstNeighbor = _objectPathFactory.CreateAbsoluteObjectPath(neighborhoodBuilder.FirstNeighbor).ToString();
         var pathToSecondNeighbor = _objectPathFactory.CreateAbsoluteObjectPath(neighborhoodBuilder.SecondNeighbor).ToString();

         // check if both neighbours has at least 1 molecule (if not - return empty list)
         if(!moleculesStartValuesForFloatingMolecules.Contains(pathToFirstNeighbor) || 
            !moleculesStartValuesForFloatingMolecules.Contains(pathToSecondNeighbor))
            return new List<string>();

         return moleculesStartValuesForFloatingMolecules[pathToFirstNeighbor]
                .Intersect(moleculesStartValuesForFloatingMolecules[pathToSecondNeighbor]).ToList();
      }
   }
}