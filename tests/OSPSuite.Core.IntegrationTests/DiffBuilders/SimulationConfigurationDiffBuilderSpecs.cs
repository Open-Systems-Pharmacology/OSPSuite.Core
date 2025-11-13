using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.DiffBuilders
{
   internal abstract class concern_for_SimulationSettingsDiffBuilder : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var configurationCreator = IoC.Resolve<ModelHelperForSpecs>();

         var simulationConfiguration1 = configurationCreator.CreateSimulationConfiguration();
         var simulationConfiguration2 = configurationCreator.CreateSimulationConfiguration();

         AdjustSimulationConfigurationsForDiff(simulationConfiguration1, simulationConfiguration2);

         _object1 = simulationConfiguration1;
         _object2 = simulationConfiguration2;
      }

      protected abstract void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2);
   }

   internal class When_comparing_similar_simulation_configuration : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         // No adjustments compare identical configurations
      }

      [Observation]
      public void the_report_should_not_contain_any_differences()
      {
         _report.Count.ShouldBeEqualTo(0);
      }
   }

   internal class When_comparing_configurations_with_different_individuals : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration1.Individual.WithName("individual");
         simulationConfiguration2.Individual.WithName("individual2");
      }

      [Observation]
      public void the_report_should_have_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_comparing_configurations_with_different_module_count : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration1.AddModuleConfiguration(new ModuleConfiguration(new Module()));
      }

      [Observation]
      public void the_report_should_have_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_comparing_configurations_with_different_modules : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration1.AddModuleConfiguration(new ModuleConfiguration(new Module().WithName("module1")));
         simulationConfiguration2.AddModuleConfiguration(new ModuleConfiguration(new Module().WithName("module2")));
      }

      [Observation]
      public void the_report_should_have_differences()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   internal class When_comparing_configurations_with_same_module_but_different_selected_initial_conditions : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         var module = new Module();
         var initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();
         module.Add(initialConditionsBuildingBlock);

         simulationConfiguration1.AddModuleConfiguration(new ModuleConfiguration(module) { SelectedInitialConditions = initialConditionsBuildingBlock });
         simulationConfiguration2.AddModuleConfiguration(new ModuleConfiguration(new Module()));
      }

      [Observation]
      public void the_report_should_have_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_comparing_configurations_with_same_module_but_different_selected_parameter_values : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         var module = new Module();
         var parameterValues = new ParameterValuesBuildingBlock();
         module.Add(parameterValues);

         simulationConfiguration1.AddModuleConfiguration(new ModuleConfiguration(module) { SelectedParameterValues = parameterValues });
         simulationConfiguration2.AddModuleConfiguration(new ModuleConfiguration(new Module()));
      }

      [Observation]
      public void the_report_should_have_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_comparing_configurations_with_different_expression_profiles : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration1.AddExpressionProfile(new ExpressionProfileBuildingBlock().WithName("expression|profile|1"));
         simulationConfiguration2.AddExpressionProfile(new ExpressionProfileBuildingBlock().WithName("expression|profile|2"));
      }

      [Observation]
      public void the_report_should_report_difference()
      {
         // absent and present x 2
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   internal class When_comparing_configurations_with_different_expression_profile_count : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration2.AddExpressionProfile(new ExpressionProfileBuildingBlock().WithName("expression|profile|2"));
      }

      [Observation]
      public void the_report_should_have_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_comparing_configurations_with_different_export_mode : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration2.SimModelExportMode = SimModelExportMode.Optimized;
      }

      [Observation]
      public void the_report_should_have_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_comparing_configurations_with_different_process_rate_creation : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration2.CreateAllProcessRateParameters = true;
      }

      [Observation]
      public void the_report_should_have_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_comparing_configurations_with_different_circular_reference_check : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration2.PerformCircularReferenceCheck = false;
      }

      [Observation]
      public void the_report_should_have_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_comparing_configurations_with_different_should_validate : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration2.ShouldValidate = false;
      }

      [Observation]
      public void the_report_should_have_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_comparing_configurations_with_different_show_progress : concern_for_SimulationSettingsDiffBuilder
   {
      protected override void AdjustSimulationConfigurationsForDiff(SimulationConfiguration simulationConfiguration1, SimulationConfiguration simulationConfiguration2)
      {
         simulationConfiguration2.ShowProgress = false;
      }

      [Observation]
      public void the_report_should_have_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}
