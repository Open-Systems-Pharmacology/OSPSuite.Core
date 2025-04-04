using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   internal interface IEventGroupBuilderToEventGroupMapper : IBuilderMapper<EventGroupBuilder, EventGroup>
   {
   }

   internal class EventGroupBuilderToEventGroupMapper : IEventGroupBuilderToEventGroupMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IContainerBuilderToContainerMapper _containerMapper;
      private readonly IEventBuilderToEventMapper _eventMapper;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IMoleculeBuilderToMoleculeAmountMapper _moleculeMapper;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IParameterFactory _parameterFactory;
      private readonly IObjectTracker _objectTracker;

      public EventGroupBuilderToEventGroupMapper(
         IObjectBaseFactory objectBaseFactory,
         ICloneManagerForModel cloneManagerForModel,
         IContainerBuilderToContainerMapper containerMapper,
         IEventBuilderToEventMapper eventMapper,
         IParameterBuilderToParameterMapper parameterMapper,
         IMoleculeBuilderToMoleculeAmountMapper moleculeMapper,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterFactory parameterFactory, 
         IObjectTracker objectTracker)
      {
         _objectBaseFactory = objectBaseFactory;
         _cloneManagerForModel = cloneManagerForModel;
         _containerMapper = containerMapper;
         _eventMapper = eventMapper;
         _parameterMapper = parameterMapper;
         _moleculeMapper = moleculeMapper;
         _formulaMapper = formulaMapper;
         _parameterFactory = parameterFactory;
         _objectTracker = objectTracker;
      }

      public EventGroup MapFrom(EventGroupBuilder eventGroupBuilder, SimulationBuilder simulationBuilder)
      {
         var eventGroup = _objectBaseFactory.Create<EventGroup>();
         simulationBuilder.AddBuilderReference(eventGroup, eventGroupBuilder);
         _objectTracker.TrackObject(eventGroup, eventGroupBuilder, simulationBuilder);
         eventGroup.UpdatePropertiesFrom(eventGroupBuilder, _cloneManagerForModel);
         eventGroup.EventGroupType = eventGroupBuilder.EventGroupType;
         createEventGroupStructure(eventGroupBuilder, eventGroup, simulationBuilder);
         return eventGroup;
      }

      private void createEventGroupStructure(EventGroupBuilder eventGroupBuilder, EventGroup eventGroup, SimulationBuilder simulationBuilder)
      {
         foreach (var childBuilder in eventGroupBuilder.Children)
         {
            //nothing to do for these entities that should not be copied in the model structure
            if (doesNotBelongIntoModel(childBuilder))
               continue;

            if (childBuilder.IsAnImplementationOf<EventGroupBuilder>())
            {
               var childEventGroup = MapFrom(childBuilder.DowncastTo<EventGroupBuilder>(), simulationBuilder);
               eventGroup.Add(childEventGroup);

               if (childBuilder.IsAnImplementationOf<ApplicationBuilder>())
                  createApplication(childBuilder.DowncastTo<ApplicationBuilder>(), childEventGroup, simulationBuilder);
            }

            else if (childBuilder.IsAnImplementationOf<EventBuilder>())
               eventGroup.Add(_eventMapper.MapFrom(childBuilder.DowncastTo<EventBuilder>(), simulationBuilder));

            else if (childBuilder.IsAnImplementationOf<IParameter>())
               eventGroup.Add(_parameterMapper.MapFrom(childBuilder.DowncastTo<IParameter>(), simulationBuilder));

            else if (childBuilder.IsAnImplementationOf<IContainer>())
               eventGroup.Add(_containerMapper.MapFrom(childBuilder.DowncastTo<IContainer>(), simulationBuilder));

            else
               eventGroup.Add(_cloneManagerForModel.Clone(childBuilder));
         }
      }

      private static bool doesNotBelongIntoModel(IEntity childBuilder)
      {
         return childBuilder.IsAnImplementationOf<TransportBuilder>() || childBuilder.IsAnImplementationOf<ApplicationMoleculeBuilder>();
      }

      private void createApplication(ApplicationBuilder applicationBuilder, EventGroup eventGroup, SimulationBuilder simulationBuilder)
      {
         //---- add molecule amounts
         foreach (var appMolecule in applicationBuilder.Molecules)
         {
            //get container for the molecule
            var moleculeContainer = appMolecule.RelativeContainerPath.Resolve<IContainer>(eventGroup);

            var molecule = _moleculeMapper.MapFrom(simulationBuilder.MoleculeByName(applicationBuilder.MoleculeName), moleculeContainer, simulationBuilder);
            molecule.Formula = _formulaMapper.MapFrom(appMolecule.Formula, simulationBuilder);

            moleculeContainer.Add(molecule);

            addVolumeParameterTo(moleculeContainer);
         }
      }

      /// <summary>
      ///    Create parameter "Volume" in the given (application) container with constant formula=1
      ///    This parameter is required by molecule concentration parameter
      /// </summary>
      private void addVolumeParameterTo(IContainer moleculeContainer)
      {
         if (moleculeContainer.ContainsName(Constants.Parameters.VOLUME))
            return;

         var volume = _parameterFactory.CreateVolumeParameter();

         volume.Editable = false;
         volume.Visible = false;
         volume.IsDefault = true;

         moleculeContainer.Add(volume);
      }
   }
}