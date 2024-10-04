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
   /// <summary>
   ///    Creates events in model using events-building block configuration
   /// </summary>
   internal interface IEventBuilderTask
   {
      /// <summary>
      ///    Adds events defined by build configuration to the given model
      /// </summary>
      void CreateEvents(ModelConfiguration modelConfiguration);
   }

   internal class EventBuilderTask : IEventBuilderTask
   {
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ITransportBuilderToTransportMapper _transportMapper;
      private readonly IEventGroupBuilderToEventGroupMapper _eventGroupMapper;
      private EntityDescriptorMapList<IContainer> _allModelContainerDescriptors;
      private ICache<DescriptorCriteria, IEnumerable<IContainer>> _sourceCriteriaTargetContainerCache;
      private ICache<DescriptorCriteria, IEnumerable<IContainer>> _applicationTransportTargetContainerCache;

      public EventBuilderTask(
         IKeywordReplacerTask keywordReplacerTask,
         ITransportBuilderToTransportMapper transportMapper,
         IEventGroupBuilderToEventGroupMapper eventGroupMapper)
      {
         _keywordReplacerTask = keywordReplacerTask;
         _transportMapper = transportMapper;
         _eventGroupMapper = eventGroupMapper;
      }

      public void CreateEvents(ModelConfiguration modelConfiguration)
      {
         try
         {
            var (model, simulationBuilder) = modelConfiguration;
            _allModelContainerDescriptors = model.Root.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();

            _sourceCriteriaTargetContainerCache = new Cache<DescriptorCriteria, IEnumerable<IContainer>>();
            _applicationTransportTargetContainerCache = new Cache<DescriptorCriteria, IEnumerable<IContainer>>();

            //This is how we get the container to add the eventGroupBuilder
            //But then, when merging, I might have more than one. 
            //private EntityDescriptorMapList<IContainer> _allModelContainerDescriptors;
            //_allModelContainerDescriptors = model.Root.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();
            //_allModelContainerDescriptors.AllSatisfiedBy(eventGroupBuilder.SourceCriteria)
            //
            //Cache all containers where the event group builder will be created using the source criteria
            foreach (var eventGroupBuilder in simulationBuilder.EventGroups)
            {
               if (_sourceCriteriaTargetContainerCache.Contains(eventGroupBuilder.SourceCriteria))
                  continue;

               _sourceCriteriaTargetContainerCache.Add(eventGroupBuilder.SourceCriteria, _allModelContainerDescriptors.AllSatisfiedBy(eventGroupBuilder.SourceCriteria));
            }

            simulationBuilder.EventGroups.Each(x => createEventGroupFrom(x, modelConfiguration));
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

      /// <summary>
      ///    Adds event group to all model containers with defined criteria
      /// </summary>
      private void createEventGroupFrom(EventGroupBuilder eventGroupBuilder, ModelConfiguration modelConfiguration)
      {
         foreach (var sourceContainer in _sourceCriteriaTargetContainerCache[eventGroupBuilder.SourceCriteria])
         {
            createEventGroupInContainer(eventGroupBuilder, sourceContainer, modelConfiguration);
         }
      }

      /// <summary>
      ///    Adds event group to source container where event takes place
      /// </summary>
      private void createEventGroupInContainer(EventGroupBuilder eventGroupBuilder, IContainer sourceContainer, ModelConfiguration modelConfiguration)
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
   }
}