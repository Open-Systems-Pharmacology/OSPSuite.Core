using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain.Services
{
   internal interface ISpatialStructureMerger
   {
      IContainer MergeContainerStructure(ModelConfiguration modelConfiguration);
      IContainer MergeNeighborhoods(ModelConfiguration modelConfiguration);
   }

   internal class SpatialStructureMerger : ISpatialStructureMerger
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IContainerBuilderToContainerMapper _containerMapper;
      private readonly INeighborhoodCollectionToContainerMapper _neighborhoodsMapper;

      public SpatialStructureMerger(
         IObjectBaseFactory objectBaseFactory,
         IContainerBuilderToContainerMapper containerMapper,
         INeighborhoodCollectionToContainerMapper neighborhoodsMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _containerMapper = containerMapper;
         _neighborhoodsMapper = neighborhoodsMapper;
      }

      public IContainer MergeContainerStructure(ModelConfiguration modelConfiguration)
      {
         var (model, simulationConfiguration) = modelConfiguration;
         // Create Root Container for the model with the name of the model
         var root = _objectBaseFactory.Create<IContainer>()
            .WithName(model.Name)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Simulation);

         root.AddTag(new Tag(Constants.ROOT_CONTAINER_TAG));

         createMergedContainerStructureInRoot(root, simulationConfiguration);

         return root;
      }

      private void createMergedContainerStructureInRoot(IContainer root, SimulationBuilder simulationConfiguration)
      {
         var allSpatialStructures = simulationConfiguration.SpatialStructures;
         if (!allSpatialStructures.Any())
            return;

         var firstSpatialStructure = allSpatialStructures[0];
         var allOtherSpatialStructures = allSpatialStructures.Skip(1).ToList();

         // First step: We create the container structure.
         // This is done by adding all top containers defined in the FIRST spatial structure
         // Then we are going to merge all other top container defined in the other spatial structures
         //Add each container defined in the spatial structure and direct child of the root container
         foreach (var topContainer in firstSpatialStructure.TopContainers)
         {
            root.Add(_containerMapper.MapFrom(topContainer, simulationConfiguration));
         }

         //Merge all other spatial structures
         foreach (var topContainer in allOtherSpatialStructures.SelectMany(x => x.TopContainers))
         {
            mergeTopContainerInStructure(topContainer, root, simulationConfiguration);
         }
      }

      private void mergeTopContainerInStructure(IContainer topContainer, IContainer root, SimulationBuilder simulationConfiguration)
      {
         //probably should never happen
         if (topContainer == null)
            return;

         //In this case, we add or replace the top container
         if (topContainer.ParentPath == null)
            addOrReplaceContainer(topContainer, root);
         else
            insertTopContainerIntoStructure(topContainer, root, simulationConfiguration);
      }

      private void insertTopContainerIntoStructure(IContainer topContainer, IContainer root, SimulationBuilder simulationConfiguration)
      {
         var parentContainer = topContainer.ParentPath.Resolve<IContainer>(root);
         if (parentContainer == null)
            throw new OSPSuiteException(Error.CannotFindParentContainerWithPath(topContainer.ParentPath.PathAsString, topContainer.Name));

         addOrReplaceContainer(topContainer, parentContainer);
      }

      private static void addOrReplaceContainer(IContainer containerToAdd, IContainer parentContainer)
      {
         var existingContainer = parentContainer.Container(containerToAdd.Name);
         if (existingContainer != null)
            parentContainer.RemoveChild(existingContainer);

         //add in any case
         parentContainer.Add(containerToAdd);
      }

      public IContainer MergeNeighborhoods(ModelConfiguration modelConfiguration)
      {
         return _neighborhoodsMapper.MapFrom(modelConfiguration);
      }
   }
}