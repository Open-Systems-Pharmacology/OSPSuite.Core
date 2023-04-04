using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_SimulationConfigurationValidator : ContextSpecification<ISimulationConfigurationValidator>
   {
      protected override void Context()
      {
         sut = new SimulationConfigurationValidator();
      }
   }

   internal class When_validating_a_simulation_configuration_with_an_incorrect_application : concern_for_SimulationConfigurationValidator
   {
      private SimulationConfiguration _simulationConfiguration;
      private ValidationResult _validationResults;
      private IEventGroupBuildingBlock _eventBuildingBlock;
      private IApplicationBuilder _applicationBuilder;
      private MoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _eventBuildingBlock = new EventGroupBuildingBlock().WithName("Events");
         _applicationBuilder = new ApplicationBuilder().WithName("Tada");
         _applicationBuilder.MoleculeName = "B";
         _eventBuildingBlock.Add(_applicationBuilder);
         _moleculeBuildingBlock = new MoleculeBuildingBlock().WithName("Molecules");
         _moleculeBuildingBlock.Add(new MoleculeBuilder().WithName("A"));

         var module = new Module
         {
            Molecules = _moleculeBuildingBlock,
            Observers = new ObserverBuildingBlock(),
            PassiveTransports = new PassiveTransportBuildingBlock(),
            Reactions = new ReactionBuildingBlock(),
            SpatialStructure = new SpatialStructure(),
            EventGroups = _eventBuildingBlock
         };
         module.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock());
         module.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());

         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module));

      }

      protected override void Because()
      {
         _validationResults = sut.Validate(_simulationConfiguration);
      }

      [Observation]
      public void should_return_in_valid_results()
      {
         _validationResults.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }

      [Observation]
      public void should_set_message_for_incorrect_event_group_building_block()
      {
         _validationResults.Messages.Any(
            m =>
            m.NotificationType.Equals(NotificationType.Error) &&
            m.BuildingBlock.Equals(_eventBuildingBlock) &&
            m.Object.Equals(_applicationBuilder) &&
            m.Text.Equals(Validation.ApplicatedMoleculeNotPresent(_applicationBuilder.MoleculeName, _applicationBuilder.Name, _moleculeBuildingBlock.Name))).ShouldBeTrue();
      }
   }
}