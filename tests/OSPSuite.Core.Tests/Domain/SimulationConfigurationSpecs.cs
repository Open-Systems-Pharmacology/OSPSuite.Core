using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SimulationConfiguration : ContextSpecification<SimulationConfiguration>
   {

   }

   public class When_cloning_a_simulation_configuration : concern_for_SimulationConfiguration
   {
      private ICloneManager _cloneManager;
      private IDataRepositoryTask _dataRepositoryTask;
      private SimulationConfiguration _result;

      protected override void Context()
      {
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         _cloneManager = new CloneManagerForBuildingBlock(new ObjectBaseFactoryForSpecs(new DimensionFactoryForIntegrationTests()), _dataRepositoryTask);
         sut = new SimulationConfiguration();
         var module = new Module
         {
            Name = "module"
         };
         module.Add(new SpatialStructure().WithName("spatial structure"));
         module.Add(new InitialConditionsBuildingBlock());
         module.Add(new ParameterValuesBuildingBlock());
         sut.AddModuleConfiguration(new ModuleConfiguration(module));

         sut.PerformCircularReferenceCheck = !sut.PerformCircularReferenceCheck;
         sut.ShowProgress = !sut.ShowProgress;
         sut.ShouldValidate = !sut.ShouldValidate;
         sut.SimModelExportMode = SimModelExportMode.Full;

         sut.SimulationSettings = new SimulationSettings();
      }

      protected override void Because()
      {
         _result = _cloneManager.Clone(sut);
      }

      [Observation]
      public void the_cloned_configuration_should_have_clones_of_module_configurations_and_modules()
      {
         _result.ModuleConfigurations.Count.ShouldBeEqualTo(1);
         _result.ModuleConfigurations[0].ShouldNotBeEqualTo(sut.ModuleConfigurations[0]);
         _result.ModuleConfigurations[0].Module.Name.ShouldBeEqualTo(sut.ModuleConfigurations[0].Module.Name);
         _result.ModuleConfigurations[0].Module.SpatialStructure.ShouldNotBeEqualTo(sut.ModuleConfigurations[0].Module.SpatialStructure);
         _result.ModuleConfigurations[0].Module.SpatialStructure.Name.ShouldBeEqualTo(sut.ModuleConfigurations[0].Module.SpatialStructure.Name);
      }

      [Observation]
      public void properties_should_match()
      {
         _result.PerformCircularReferenceCheck.ShouldBeEqualTo(sut.PerformCircularReferenceCheck);
         _result.ShowProgress.ShouldBeEqualTo(sut.ShowProgress);
         _result.ShouldValidate.ShouldBeEqualTo(sut.ShouldValidate);
         _result.SimModelExportMode.ShouldBeEqualTo(sut.SimModelExportMode);
      }
   }
}