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
         var mapToModelContainer = mapContainerDef(simulationBuilder);
         var mergeTopContainerIntoModel = mergeTopContainerInStructure(root);

         // First step: We create the container structure.
         // This is done by adding all top containers defined in the FIRST spatial structure
         // Then we merge all other top containers defined in the other spatial structures

         // Add each container defined in the spatial structure and direct child of the root container
         root.AddChildren(firstSpatialStructure.TopContainers.Select(mapToModelContainer));

         //Merge all other spatial structures
         allOtherSpatialStructures.SelectMany(x => x.TopContainers)
            //make sure we map the container to a model container so that we do not change the original containers
            .Select(mapToModelContainer)
            .Each(mergeTopContainerIntoModel);

         //create the temporary GLOBAL MOLECULE PROPERTIES THAT WILL BE REMOVED AT THE END but used as based for copying
         var allGlobalMoleculeContainers = simulationBuilder
            .SpatialStructures
            .Select(x => x?.GlobalMoleculeDependentProperties)
            .Where(x => x != null)
            .ToList();


         var firstGlobalMoleculeContainer = allGlobalMoleculeContainers.FirstOrDefault();
         var otherGlobalMoleculeContainer = allGlobalMoleculeContainers.Skip(1).ToList();

         if (firstGlobalMoleculeContainer != null)
         {
            otherGlobalMoleculeContainer.Each(x => mergeContainers(firstGlobalMoleculeContainer, x));
            root.Add(firstGlobalMoleculeContainer);
         }
      }

      private Func<IContainer, IContainer> mapContainerDef(SimulationBuilder simulationBuilder) => container => _containerMapper.MapFrom(container, simulationBuilder);

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

      private void addOrReplaceContainer(IContainer containerToAdd, IContainer parentContainer)
      {
         //we do have a special case to deal with when dealing with MoleculeProperties container that we merge instead of replacing
         if (containerToAdd.IsNamed(Constants.MOLECULE_PROPERTIES))
         {
            var existingContainer = parentContainer.Container(containerToAdd.Name);
            if (existingContainer == null)
               parentContainer.Add(containerToAdd);
            else
               mergeContainers(existingContainer, containerToAdd);
         }
         else
            addOrReplaceInContainer(parentContainer, containerToAdd);
      }

      //Adds or replace all children from the containerToMerge into the targetContainer
      private void mergeContainers(IContainer targetContainer, IContainer containerToMerge)
      {
         containerToMerge.Children.Each(x => addOrReplaceInContainer(targetContainer, x));
      }

      private void addOrReplaceInContainer(IContainer container, IEntity objectToReplace)
      {
         var existingChild = container.GetSingleChildByName(objectToReplace.Name);
         if (existingChild != null)
            container.RemoveChild(existingChild);

         container.Add(objectToReplace);
      }

      public IContainer MergeNeighborhoods(ModelConfiguration modelConfiguration) => _neighborhoodsMapper.MapFrom(modelConfiguration);
   }
}