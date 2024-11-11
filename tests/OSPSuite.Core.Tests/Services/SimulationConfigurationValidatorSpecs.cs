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

   internal class When_validating_a_simulation_configuration_with_colliding_neighborhoods : concern_for_SimulationConfigurationValidator
   {
      private SimulationConfiguration _simulationConfiguration;
      private ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(moduleWithNeighborhood("name1")));
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(moduleWithNeighborhood("name2")));
      }

      private Module moduleWithNeighborhood(string neighborhoodName)
      {
         var spatialStructure = new SpatialStructure { NeighborhoodsContainer = new Container() };
         spatialStructure.AddNeighborhood(new NeighborhoodBuilder { FirstNeighborPath = new ObjectPath("path1"), SecondNeighborPath = new ObjectPath("path2") }.WithName(neighborhoodName));

         return new Module { spatialStructure };
      }

      protected override void Because()
      {
         _result = sut.Validate(_simulationConfiguration);
      }

      [Observation]
      public void the_result_should_be_invalid()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }

      [Observation]
      public void the_error_should_only_be_reported_once_instead_of_for_both_neighborhoods()
      {
         _result.Messages.Count().ShouldBeEqualTo(1);
      }
   }

   internal class When_validating_a_simulation_configuration_with_an_incorrect_application : concern_for_SimulationConfigurationValidator
   {
      private SimulationConfiguration _simulationConfiguration;
      private ValidationResult _validationResults;
      private EventGroupBuildingBlock _eventBuildingBlock;
      private ApplicationBuilder _applicationBuilder;
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

         var moleculeModule = new Module
         {
            _moleculeBuildingBlock,
            new ObserverBuildingBlock(),
            new PassiveTransportBuildingBlock(),
            new ReactionBuildingBlock(),
            new SpatialStructure(),
            new InitialConditionsBuildingBlock(),
            new ParameterValuesBuildingBlock()
         };

         var eventModule = new Module
         {
            new ObserverBuildingBlock(),
            new PassiveTransportBuildingBlock(),
            new ReactionBuildingBlock(),
            new SpatialStructure(),
            _eventBuildingBlock,
            new InitialConditionsBuildingBlock(),
            new ParameterValuesBuildingBlock()
         };

         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(moleculeModule));
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(eventModule));

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
            m.Text.Equals(Validation.ApplicatedMoleculeNotPresent(_applicationBuilder.MoleculeName, _applicationBuilder.Name))).ShouldBeTrue();
      }
   }
}