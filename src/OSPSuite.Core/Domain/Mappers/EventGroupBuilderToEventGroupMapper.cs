using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   internal interface IEventGroupBuilderToEventGroupMapper : IBuilderMapper<IEventGroupBuilder, IEventGroup>
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

      public EventGroupBuilderToEventGroupMapper(
         IObjectBaseFactory objectBaseFactory,
         ICloneManagerForModel cloneManagerForModel,
         IContainerBuilderToContainerMapper containerMapper,
         IEventBuilderToEventMapper eventMapper,
         IParameterBuilderToParameterMapper parameterMapper,
         IMoleculeBuilderToMoleculeAmountMapper moleculeMapper,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterFactory parameterFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _cloneManagerForModel = cloneManagerForModel;
         _containerMapper = containerMapper;
         _eventMapper = eventMapper;
         _parameterMapper = parameterMapper;
         _moleculeMapper = moleculeMapper;
         _formulaMapper = formulaMapper;
         _parameterFactory = parameterFactory;
      }

      public IEventGroup MapFrom(IEventGroupBuilder eventGroupBuilder, SimulationBuilder simulationBuilder)
      {
         var eventGroup = _objectBaseFactory.Create<IEventGroup>();
         simulationBuilder.AddBuilderReference(eventGroup, eventGroupBuilder);
         eventGroup.UpdatePropertiesFrom(eventGroupBuilder, _cloneManagerForModel);
         eventGroup.EventGroupType = eventGroupBuilder.EventGroupType;
         createEventGroupStructure(eventGroupBuilder, eventGroup, simulationBuilder);
         return eventGroup;
      }

      private void createEventGroupStructure(IEventGroupBuilder eventGroupBuilder, IEventGroup eventGroup, SimulationBuilder simulationBuilder)
      {
         foreach (var childBuilder in eventGroupBuilder.Children)
         {
            //nothing to do for these entities that should not be copied in the model structure
            if (doesNotBelongIntoModel(childBuilder))
               continue;

            if (childBuilder.IsAnImplementationOf<IEventGroupBuilder>())
            {
               var childEventGroup = MapFrom(childBuilder.DowncastTo<IEventGroupBuilder>(), simulationBuilder);
               eventGroup.Add(childEventGroup);

               if (childBuilder.IsAnImplementationOf<IApplicationBuilder>())
                  createApplication(childBuilder.DowncastTo<IApplicationBuilder>(), childEventGroup, simulationBuilder);
            }

            else if (childBuilder.IsAnImplementationOf<IEventBuilder>())
               eventGroup.Add(_eventMapper.MapFrom(childBuilder.DowncastTo<IEventBuilder>(), simulationBuilder));

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
         return childBuilder.IsAnImplementationOf<ITransportBuilder>() || childBuilder.IsAnImplementationOf<IApplicationMoleculeBuilder>();
      }

      private void createApplication(IApplicationBuilder applicationBuilder, IEventGroup eventGroup, SimulationBuilder simulationBuilder)
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