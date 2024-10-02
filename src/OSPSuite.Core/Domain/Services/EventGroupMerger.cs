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
   internal interface IEventGroupMerger
   {
      IContainer MergeEventGroupsContainer(ModelConfiguration modelConfiguration);
   }

   internal class EventGroupMerger : IEventGroupMerger
   {
      
      private readonly IEventGroupBuilderToEventGroupMapper _eventGroupMapper;
      private readonly IContainerMergeTask _containerMergeTask;
      private readonly IEventBuilderTask _eventBuilderTask;
      
      public EventGroupMerger(
         IContainerMergeTask containerMergeTask, 
         IEventBuilderTask eventBuilderTask,
         IEventGroupBuilderToEventGroupMapper eventGroupMapper)
      {
         _eventGroupMapper = eventGroupMapper;
         _containerMergeTask = containerMergeTask;
         _eventBuilderTask = eventBuilderTask;
      }

      public IContainer MergeEventGroupsContainer(ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder) = modelConfiguration;

         _eventBuilderTask.CreateEvents(modelConfiguration);
         createMergedContainerStructureInRoot(model.Root, simulationBuilder);

         return model.Root;
      }

      private void createMergedContainerStructureInRoot(IContainer root, SimulationBuilder simulationBuilder)
      {
         var allEventGroupAndMergeBehaviors = simulationBuilder.EventGroupAndMergeBehaviors;
         if (!allEventGroupAndMergeBehaviors.Any())
            return;

         var firstEventGroupBuildingBlock = allEventGroupAndMergeBehaviors[0].eventGroupBuildingBlock;
         var allOtherSpatialStructuresWithMergeBehavior = allEventGroupAndMergeBehaviors.Skip(1).ToList();
         var mapToModelContainer = mapContainerDef(simulationBuilder);

         root.AddChildren(firstEventGroupBuildingBlock.Select(mapToModelContainer));

         allOtherSpatialStructuresWithMergeBehavior
            .Select(x => new {MergeBehavior = x.mergeBehavior, eventGroups = x.eventGroupBuildingBlock.Select(mapToModelContainer), buildingBlock = x.eventGroupBuildingBlock})
            .Each(x => x.eventGroups.Each(eventGroup => tryMergeEventGroupInContainer(root, eventGroup, x.MergeBehavior, x.buildingBlock)));
      }

      private void tryMergeEventGroupInContainer(IContainer root, IContainer eventGroup, MergeBehavior mergeBehavior, EventGroupBuildingBlock eventGroupBuildingBlock)
      {
         try
         {
            mergeTopContainerInEventGroup(eventGroup, root, mergeBehavior);
         }
         catch (Exception)
         {
            throw new OSPSuiteException(Error.CannotFindParentContainerWithPath(root.ParentPath.PathAsString, root.Name, eventGroupBuildingBlock.Name, eventGroupBuildingBlock.Module.Name));
         }
      }

      private Func<EventGroupBuilder, IContainer> mapContainerDef(SimulationBuilder simulationBuilder) => container => _eventGroupMapper.MapFrom(container, simulationBuilder);

      private void mergeTopContainerInEventGroup(IContainer eventGroup, IContainer root, MergeBehavior mergeBehavior)
      {
         //probably should never happen
         if (root == null)
            return;

         //In this case, we add or replace the top container
         if (root.ParentPath == null || string.IsNullOrEmpty(root.ParentPath.PathAsString))
            replaceOrMergeContainerIntoParent(root, eventGroup, mergeBehavior);
         else
            insertTopContainerIntoStructure(root, eventGroup, mergeBehavior);
      }

      private void insertTopContainerIntoStructure(IContainer topContainer, IContainer root, MergeBehavior mergeBehavior)
      {
         var parentContainer = root.ParentPath.Resolve<IContainer>(root);
         if (parentContainer == null)
            throw new Exception();

         replaceOrMergeContainerIntoParent(parentContainer,topContainer  ,mergeBehavior);
      }

      private void replaceOrMergeContainerIntoParent(IContainer parentContainer, IContainer containerToMerge, MergeBehavior mergeBehavior)
      {
         //Merge behavior is extend or we have a special case to deal with when dealing with MoleculeProperties container that we merge instead of replacing
         if (mergeBehavior == MergeBehavior.Extend)
            _containerMergeTask.AddOrMergeContainer(parentContainer, containerToMerge);
         else
            _containerMergeTask.AddOrReplaceInContainer(parentContainer,containerToMerge);
      }
   } 
}