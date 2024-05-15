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
      protected SimulationConfiguration _simulationConfiguration;
      protected CreationResult _result;
      protected IModel _model;

      protected string _modelName = "MyModel";
      protected SimulationBuilder _simulationBuilder;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationConfiguration = IoC.Resolve<ModuleHelperForSpecs>().CreateSimulationConfiguration();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
      }

      protected override void Because()
      {
         sut = IoC.Resolve<IModelConstructor>();
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
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_extend : concern_for_ModuleIntegration
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationConfiguration = IoC.Resolve<ModuleHelperForSpecs>().CreateSimulationConfigurationForExtendMergeBehavior();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
      }

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
      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationConfiguration = IoC.Resolve<ModuleHelperForSpecs>().CreateSimulationConfigurationForExtendMergeBehaviorWithRecursiveContainers();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
      }

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