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
      void CreateEvents(ModelConfiguration modelConfiguration);
   }

   internal class EventBuilderTask : IEventBuilderTask
   {
      private readonly IEventGroupBuilderToEventGroupMapper _eventGroupMapper;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ITransportBuilderToTransportMapper _transportMapper;

      public EventBuilderTask(
         IEventGroupBuilderToEventGroupMapper eventGroupMapper,
         IKeywordReplacerTask keywordReplacerTask,
         ITransportBuilderToTransportMapper transportMapper)
      {
         _eventGroupMapper = eventGroupMapper;
         _keywordReplacerTask = keywordReplacerTask;
         _transportMapper = transportMapper;
      }

      public void CreateEvents(ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder) = modelConfiguration;
         var caches = new EventBuilderCaches(model.Root.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList());

         //Cache all containers where the event group builder will be created using the source criteria
         foreach (var eventGroupBuilder in simulationBuilder.EventGroups)
         {
            if (caches.SourceCriteriaTargetContainers.Contains(eventGroupBuilder.SourceCriteria))
               continue;

            caches.SourceCriteriaTargetContainers.Add(eventGroupBuilder.SourceCriteria, caches.AllModelContainerDescriptors.AllSatisfiedBy(eventGroupBuilder.SourceCriteria));
         }

         simulationBuilder.EventGroups.Each(x => createEventGroupFrom(x, modelConfiguration, caches));
      }

      /// <summary>
      ///    Adds event group to all model containers with defined criteria
      /// </summary>
      private void createEventGroupFrom(EventGroupBuilder eventGroupBuilder, ModelConfiguration modelConfiguration, EventBuilderCaches caches)
      {
         foreach (var sourceContainer in caches.SourceCriteriaTargetContainers[eventGroupBuilder.SourceCriteria])
         {
            createEventGroupInContainer(eventGroupBuilder, sourceContainer, modelConfiguration, caches);
         }
      }

      /// <summary>
      ///    Adds event group to source container where event takes place
      /// </summary>
      private void createEventGroupInContainer(EventGroupBuilder eventGroupBuilder, IContainer sourceContainer, ModelConfiguration modelConfiguration, EventBuilderCaches caches)
      {
         //this creates recursively all event groups for the given builder
         var (_, simulationBuilder, replacementContext) = modelConfiguration;
         var eventGroup = _eventGroupMapper.MapFrom(eventGroupBuilder, simulationBuilder);
         sourceContainer.Add(eventGroup);

         //needs to add the required transport into model only for the added event group
         foreach (var childEventGroup in eventGroup.GetAllContainersAndSelf<EventGroup>())
         {
            var childEventGroupBuilder = simulationBuilder.BuilderFor(childEventGroup).DowncastTo<EventGroupBuilder>();
            if (childEventGroupBuilder is ApplicationBuilder applicationBuilder)
               addApplicationTransports(applicationBuilder, childEventGroup, modelConfiguration, caches);

            _keywordReplacerTask.ReplaceIn(childEventGroup, childEventGroupBuilder, replacementContext);
         }
      }

      private void addApplicationTransports(ApplicationBuilder applicationBuilder, EventGroup eventGroup, ModelConfiguration modelConfiguration, EventBuilderCaches caches)
      {
         var allEventGroupParentChildContainers = eventGroup.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();
         foreach (var appTransport in applicationBuilder.Transports)
         {
            var transportBuilder = appTransport;
            if (!caches.ApplicationTransportTargetContainers.Contains(transportBuilder.TargetCriteria))
               caches.ApplicationTransportTargetContainers.Add(appTransport.TargetCriteria, caches.AllModelContainerDescriptors.AllSatisfiedBy(transportBuilder.TargetCriteria));

            addApplicationTransportToModel(transportBuilder, allEventGroupParentChildContainers, applicationBuilder.MoleculeName, modelConfiguration, caches);
         }
      }

      private void addApplicationTransportToModel(TransportBuilder appTransport, EntityDescriptorMapList<IContainer> allEventGroupParentChildContainers, string moleculeName, ModelConfiguration modelConfiguration, EventBuilderCaches caches)
      {
         var appTransportSourceContainers = sourceContainersFor(appTransport, allEventGroupParentChildContainers);
         var appTransportTargetContainers = caches.ApplicationTransportTargetContainers[appTransport.TargetCriteria].ToList();
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

      /// <summary>
      ///    Caches only used to speed up the creation of events for one model. Created per call so that the task remains stateless
      /// </summary>
      private class EventBuilderCaches
      {
         public Cache<DescriptorCriteria, IEnumerable<IContainer>> SourceCriteriaTargetContainers { get; } = new Cache<DescriptorCriteria, IEnumerable<IContainer>>();
         public Cache<DescriptorCriteria, IEnumerable<IContainer>> ApplicationTransportTargetContainers { get; } = new Cache<DescriptorCriteria, IEnumerable<IContainer>>();
         public EntityDescriptorMapList<IContainer> AllModelContainerDescriptors { get; }

         public EventBuilderCaches(EntityDescriptorMapList<IContainer> allModelContainerDescriptors)
         {
            AllModelContainerDescriptors = allModelContainerDescriptors;
         }
      }
   }
}