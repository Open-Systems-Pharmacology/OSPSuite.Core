using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

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

   public class When_comparing_two_modules_where_only_one_overrides_a_distribution_sub_parameter : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         _comparerSettings = new ComparerSettings();
         addDistributedParameter(module1);
         addDistributedParameter(module2);
         module2.ParameterValuesCollection[0].Add(new ParameterValue
         {
            Path = new ObjectPath("Organism", "Liver", "Volume", "Mean"),
            Value = 99
         });
      }

      private static void addDistributedParameter(Module module)
      {
         var distributedParameter = parameterValue("Volume", value: 5);
         distributedParameter.DistributionType = DistributionType.Normal;
         module.ParameterValuesCollection[0].Add(distributedParameter);
      }

      [Observation]
      public void should_report_the_one_sided_override()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_with_different_constant_reaction_kinetics_using_default_settings : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         _comparerSettings = new ComparerSettings();
         module1.Reactions.FindByName("R1").Formula = new ConstantFormula(1);
         module2.Reactions.FindByName("R1").Formula = new ConstantFormula(99);
      }

      [Observation]
      public void should_report_the_different_kinetic()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_with_different_process_rate_parameter_persistence : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module1.Reactions.FindByName("R1").CreateProcessRateParameter = true;
         module1.Reactions.FindByName("R1").ProcessRateParameterPersistable = true;
         module2.Reactions.FindByName("R1").CreateProcessRateParameter = true;
         module2.Reactions.FindByName("R1").ProcessRateParameterPersistable = false;
      }

      [Observation]
      public void should_report_the_different_persistence()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public abstract class concern_for_ModuleDiffBuilder_with_event_groups : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         AdjustEventGroupsForDiff(addEventGroupTo(module1), addEventGroupTo(module2));
      }

      protected abstract void AdjustEventGroupsForDiff(EventGroupBuilder eventGroup1, EventGroupBuilder eventGroup2);

      private static EventGroupBuilder addEventGroupTo(Module module)
      {
         var eventGroup = new EventGroupBuilder().WithName("Application");
         eventGroup.EventGroupType = "Application";
         var eventBuilder = new EventBuilder().WithName("Start");
         eventBuilder.Dimension = Constants.Dimension.NO_DIMENSION;
         eventGroup.Add(eventBuilder);

         var eventGroups = new EventGroupBuildingBlock().WithName($"{module.Name} - Events");
         eventGroups.Add(eventGroup);
         module.Add(eventGroups);
         return eventGroup;
      }
   }

   public class When_comparing_two_modules_with_different_event_group_types : concern_for_ModuleDiffBuilder_with_event_groups
   {
      protected override void AdjustEventGroupsForDiff(EventGroupBuilder eventGroup1, EventGroupBuilder eventGroup2)
      {
         eventGroup2.EventGroupType = "Formulation";
      }

      [Observation]
      public void should_report_the_different_event_group_type()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_with_different_event_dimensions : concern_for_ModuleDiffBuilder_with_event_groups
   {
      protected override void AdjustEventGroupsForDiff(EventGroupBuilder eventGroup1, EventGroupBuilder eventGroup2)
      {
         eventGroup2.GetSingleChildByName<EventBuilder>("Start").Dimension = DomainHelperForSpecs.TimeDimensionForSpecs();
      }

      [Observation]
      public void should_report_the_different_event_dimension()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_whose_spatial_structures_differ_only_in_a_distribution_deviation : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         module1.Add(spatialStructureWithDistributedParameter(module1.Name, deviation: 2));
         module2.Add(spatialStructureWithDistributedParameter(module2.Name, deviation: 3));
      }

      private static SpatialStructure spatialStructureWithDistributedParameter(string moduleName, double deviation)
      {
         var distributedFormulaFactory = IoC.Resolve<IDistributionFormulaFactory>();
         var organism = new Container().WithName("Organism");
         var parameter = new DistributedParameter().WithName("Volume").WithParentContainer(organism);
         var mean = new Parameter().WithFormula(new ConstantFormula(1)).WithName(Constants.Distribution.MEAN).WithParentContainer(parameter);
         var dev = new Parameter().WithFormula(new ConstantFormula(deviation)).WithName(Constants.Distribution.DEVIATION).WithParentContainer(parameter);
         new Parameter().WithFormula(new ConstantFormula(0.5)).WithName(Constants.Distribution.PERCENTILE).WithParentContainer(parameter);
         parameter.Formula = distributedFormulaFactory.CreateNormalDistributionFormulaFor(parameter, mean, dev);

         var spatialStructure = new SpatialStructure().WithName($"{moduleName} - Spatial Structure");
         spatialStructure.AddTopContainer(organism);
         return spatialStructure;
      }

      [Observation]
      public void should_report_the_different_deviation()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_modules_where_a_molecule_is_xenobiotic_in_only_one : concern_for_ModuleDiffBuilder
   {
      protected override void AdjustModulesForDiff(Module module1, Module module2)
      {
         _comparerSettings = new ComparerSettings();
         module2.Molecules.FindByName("A").IsXenobiotic = false;
      }

      [Observation]
      public void should_report_the_difference()
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
