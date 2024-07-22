using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Helpers.ConstantsForSpecs;

namespace OSPSuite.Core
{
   internal abstract class concern_for_ModuleIntegration : ContextForIntegration<IModelConstructor>
   {
      protected CreationResult _result;
      protected IModel _model;

      protected string _modelName = "MyModel";
      protected SimulationBuilder _simulationBuilder;
      protected SimulationConfiguration _simulationConfiguration;

      protected abstract Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder();

      protected override void Because()
      {
         sut = IoC.Resolve<IModelConstructor>();
         var moduleHelper = IoC.Resolve<ModuleHelperForSpecs>();
         _simulationConfiguration = SimulationConfigurationBuilder()(moduleHelper);
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
         _result = sut.CreateModelFrom(_simulationConfiguration, _modelName);
         _model = _result.Model;
      }
   }

   internal class When_running_the_case_study_for_module_integration : concern_for_ModuleIntegration
   {
      [Observation]
      public void should_return_a_successful_validation()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Valid, _result.ValidationResult.Messages.Select(m => m.Text).ToString("\n"));
      }

      [Observation]
      public void should_have_added_a_tumor_organ_in_heart_organ()
      {
         var tumor = _model.ModelOrganCompartment(Heart, Tumor);
         tumor.ShouldNotBeNull();

         var tumor_pls = tumor.Container(Plasma);
         tumor_pls.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_left_the_original_container_of_the_spatial_structure_intact()
      {
         var module2 = _simulationConfiguration.ModuleConfigurations[1].Module;
         var lungModule2 = module2.SpatialStructure.FindByName(Lung);
         lungModule2.ShouldNotBeNull();
         lungModule2.ParentContainer.ShouldBeNull();
      }

      [Observation]
      public void should_have_removed_neighborhoods_pointing_to_invalid_neighbors()
      {
         _model.Neighborhoods.FindByName("does_not_match_existing").ShouldBeNull();
      }

      [Observation]
      public void should_have_merged_the_existing_parameters()
      {
         var moleculeAGlobalContainer = _model.Root.Container("A");
         moleculeAGlobalContainer.ShouldNotBeNull();

         moleculeAGlobalContainer.Parameter("P1").Value.ShouldBeEqualTo(100);
         moleculeAGlobalContainer.Parameter("P2").Value.ShouldBeEqualTo(20);
         moleculeAGlobalContainer.Parameter("P3").Value.ShouldBeEqualTo(30);
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfiguration();
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_override_to_extend : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehaviorOverridingModuleBehavior();

      [Observation]
      public void should_have_added_the_missing_parameters_to_lung()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P2).Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_have_merged_the_tag_at_the_container_level()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Tags.Contains("Tag1").ShouldBeTrue();
         lung.Tags.Contains("Tag2").ShouldBeTrue();
      }

      [Observation]
      public void should_have_updated_the_container_mode_to_the_one_of_the_merged_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Mode.ShouldBeEqualTo(ContainerMode.Physical);
      }

      [Observation]
      public void should_have_not_merged_the_tag_at_the_parameter_level()
      {
         var lungQ = _model.Root.EntityAt<Parameter>(Constants.ORGANISM, Lung, "Q");
         lungQ.Tags.Contains("ParamTag1").ShouldBeFalse();
         lungQ.Tags.Contains("ParamTag2").ShouldBeTrue();
      }

      [Observation]
      public void should_have_merged_the_tag_at_the_neighborhood_level()
      {
         var lng_pls_to_lng_cell = _model.Root.EntityAt<Neighborhood>(Constants.NEIGHBORHOODS, "lng_pls_to_lng_cell");
         var tags = lng_pls_to_lng_cell.Tags.Select(x => x.Value).ToString(", ");

         lng_pls_to_lng_cell.Tags.Contains("NeighborhoodTag1").ShouldBeTrue(tags);
         lng_pls_to_lng_cell.Tags.Contains("NeighborhoodTag2").ShouldBeTrue(tags);
      }

      [Observation]
      public void should_have_updating_existing_parameters()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(Q).Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_have_kept_parameters_defined_in_the_source_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P).Value.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_have_added_the_existing_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Container(Interstitial).ShouldNotBeNull();
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_extend : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehavior();

      [Observation]
      public void should_have_added_the_missing_parameters_to_lung()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P2).Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_have_updating_existing_parameters()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(Q).Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_have_kept_parameters_defined_in_the_source_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P).Value.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_have_added_the_existing_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Container(Interstitial).ShouldNotBeNull();
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_extend_and_recursive_container : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehaviorWithRecursiveContainers();

      [Observation]
      public void should_have_added_the_missing_container_to_lung_and_arterial_blood()
      {
         var lngInt = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung, Interstitial);
         lngInt.ShouldNotBeNull();

         var artInt = _model.Root.EntityAt<Container>(Constants.ORGANISM, ArterialBlood, Interstitial);
         artInt.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_updating_existing_parameters()
      {
         var lungPls = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung, Plasma);
         lungPls.Parameter(Volume).Value.ShouldBeEqualTo(20);
         lungPls.Parameter(Q).Value.ShouldBeEqualTo(21);
      }

      [Observation]
      public void should_have_kept_parameters_defined_in_the_source_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P).Value.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_have_kept_existing_container()
      {
         var lungCell = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung, Cell);
         lungCell.ShouldNotBeNull();
      }
   }
}