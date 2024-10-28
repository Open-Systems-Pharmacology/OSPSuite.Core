using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   internal interface IEventBuilderTask
   {
      void MergeEventGroups(ModelConfiguration modelConfiguration);
   }

   internal class EventBuilderTask : IEventBuilderTask
   {
      private readonly IEventGroupBuilderToEventGroupMapper _eventGroupMapper;
      private readonly IContainerMergeTask _containerMergeTask;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ITransportBuilderToTransportMapper _transportMapper;
      private ICache<DescriptorCriteria, IEnumerable<IContainer>> _sourceCriteriaTargetContainerCache;
      private Cache<DescriptorCriteria, IEnumerable<IContainer>> _applicationTransportTargetContainerCache;
      private EntityDescriptorMapList<IContainer> _allModelContainerDescriptors;

      public EventBuilderTask(
         IContainerMergeTask containerMergeTask,
         IEventGroupBuilderToEventGroupMapper eventGroupMapper,
         IKeywordReplacerTask keywordReplacerTask,
         ITransportBuilderToTransportMapper transportMapper)
      {
         _eventGroupMapper = eventGroupMapper;
         _keywordReplacerTask = keywordReplacerTask;
         _transportMapper = transportMapper;
         _containerMergeTask = containerMergeTask;
      }

      public void MergeEventGroups(ModelConfiguration modelConfiguration)
      {
         createMergedContainerStructureInRoot(modelConfiguration);
      }

      private void createMergedContainerStructureInRoot(ModelConfiguration modelConfiguration)
      {
         try
         {
            var (model, simulationBuilder) = modelConfiguration;

            _allModelContainerDescriptors = model.Root.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();
            _sourceCriteriaTargetContainerCache = new Cache<DescriptorCriteria, IEnumerable<IContainer>>();
            _applicationTransportTargetContainerCache = new Cache<DescriptorCriteria, IEnumerable<IContainer>>();

            simulationBuilder.EventGroupAndMergeBehaviors.Each(eventGroupBuildingBlockAndMerge =>
            {
               var (buildingBlock, mergeBehavior) = eventGroupBuildingBlockAndMerge;
               mergeEventGroups(modelConfiguration, buildingBlock, mergeBehavior);
            });
         }
         finally
         {
            _allModelContainerDescriptors = null;
            _sourceCriteriaTargetContainerCache.Clear();
            _sourceCriteriaTargetContainerCache = null;
            _applicationTransportTargetContainerCache.Clear();
            _applicationTransportTargetContainerCache = null;
         }
      }

      private void mergeEventGroups(ModelConfiguration modelConfiguration, EventGroupBuildingBlock buildingBlock, MergeBehavior mergeBehavior)
      {
         foreach (var eventGroupBuilder in buildingBlock)
         {
            if (_sourceCriteriaTargetContainerCache.Contains(eventGroupBuilder.SourceCriteria))
               continue;

            _sourceCriteriaTargetContainerCache.Add(eventGroupBuilder.SourceCriteria, _allModelContainerDescriptors.AllSatisfiedBy(eventGroupBuilder.SourceCriteria));
         }

         buildingBlock.Each(x => createEventGroupFrom(x, modelConfiguration, buildingBlock, mergeBehavior));
      }

      /// <summary>
      ///    Adds event group to all model containers with defined criteria
      /// </summary>
      private void createEventGroupFrom(EventGroupBuilder eventGroupBuilder, ModelConfiguration modelConfiguration, EventGroupBuildingBlock eventGroupBuildingBlock, MergeBehavior mergeBehavior)
      {
         foreach (var sourceContainer in _sourceCriteriaTargetContainerCache[eventGroupBuilder.SourceCriteria])
         {
            createEventGroupInContainer(eventGroupBuilder, sourceContainer, modelConfiguration, eventGroupBuildingBlock, mergeBehavior);
         }
      }

      /// <summary>
      ///    Adds event group to source container where event takes place
      /// </summary>
      private void createEventGroupInContainer(EventGroupBuilder eventGroupBuilder, IContainer sourceContainer, ModelConfiguration modelConfiguration, EventGroupBuildingBlock eventGroupBuildingBlock, MergeBehavior mergeBehavior)
      {
         //this creates recursively all event groups for the given builder
         var (_, simulationBuilder, replacementContext) = modelConfiguration;
         var eventGroup = _eventGroupMapper.MapFrom(eventGroupBuilder, simulationBuilder);

         tryMergeEventGroupInContainer(sourceContainer, eventGroup, mergeBehavior, eventGroupBuildingBlock);

         //needs to add the required transport into model only for the added event group
         foreach (var childEventGroup in eventGroup.GetAllContainersAndSelf<EventGroup>())
         {
            var childEventGroupBuilder = simulationBuilder.BuilderFor(childEventGroup).DowncastTo<EventGroupBuilder>();
            if (childEventGroupBuilder is ApplicationBuilder applicationBuilder)
               addApplicationTransports(applicationBuilder, childEventGroup, modelConfiguration);

            _keywordReplacerTask.ReplaceIn(childEventGroup, childEventGroupBuilder, replacementContext);
         }
      }

      private void addApplicationTransports(ApplicationBuilder applicationBuilder, EventGroup eventGroup, ModelConfiguration modelConfiguration)
      {
         var allEventGroupParentChildContainers = eventGroup.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();
         foreach (var appTransport in applicationBuilder.Transports)
         {
            var transportBuilder = appTransport;
            if (!_applicationTransportTargetContainerCache.Contains(transportBuilder.TargetCriteria))
               _applicationTransportTargetContainerCache.Add(appTransport.TargetCriteria, _allModelContainerDescriptors.AllSatisfiedBy(transportBuilder.TargetCriteria));

            addApplicationTransportToModel(transportBuilder, allEventGroupParentChildContainers, applicationBuilder.MoleculeName, modelConfiguration);
         }
      }

      private void addApplicationTransportToModel(TransportBuilder appTransport, EntityDescriptorMapList<IContainer> allEventGroupParentChildContainers, string moleculeName, ModelConfiguration modelConfiguration)
      {
         var appTransportSourceContainers = sourceContainersFor(appTransport, allEventGroupParentChildContainers);
         var appTransportTargetContainers = _applicationTransportTargetContainerCache[appTransport.TargetCriteria].ToList();
         var (_, simulationBuilder, replacementContext) = modelConfiguration;

         foreach (var sourceContainer in appTransportSourceContainers)
         {
            var sourceAmount = sourceContainer.GetSingleChildByName<MoleculeAmount>(moleculeName);
            if (sourceAmount == null)
               throw new OSPSuiteException(Validation.CannotCreateApplicationSourceNotFound(appTransport.Name, moleculeName, sourceContainer.Name));

            foreach (var targetContainer in appTransportTargetContainers)
            {
               var targetAmount = targetContainer.GetSingleChildByName<MoleculeAmount>(moleculeName);
               if (targetAmount == null)
                  throw new OSPSuiteException(Validation.CannotCreateApplicationTargetNotFound(appTransport.Name, moleculeName, targetContainer.Name));

               var transport = _transportMapper.MapFrom(appTransport, simulationBuilder);

               transport.SourceAmount = sourceAmount;
               transport.TargetAmount = targetAmount;

               _keywordReplacerTask.ReplaceIn(transport, moleculeName, replacementContext);

               //At the moment, no neighborhoods between application sub-containers and
               //spatial structure sub-containers are defined. Application transports are
               //added as direct children of the source molecule amount
               if (!sourceAmount.ContainsName(transport.Name))
                  sourceAmount.Add(transport);
               else
                  throw new OSPSuiteException(Validation.TransportAlreadyCreatorForMolecule(appTransport.Name, transport.Name, moleculeName));
            }
         }
      }

      private IEnumerable<IContainer> sourceContainersFor(TransportBuilder transport, EntityDescriptorMapList<IContainer> allEventGroupParentChildContainers)
      {
         return allEventGroupParentChildContainers.AllSatisfiedBy(transport.SourceCriteria);
      }

      private void tryMergeEventGroupInContainer(IContainer mergeContainer, EventGroup eventGroup, MergeBehavior mergeBehavior, EventGroupBuildingBlock eventGroupBuildingBlock)
      {
         //probably should never happen
         if (mergeContainer == null)
            return;

         if (mergeBehavior == MergeBehavior.Extend)
            _containerMergeTask.AddOrMergeContainer(mergeContainer, eventGroup);
         else
            _containerMergeTask.AddOrReplaceInContainer(mergeContainer, eventGroup);
      }
   }
}