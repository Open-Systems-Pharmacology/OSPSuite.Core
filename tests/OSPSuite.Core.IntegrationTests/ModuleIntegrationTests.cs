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
        var module2= _simulationConfiguration.ModuleConfigurations[1].Module;
        var lungModule2 = module2.SpatialStructure.FindByName(Lung);
        lungModule2.ShouldNotBeNull();
        lungModule2.ParentContainer.ShouldBeNull();
      }

      [Observation]
      public void should_have_removed_neighborhoods_pointing_to_invalid_neighbors()
      {
         _model.Neighborhoods.FindByName("does_not_match_existing").ShouldBeNull();
      }
   }
}