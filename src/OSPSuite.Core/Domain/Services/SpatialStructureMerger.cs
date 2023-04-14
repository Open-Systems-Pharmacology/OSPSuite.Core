using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

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
         var (model, simulationBuilder) = modelConfiguration;
         // Create Root Container for the model with the name of the model
         var root = _objectBaseFactory.Create<IContainer>()
            .WithName(model.Name)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Simulation);

         root.AddTag(new Tag(Constants.ROOT_CONTAINER_TAG));

         createMergedContainerStructureInRoot(root, simulationBuilder);

         return root;
      }

      private void createMergedContainerStructureInRoot(IContainer root, SimulationBuilder simulationBuilder)
      {
         var allSpatialStructures = simulationBuilder.SpatialStructures;
         if (!allSpatialStructures.Any())
            return;

         var firstSpatialStructure = allSpatialStructures[0];
         var allOtherSpatialStructures = allSpatialStructures.Skip(1).ToList();
         var mapTopContainerToModelContainer = mapContainer(simulationBuilder);
         var mergeTopContainerIntoModel = mergeTopContainerInStructure(root);

         // First step: We create the container structure.
         // This is done by adding all top containers defined in the FIRST spatial structure
         // Then we are going to merge all other top container defined in the other spatial structures
         //Add each container defined in the spatial structure and direct child of the root container
         root.AddChildren(firstSpatialStructure.TopContainers.Select(mapTopContainerToModelContainer));

         //Merge all other spatial structures
         allOtherSpatialStructures.SelectMany(x => x.TopContainers).Each(mergeTopContainerIntoModel);
      }

      private Func<IContainer, IContainer> mapContainer(SimulationBuilder simulationBuilder) => container => _containerMapper.MapFrom(container, simulationBuilder);

      private Action<IContainer> mergeTopContainerInStructure(IContainer root) => topContainer =>
      {
         //probably should never happen
         if (topContainer == null)
            return;

         //In this case, we add or replace the top container
         if (topContainer.ParentPath == null)
            addOrReplaceContainer(topContainer, root);
         else
            insertTopContainerIntoStructure(topContainer, root);
      };

      private void insertTopContainerIntoStructure(IContainer topContainer, IContainer root)
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

      public IContainer MergeNeighborhoods(ModelConfiguration modelConfiguration) => _neighborhoodsMapper.MapFrom(modelConfiguration);
   }
}