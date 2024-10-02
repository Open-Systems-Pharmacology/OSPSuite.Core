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
      private readonly IContainerMergeTask _containerMergeTask;

      public SpatialStructureMerger(
         IObjectBaseFactory objectBaseFactory,
         IContainerBuilderToContainerMapper containerMapper,
         INeighborhoodCollectionToContainerMapper neighborhoodsMapper,
         IContainerMergeTask containerMergeTask)
      {
         _objectBaseFactory = objectBaseFactory;
         _containerMapper = containerMapper;
         _neighborhoodsMapper = neighborhoodsMapper;
         _containerMergeTask = containerMergeTask;
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

         // First step: We create the container structure.
         // This is done by adding all top containers defined in the FIRST spatial structure
         // Then we merge all other top containers defined in the other spatial structures based on the merge behavior

         // Add each container defined in the spatial structure and direct child of the root container
         root.AddChildren(firstSpatialStructure.TopContainers.Select(mapToModelContainer));

         //Merge all other spatial structures
         //make sure we map the container to a model container so that we do not change the original containers
         allOtherSpatialStructuresWithMergeBehavior
            .Select(x => new {x.mergeBehavior, topContainers = x.spatialStructure.TopContainers.Select(mapToModelContainer).ToList(), x.spatialStructure})
            .Each(x => x.topContainers.Each(topContainer => tryMergeTopContainerInStructure(root, topContainer, x.mergeBehavior, x.spatialStructure)));

         //create the temporary GLOBAL MOLECULE PROPERTIES THAT WILL BE REMOVED AT THE END but used as based for copying
         //For molecule properties, we always merged as we used to and never replace
         var allGlobalMoleculeContainers = allSpatialStructureAndMergeBehaviors
            .Select(x => x.spatialStructure.GlobalMoleculeDependentProperties)
            .Select(mapToModelContainer)
            .ToList();


         var firstGlobalMoleculeContainer = allGlobalMoleculeContainers.FirstOrDefault();
         var otherGlobalMoleculeContainer = allGlobalMoleculeContainers.Skip(1).ToList();

         if (firstGlobalMoleculeContainer != null)
         {
            otherGlobalMoleculeContainer.Each(x => _containerMergeTask.MergeContainers(firstGlobalMoleculeContainer, x));
            root.Add(firstGlobalMoleculeContainer);
         }
      }

      private void tryMergeTopContainerInStructure(IContainer root, IContainer topContainer, MergeBehavior mergeBehavior, SpatialStructure spatialStructure)
      {
         try
         {
            mergeTopContainerInStructure(topContainer, root, mergeBehavior);
         }
         catch (Exception)
         {
            throw new OSPSuiteException(Error.CannotFindParentContainerWithPath(topContainer.ParentPath.PathAsString, topContainer.Name, spatialStructure.Name, spatialStructure.Module.Name));
         }
      }

      private Func<IContainer, IContainer> mapContainerDef(SimulationBuilder simulationBuilder) => container => _containerMapper.MapFrom(container, simulationBuilder);

      private void mergeTopContainerInStructure(IContainer topContainer, IContainer root, MergeBehavior mergeBehavior)
      {
         //probably should never happen
         if (topContainer == null)
            return;

         //In this case, we add or replace the top container
         if (topContainer.ParentPath == null || string.IsNullOrEmpty(topContainer.ParentPath.PathAsString))
            replaceOrMergeContainerIntoParent(root, topContainer, mergeBehavior);
         else
            insertTopContainerIntoStructure(topContainer, root, mergeBehavior);
      }

      private void insertTopContainerIntoStructure(IContainer topContainer, IContainer root, MergeBehavior mergeBehavior)
      {
         var parentContainer = topContainer.ParentPath.Resolve<IContainer>(root);
         if (parentContainer == null)
            throw new ContainerNotFoundException();

         replaceOrMergeContainerIntoParent(parentContainer, topContainer, mergeBehavior);
      }

      private void replaceOrMergeContainerIntoParent(IContainer parentContainer, IContainer containerToMerge, MergeBehavior mergeBehavior)
      {
         //Merge behavior is extend or we have a special case to deal with when dealing with MoleculeProperties container that we merge instead of replacing
         if (mergeBehavior == MergeBehavior.Extend || containerToMerge.IsNamed(Constants.MOLECULE_PROPERTIES))
            _containerMergeTask.AddOrMergeContainer(parentContainer, containerToMerge);
         else
            _containerMergeTask.AddOrReplaceInContainer(parentContainer, containerToMerge);
      }

      public IContainer MergeNeighborhoods(ModelConfiguration modelConfiguration) => _neighborhoodsMapper.MapFrom(modelConfiguration);
   }

   internal class ContainerNotFoundException : OSPSuiteException
   {
   }
}