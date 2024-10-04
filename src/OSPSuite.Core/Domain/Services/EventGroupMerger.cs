using System;
using System.Collections;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using System.Reflection;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using System.Collections.Generic;

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
         //When runniong the should_not_crash test:
            //we have the "Bolus Application" here -> Root|Organism|ArterialBlood|Plasma|Bolus Application
         createMergedContainerStructureInRoot(model.Root, simulationBuilder);
         //After this it is on the Root Container, due to this line inside the createMergedContainerStructureInRoot method:
         //root.AddChildren(firstEventGroupBuildingBlock.Select(mapToModelContainer));

         return model.Root;
      }
      private ICache<DescriptorCriteria, IEnumerable<IContainer>> _sourceCriteriaTargetContainerCache;
      private void createMergedContainerStructureInRoot(IContainer root, SimulationBuilder simulationBuilder)
      {
         _sourceCriteriaTargetContainerCache = new Cache<DescriptorCriteria, IEnumerable<IContainer>>();
         var allEventGroupAndMergeBehaviors = simulationBuilder.EventGroupAndMergeBehaviors;
         if (!allEventGroupAndMergeBehaviors.Any())
            return;

         var firstEventGroupBuildingBlock = allEventGroupAndMergeBehaviors[0].eventGroupBuildingBlock;
         var allOtherSpatialStructuresWithMergeBehavior = allEventGroupAndMergeBehaviors.Skip(1).ToList();
         var mapToModelContainer = mapContainerDef(simulationBuilder);


         //Adding this to replicate somehow, how the containers are added to the model
         //This is extracted from the CreateEvents method in the EventBuilderTask class
         //Cache all containers where the event group builder will be created using the source criteria
         //Depending on the test running, the should_have_merged_the_events_with_second_present_only or should_not_crash
         //we have a different outcome from this. Again this is just a test that I`m doing.

         //I think we might have to find what container to place the events on by this criteria somehow.
         //as mentioned befor, it is setting them on "PLASMA" container. But what if we don`t have one?
         //that`s happening with my should_have_merged_the_events_with_second_present_only  test, maybe I 
         //should add it to the construction of the modules? 

         EntityDescriptorMapList<IContainer> _allModelContainerDescriptors;
         _allModelContainerDescriptors = root.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();

         foreach (var eventGroupBuilder in simulationBuilder.EventGroups)
         {
            if (_sourceCriteriaTargetContainerCache.Contains(eventGroupBuilder.SourceCriteria))
               continue;

            _sourceCriteriaTargetContainerCache.Add(eventGroupBuilder.SourceCriteria, _allModelContainerDescriptors.AllSatisfiedBy(eventGroupBuilder.SourceCriteria));
         }
         //This contains all the containers to add the eventgroups? -> _sourceCriteriaTargetContainerCache

         foreach (var eventGroupBuilder in simulationBuilder.EventGroups)
         {
            var lst = _allModelContainerDescriptors.AllSatisfiedBy(eventGroupBuilder.SourceCriteria);
         }
         


         root.AddChildren(firstEventGroupBuildingBlock.Select(mapToModelContainer));

         //var children = firstEventGroupBuildingBlock.Select(mapToModelContainer);
         //var uniqueChildren = children.Where(newChild => !root.Children.Any(existingChild => existingChild.Name == newChild.Name));
         //root.AddChildren(uniqueChildren);

         allOtherSpatialStructuresWithMergeBehavior
            .Select(x => new { MergeBehavior = x.mergeBehavior, eventGroups = x.eventGroupBuildingBlock.Select(mapToModelContainer), buildingBlock = x.eventGroupBuildingBlock })
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

         replaceOrMergeContainerIntoParent(parentContainer, topContainer, mergeBehavior);
      }

      private void replaceOrMergeContainerIntoParent(IContainer parentContainer, IContainer containerToMerge, MergeBehavior mergeBehavior)
      {
         //Merge behavior is extend or we have a special case to deal with when dealing with MoleculeProperties container that we merge instead of replacing
         if (mergeBehavior == MergeBehavior.Extend)
            _containerMergeTask.AddOrMergeContainer(parentContainer, containerToMerge);
         else
            _containerMergeTask.AddOrReplaceInContainer(parentContainer, containerToMerge);
      }
   }
}