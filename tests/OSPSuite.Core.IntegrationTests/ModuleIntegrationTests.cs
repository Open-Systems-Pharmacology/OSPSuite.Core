using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_ModuleIntegration : ContextForIntegration<IModelConstructor>
   {
      protected SimulationConfiguration _simulationConfiguration;
      protected CreationResult _result;
      protected IModel _model;

      protected string _modelName = "MyModel";

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationConfiguration = IoC.Resolve<ModuleHelperForSpecs>().CreateSimulationConfiguration();
         //Freeze in constructor in case access is also required to properties for setup
         _simulationConfiguration.Freeze();
      }

      protected override void Because()
      {
         sut = IoC.Resolve<IModelConstructor>();
         _result = sut.CreateModelFrom(_simulationConfiguration, _modelName);
         //Freeze in the test so that we can access the properties. This needs to be done AFTER the model creation 
         _simulationConfiguration.Freeze();
         _model = _result.Model;
      }
   }

   public class When_running_the_case_study_for_module_integration : concern_for_ModuleIntegration
   {
      [Observation]
      public void should_return_a_successful_validation()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Valid, _result.ValidationResult.Messages.Select(m => m.Text).ToString("\n"));
      }
   }
}