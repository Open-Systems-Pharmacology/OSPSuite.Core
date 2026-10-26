using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.DiffBuilders
{
   public abstract class concern_for_ModuleDiffBuilder : concern_for_ObjectComparer
   {
      protected Module _module1;
      protected Module _module2;

      protected override void Context()
      {
         base.Context();
         _module1 = createModule("Module 1");
         _module2 = createModule("Module 2");
         AdjustModulesForDiff(_module1, _module2);
         _object1 = _module1;
         _object2 = _module2;
      }

      protected abstract void AdjustModulesForDiff(Module module1, Module module2);

      private static Module createModule(string name)
      {
         var module = new Module().WithName(name);
         module.PKSimVersion = "13.0";

         var molecules = new MoleculeBuildingBlock().WithName($"{name} - Molecules");
         var moleculeBuilder = new MoleculeBuilder().WithName("A");
         moleculeBuilder.DefaultStartFormula = new ConstantFormula(0);
         molecules.Add(moleculeBuilder);
         module.Add(molecules);

         var reactions = new ReactionBuildingBlock().WithName($"{name} - Reactions");
         reactions.Add(new ReactionBuilder().WithName("R1"));
         module.Add(reactions);

         module.Add(parameterValues("Parameter Values 1", value: 1));
         module.Add(parameterValues("Parameter Values 2", value: 2));

         var initialConditions = new InitialConditionsBuildingBlock().WithName("Initial Conditions");
         initialConditions.Add(initialCondition("A"));
         module.Add(initialConditions);

         return module;
      }

      protected static ParameterValuesBuildingBlock parameterValues(string name, double value)
      {
         var parameterValues = new ParameterValuesBuildingBlock().WithName(name);
         parameterValues.Add(parameterValue("P", value));
         return parameterValues;
      }

      protected static ParameterValue parameterValue(string name, double value)
      {
         var parameterValue = new ParameterValue().WithName(name);
         parameterValue.ContainerPath = new ObjectPath("Organism", "Liver");
         parameterValue.Value = value;
         return parameterValue;
      }

      private static InitialCondition initialCondition(string name)
      {
         var initialCondition = new InitialCondition().WithName(name);
         initialCondition.ContainerPath = new ObjectPath("Organism", "Liver", "Plasma");
         initialCondition.Value = 0;
         return initialCondition;
      }
   }

   public class When_comparing_two_modules_with_the_same_content_but_different_names_and_ids : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         //modules and their single building blocks are created with different names. Nothing else to adjust
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_two_modules_with_different_names_and_comparing_all_properties : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         _comparerSettings.OnlyComputingRelevant = false;
      }

      [Observation]
      public void should_report_the_module_name_and_the_names_of_the_building_blocks_matched_by_type()
      {
         //module name, molecules name and reactions name
         _report.Count.ShouldBeEqualTo(3);
      }
   }

   public class When_comparing_two_modules_with_different_merge_behavior : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module1.MergeBehavior = MergeBehavior.Overwrite;
         module2.MergeBehavior = MergeBehavior.Extend;
      }

      [Observation]
      public void should_report_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_exported_from_different_pksim_versions : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module2.PKSimVersion = "12.1";
      }

      [Observation]
      public void should_report_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_a_pksim_module_with_a_mobi_module_having_the_same_content : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module1.IsPKSimModule = true;
         module2.IsPKSimModule = false;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_a_pksim_module_with_a_mobi_module_and_comparing_all_properties : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         _comparerSettings.OnlyComputingRelevant = false;
         module1.WithName("Module");
         module2.WithName("Module");
         module1.Molecules.WithName("Molecules");
         module2.Molecules.WithName("Molecules");
         module1.Reactions.WithName("Reactions");
         module2.Reactions.WithName("Reactions");
         module1.IsPKSimModule = true;
         module2.IsPKSimModule = false;
      }

      [Observation]
      public void should_report_the_pksim_flag_and_the_icon_derived_from_it()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_modules_where_one_is_missing_a_building_block : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module2.Remove(module2.Reactions);
      }

      [Observation]
      public void should_report_the_missing_building_block()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_with_a_different_value_in_a_parameter_values_building_block : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module2.ParameterValuesCollection[0].FindByName("P").Value = 10;
      }

      [Observation]
      public void should_report_the_changed_value()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_with_parameter_values_building_blocks_having_different_names : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module2.ParameterValuesCollection[0].WithName("Renamed Parameter Values");
      }

      [Observation]
      public void should_report_the_building_block_as_missing_in_each_module()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_modules_with_a_different_molecule : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module2.Molecules.Add(new MoleculeBuilder().WithName("B"));
      }

      [Observation]
      public void should_report_the_missing_molecule()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_with_a_different_molecule_start_value_using_default_settings : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         _comparerSettings = new ComparerSettings();
         module2.Molecules.FindByName("A").DefaultStartFormula = new ConstantFormula(1.1);
      }

      [Observation]
      public void should_report_the_different_start_value()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_with_a_different_distribution_in_a_parameter_values_building_block : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         addDistributedParameterWithMean(module1, mean: 1);
         addDistributedParameterWithMean(module2, mean: 2);
      }

      private static void addDistributedParameterWithMean(Module module, double mean)
      {
         var parameterValues = module.ParameterValuesCollection[0];
         var distributedParameter = parameterValue("Volume", value: 5);
         distributedParameter.DistributionType = DistributionType.Normal;
         parameterValues.Add(distributedParameter);
         parameterValues.Add(new ParameterValue
         {
            Path = new ObjectPath("Organism", "Liver", "Volume", "Mean"),
            Value = mean
         });
      }

      [Observation]
      public void should_report_the_different_distribution_sub_parameter()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_a_module_against_a_module_with_an_additional_parameter_values_building_block_of_the_same_name : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module2.Add(parameterValues("Parameter Values 1", value: 99));
      }

      [Observation]
      public void should_report_the_additional_building_block_as_missing()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_a_module_with_an_additional_parameter_values_building_block_of_the_same_name_against_a_module : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module1.Add(parameterValues("Parameter Values 1", value: 99));
      }

      [Observation]
      public void should_report_the_additional_building_block_as_missing()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}
