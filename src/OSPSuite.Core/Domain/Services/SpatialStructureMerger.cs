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
         var allSpatialStructureAndMergeBehaviors = simulationBuilder.SpatialStructureAndMergeBehaviors;
         if (!allSpatialStructureAndMergeBehaviors.Any())
            return;

         var firstSpatialStructure = allSpatialStructureAndMergeBehaviors[0].spatialStructure;
         var allOtherSpatialStructuresWithMergeBehavior = allSpatialStructureAndMergeBehaviors.Skip(1).ToList();
         var mapToModelContainer = mapContainerDef(simulationBuilder);
         var mergeTopContainerIntoModel = mergeTopContainerInStructure(root);

         // First step: We create the container structure.
         // This is done by adding all top containers defined in the FIRST spatial structure
         // Then we merge all other top containers defined in the other spatial structures based on the merge behavior

         // Add each container defined in the spatial structure and direct child of the root container
         root.AddChildren(firstSpatialStructure.TopContainers.Select(mapToModelContainer));

         //Merge all other spatial structures
         //make sure we map the container to a model container so that we do not change the original containers

         allOtherSpatialStructuresWithMergeBehavior.Select(x => new {x.mergeBehavior, topContainers = x.spatialStructure.TopContainers.Select(mapToModelContainer).ToList()})
            .Each(x => x.topContainers.Each(topContainer => mergeTopContainerIntoModel(topContainer, x.mergeBehavior)));

         //create the temporary GLOBAL MOLECULE PROPERTIES THAT WILL BE REMOVED AT THE END but used as based for copying
         //For molecule properties, we always merged as we used to and never replace
         var allGlobalMoleculeContainers = allSpatialStructureAndMergeBehaviors
            .Select(x=>x.spatialStructure.GlobalMoleculeDependentProperties)
            .Select(mapToModelContainer)
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

      private Action<IContainer, MergeBehavior> mergeTopContainerInStructure(IContainer root) => (topContainer, mergeBehavior) =>
      {
         //probably should never happen
         if (topContainer == null)
            return;

         //In this case, we add or replace the top container
         if (topContainer.ParentPath == null || string.IsNullOrEmpty(topContainer.ParentPath.PathAsString))
            addOrReplaceContainer(topContainer, root, mergeBehavior);
         else
            insertTopContainerIntoStructure(topContainer, root, mergeBehavior);
      };

      private void insertTopContainerIntoStructure(IContainer topContainer, IContainer root, MergeBehavior mergeBehavior)
      {
         var parentContainer = topContainer.ParentPath.Resolve<IContainer>(root);
         if (parentContainer == null)
            throw new OSPSuiteException(Error.CannotFindParentContainerWithPath(topContainer.ParentPath.PathAsString, topContainer.Name));

         addOrReplaceContainer(topContainer, parentContainer, mergeBehavior);
      }

      private void addOrReplaceContainer(IContainer containerToAdd, IContainer parentContainer, MergeBehavior mergeBehavior)
      {
         //Merge behavior is extend or we have a special case to deal with when dealing with MoleculeProperties container that we merge instead of replacing
         if (mergeBehavior == MergeBehavior.Extend || containerToAdd.IsNamed(Constants.MOLECULE_PROPERTIES))
            addOrMergeContainer(parentContainer, containerToAdd);
         else
            addOrReplaceInContainer(parentContainer, containerToAdd);
      }

      //Adds or replace all children from the containerToMerge into the targetContainer
      private void mergeContainers(IContainer targetContainer, IContainer containerToMerge)
      {
         containerToMerge.Children.Each(x => addOrReplaceInContainer(targetContainer, x));
      }

      private void addOrMergeContainer(IContainer parentContainer, IContainer containerToAddOrMerge)
      {
         var existingContainer = parentContainer.Container(containerToAddOrMerge.Name);
         if (existingContainer == null)
            parentContainer.Add(containerToAddOrMerge);
         else
            mergeContainers(existingContainer, containerToAddOrMerge);
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