using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Creates events in model using events-building block configuration
   /// </summary>
   public interface IEventBuilderTask
   {
      /// <summary>
      ///    Adds events defined by build configuration to the given model
      /// </summary>
      /// <param name="buildConfiguration">the build configuration</param>
      /// <param name="model">the model where the obervers should be defined</param>
      void CreateEvents(IBuildConfiguration buildConfiguration, IModel model);
   }

   internal class EventBuilderTask : IEventBuilderTask
   {
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ITransportBuilderToTransportMapper _transportMapper;
      private readonly IEventGroupBuilderToEventGroupMapper _eventGroupMapper;
      private readonly IContainerTask _containerTask;
      private IModel _model;
      private EntityDescriptorMapList<IContainer> _allModelContainerDescriptors;
      private ICache<DescriptorCriteria, IEnumerable<IContainer>> _sourceCriteriaTargetContainerCache;
      private ICache<DescriptorCriteria, IEnumerable<IContainer>> _applicationTransportTargetContainerCache;
      private IBuildConfiguration _buildConfiguration;
      private IList<IEventGroup> _eventGroupsWhichAreNotApplications;

      public EventBuilderTask(
         IKeywordReplacerTask keywordReplacerTask, 
         ITransportBuilderToTransportMapper transportMapper, 
         IEventGroupBuilderToEventGroupMapper eventGroupMapper,
         IContainerTask containerTask)
      {
         _keywordReplacerTask = keywordReplacerTask;
         _transportMapper = transportMapper;
         _eventGroupMapper = eventGroupMapper;
         _containerTask = containerTask;
      }

      public void CreateEvents(IBuildConfiguration buildConfiguration, IModel model)
      {
         try
         {
            _model = model;
            _buildConfiguration = buildConfiguration;
            _allModelContainerDescriptors = model.Root.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();

            _sourceCriteriaTargetContainerCache = new Cache<DescriptorCriteria, IEnumerable<IContainer>>();
            _applicationTransportTargetContainerCache = new Cache<DescriptorCriteria, IEnumerable<IContainer>>();

            _eventGroupsWhichAreNotApplications=new List<IEventGroup>();

            //Cache all containers where the event group builder will be created using the source criteria
            foreach (var eventGroupBuilder in _buildConfiguration.EventGroups)
            {
               if (_sourceCriteriaTargetContainerCache.Contains(eventGroupBuilder.SourceCriteria))
                  continue;

               _sourceCriteriaTargetContainerCache.Add(eventGroupBuilder.SourceCriteria, _allModelContainerDescriptors.AllSatisfiedBy(eventGroupBuilder.SourceCriteria));
            }

            foreach (var eventGroupBuilder in _buildConfiguration.EventGroups)
            {
               createEventGroupFrom(eventGroupBuilder, buildConfiguration.Molecules);
            }

            //---- Replace the keyword MOLECULE in the event groups which are not applications with
            //     the name of the first floating molecule. This is done only for backward compatibility:
            //
            //     - continuous EHC defined in old PK-Sim 5.x projects.
            //       (the new implementation of EHC does not require this kind of replacement)
            //
            //     - immediate EHC created during project conversion from old PK-Sim 4.2 project
            //
            //     This step must be done after ALL other keyword replacements
            foreach (var nonAppEventGroup in _eventGroupsWhichAreNotApplications )
            {
               _keywordReplacerTask.ReplaceMoleculeKeywordInNonApplicationEventGroup(nonAppEventGroup,buildConfiguration.Molecules);
            }
         }
         finally
         {
            _model = null;
            _allModelContainerDescriptors = null;
            _sourceCriteriaTargetContainerCache.Clear();
            _sourceCriteriaTargetContainerCache = null;
            _applicationTransportTargetContainerCache.Clear();
            _applicationTransportTargetContainerCache = null;
            _buildConfiguration = null;
         }
      }

      /// <summary>
      ///    Adds event group to all model containers with defined criteria
      /// </summary>
      private void createEventGroupFrom(IEventGroupBuilder eventGroupBuilder, IMoleculeBuildingBlock molecules)
      {
         foreach (var sourceContainer in _sourceCriteriaTargetContainerCache[eventGroupBuilder.SourceCriteria])
         {
            createEventGroupInContainer(eventGroupBuilder, sourceContainer);
         }
      }

      /// <summary>
      ///    Adds event group to source container where event takes place
      /// </summary>
      private void createEventGroupInContainer(IEventGroupBuilder eventGroupBuilder, IContainer sourceContainer)
      {
         //this creates recursively all event groups for the given builder
         var eventGroup = _eventGroupMapper.MapFrom(eventGroupBuilder, _buildConfiguration);
         sourceContainer.Add(eventGroup);

         //needs to add the requires transport into model only for the added event group
         foreach (var childEventGroup in eventGroup.GetAllContainersAndSelf<IEventGroup>())
         {
            var childEventGroupBuilder = _buildConfiguration.BuilderFor(childEventGroup).DowncastTo<IEventGroupBuilder>();
            if (childEventGroupBuilder.IsAnImplementationOf<IApplicationBuilder>())
            {
               addApplicationTransports(childEventGroupBuilder.DowncastTo<IApplicationBuilder>(), childEventGroup);
            }
            else
            {
               _eventGroupsWhichAreNotApplications.Add(childEventGroup);
            }

            _keywordReplacerTask.ReplaceIn(childEventGroup, _model.Root, childEventGroupBuilder, _buildConfiguration.Molecules);
         }
      }

      private void addApplicationTransports(IApplicationBuilder applicationBuilder, IEventGroup eventGroup)
      {
         var allEventGroupParentChildContainers = eventGroup.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();
         foreach (var appTransport in applicationBuilder.Transports)
         {
            var transportBuilder = appTransport;
            if (!_applicationTransportTargetContainerCache.Contains(transportBuilder.TargetCriteria))
               _applicationTransportTargetContainerCache.Add(appTransport.TargetCriteria, _allModelContainerDescriptors.AllSatisfiedBy(transportBuilder.TargetCriteria));

            addApplicationTransportToModel(transportBuilder, allEventGroupParentChildContainers, applicationBuilder.MoleculeName);
         }
      }

      private void addApplicationTransportToModel(ITransportBuilder appTransport, EntityDescriptorMapList<IContainer> allEventGroupParentChildContainers, string moleculeName)
      {
         var appTransportSourceContainers = sourceContainersFor(appTransport, allEventGroupParentChildContainers);
         var appTransportTargetContainers = _applicationTransportTargetContainerCache[appTransport.TargetCriteria].ToList();

         foreach (var sourceContainer in appTransportSourceContainers)
         {
            var sourceAmount = sourceContainer.GetSingleChildByName<IMoleculeAmount>(moleculeName);
            if (sourceAmount == null)
               throw new OSPSuiteException(Validation.CannotCreateApplicationSourceNotFound(appTransport.Name, moleculeName, sourceContainer.Name));

            foreach (var targetContainer in appTransportTargetContainers)
            {
               var targetAmount = targetContainer.GetSingleChildByName<IMoleculeAmount>(moleculeName);
               if (targetAmount == null)
                  throw new OSPSuiteException(Validation.CannotCreateApplicationTargetNotFound(appTransport.Name, moleculeName, targetContainer.Name));

               var transport = _transportMapper.MapFrom(appTransport, _buildConfiguration);

               transport.SourceAmount = sourceAmount;
               transport.TargetAmount = targetAmount;

               _keywordReplacerTask.ReplaceIn(transport, _model.Root, moleculeName);

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

      private IEnumerable<IContainer> sourceContainersFor(ITransportBuilder transport, EntityDescriptorMapList<IContainer> allEventGroupParentChildContainers)
      {
         return allEventGroupParentChildContainers.AllSatisfiedBy(transport.SourceCriteria);
      }
   }
}