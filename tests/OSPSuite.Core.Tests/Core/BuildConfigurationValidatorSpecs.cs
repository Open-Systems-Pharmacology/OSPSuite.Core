using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_BuildConfigurationValidatorSpecs : ContextSpecification<IBuildConfigurationValidator>
   {
      protected override void Context()
      {
         sut = new BuildConfigurationValidator();
      }
   }

   internal class When_validating_a_build_configuration_with_an_incorrect_application : concern_for_BuildConfigurationValidatorSpecs
   {
      private IBuildConfiguration _buildConfiguration;
      private ValidationResult _validationResults;
      private IEventGroupBuildingBlock _eventBuildingBlock;
      private IApplicationBuilder _applicationBuilder;
      private IMoleculeBuildingBlock _molceuleBuidingBlock;

      protected override void Context()
      {
         base.Context();
         _eventBuildingBlock = new EventGroupBuildingBlock().WithName("Events");
         _applicationBuilder = new ApplicationBuilder().WithName("Tada");
         _applicationBuilder.MoleculeName = "B";
         _eventBuildingBlock.Add(_applicationBuilder);
         _molceuleBuidingBlock = new MoleculeBuildingBlock().WithName("Molecules");
         _molceuleBuidingBlock.Add(new MoleculeBuilder().WithName("A"));
         _buildConfiguration = new BuildConfiguration();
         _buildConfiguration.Molecules = _molceuleBuidingBlock;
         _buildConfiguration.MoleculeStartValues = new MoleculeStartValuesBuildingBlock();
         _buildConfiguration.Observers = new ObserverBuildingBlock();
         _buildConfiguration.ParameterStartValues = new ParameterStartValuesBuildingBlock();
         _buildConfiguration.PassiveTransports = new PassiveTransportBuildingBlock();
         _buildConfiguration.Reactions = new ReactionBuildingBlock();
         _buildConfiguration.SpatialStructure = new SpatialStructure();
         _buildConfiguration.EventGroups = _eventBuildingBlock;
      }

      protected override void Because()
      {
         _validationResults = sut.Validate(_buildConfiguration);
      }

      [Observation]
      public void should_return_in_valide_results()
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
            m.Text.Equals(Validation.ApplicatedMoleculeNotPresent(_applicationBuilder.MoleculeName, _applicationBuilder.Name, _molceuleBuidingBlock.Name))).ShouldBeTrue();
      }
   }
}