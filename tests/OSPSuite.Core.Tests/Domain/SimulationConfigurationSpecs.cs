using System.Collections.Generic;
using System.Linq;
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

         sut.AddCalculationMethodsOverridesFor("moleculeName", new List<UsedCalculationMethod> { new("category", "method") });

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

      [Observation]
      public void the_calculation_method_overrides_should_be_present_and_clones()
      {
         _result.AllCalculationMethodOverrides.Count.ShouldBeEqualTo(1);
         _result.CalculationMethodOverridesFor("moleculeName").UsedCalculationMethods.Count.ShouldBeEqualTo(1);
         ReferenceEquals(_result.CalculationMethodOverridesFor("moleculeName").UsedCalculationMethods.First(), sut.CalculationMethodOverridesFor("moleculeName").UsedCalculationMethods.First()).ShouldNotBeEqualTo(true);
      }
   }

   public class When_adding_duplicate_category_molecule_calculation_methods_to_a_simulation_configuration : concern_for_SimulationConfiguration
   {
      private const string MoleculeName = "molecule";
      private UsedCalculationMethod _usedCalculationMethod1;
      private UsedCalculationMethod _usedCalculationMethod2;

      protected override void Context()
      {
         sut = new SimulationConfiguration();
         _usedCalculationMethod1 = new UsedCalculationMethod { Category = "category1", CalculationMethod = "method1" };
         _usedCalculationMethod2 = new UsedCalculationMethod { Category = "category1", CalculationMethod = "method2" };
      }

      protected override void Because()
      {
         sut.AddCalculationMethodsOverridesFor(MoleculeName, [_usedCalculationMethod1, _usedCalculationMethod2]);
      }

      [Observation]
      public void the_first_method_should_be_overwritten_by_the_second()
      {
         var moleculeUsedCalculationMethods = sut.CalculationMethodOverridesFor(MoleculeName);
         moleculeUsedCalculationMethods.UsedCalculationMethods.Count.ShouldBeEqualTo(1);
         moleculeUsedCalculationMethods.UsedCalculationMethods.ShouldNotContain(_usedCalculationMethod1);
         moleculeUsedCalculationMethods.UsedCalculationMethods.ShouldContain(_usedCalculationMethod2);
      }
   }

   public class When_getting_calculation_methods_for_non_configured_molecule : concern_for_SimulationConfiguration
   {
      private const string MoleculeName = "molecule";
      private UsedCalculationMethod _usedCalculationMethod1;
      private UsedCalculationMethod _usedCalculationMethod2;

      protected override void Context()
      {
         sut = new SimulationConfiguration();
         _usedCalculationMethod1 = new UsedCalculationMethod { Category = "category1", CalculationMethod = "method1" };
         _usedCalculationMethod2 = new UsedCalculationMethod { Category = "category1", CalculationMethod = "method2" };
      }

      protected override void Because()
      {
         sut.AddCalculationMethodsOverridesFor(MoleculeName, [_usedCalculationMethod1, _usedCalculationMethod2]);
      }

      [Observation]
      public void the_overrides_list_should_be_empty()
      {
         var moleculeUsedCalculationMethods = sut.CalculationMethodOverridesFor("SomethingElse");
         moleculeUsedCalculationMethods.UsedCalculationMethods.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_adding_molecule_calculation_methods_to_a_simulation_configuration : concern_for_SimulationConfiguration
   {
      private const string MoleculeName = "molecule";
      private UsedCalculationMethod _usedCalculationMethod1;
      private UsedCalculationMethod _usedCalculationMethod2;

      protected override void Context()
      {
         sut = new SimulationConfiguration();
         _usedCalculationMethod1 = new UsedCalculationMethod { Category = "category1" };
         _usedCalculationMethod2 = new UsedCalculationMethod { Category = "category2" };
      }

      protected override void Because()
      {
         sut.AddCalculationMethodsOverridesFor(MoleculeName, [_usedCalculationMethod1, _usedCalculationMethod2]);
      }

      [Observation]
      public void the_calculation_methods_should_be_added_to_the_molecule_used_calculation_methods_cache()
      {
         var moleculeUsedCalculationMethods = sut.CalculationMethodOverridesFor(MoleculeName);
         moleculeUsedCalculationMethods.UsedCalculationMethods.Count.ShouldBeEqualTo(2);
         moleculeUsedCalculationMethods.UsedCalculationMethods.ShouldContain(_usedCalculationMethod1);
         moleculeUsedCalculationMethods.UsedCalculationMethods.ShouldContain(_usedCalculationMethod2);
      }
   }
}