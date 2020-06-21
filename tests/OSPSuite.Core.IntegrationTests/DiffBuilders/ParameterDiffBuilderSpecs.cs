using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparing_parameters_having_the_same_formula_but_depending_on_parameters_having_different_values_and_we_are_validating_using_value_check : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var c1 = new Container {Name = "O"};
         var p11 = new Parameter {Name = "P1"}.WithParentContainer(c1);
         p11.Formula = new ConstantFormula(5);

         var p12 = new Parameter {Name = "P2"}.WithParentContainer(c1);
         p12.Formula = new ExplicitFormula("P1*5");
         p12.Formula.AddObjectPath(new FormulaUsablePath(new[] {ObjectPath.PARENT_CONTAINER, p11.Name}).WithAlias(p11.Name));


         var c2 = new Container {Name = "O"};
         var p21 = new Parameter {Name = "P1"}.WithParentContainer(c2);
         p21.Formula = new ConstantFormula(10);

         var p22 = new Parameter {Name = "P2"}.WithParentContainer(c2);
         p22.Formula = new ExplicitFormula("P1*5");
         p22.Formula.AddObjectPath(new FormulaUsablePath(new[] {ObjectPath.PARENT_CONTAINER, p21.Name}).WithAlias(p21.Name));
         _object1 = c1;
         _object2 = c2;

         _comparerSettings = new ComparerSettings {FormulaComparison = FormulaComparison.Value};
      }

      [Observation]
      public void should_have_one_report_entry_for_each_parameter()
      {
         _report.Count.ShouldBeEqualTo(2);
         _report[0].DowncastTo<PropertyValueDiffItem>().PropertyName.ShouldNotBeNull();
      }
   }

   public class When_comparing_parameters_having_the_same_value_but_using_different_display_units_and_the_comparison_only_compare_relevant_properties : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var dimension = DomainHelperForSpecs.LengthDimensionForSpecs();
         var c1 = new Container {Name = "O"};
         var p11 = new Parameter {Name = "P1"}.WithParentContainer(c1);
         p11.DisplayUnit = dimension.Units.First();
         p11.Formula = new ConstantFormula(5);


         var c2 = new Container {Name = "O"};
         var p21 = new Parameter {Name = "P1"}.WithParentContainer(c2);
         p21.DisplayUnit = dimension.Units.Last();
         p21.Formula = new ConstantFormula(5);

         _object1 = c1;
         _object2 = c2;

         _comparerSettings = new ComparerSettings {FormulaComparison = FormulaComparison.Value, OnlyComputingRelevant = true};
      }

      [Observation]
      public void should_not_show_any_difference()
      {
         _report.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_comparing_parameters_having_the_same_value_but_using_different_display_units_and_the_comparison_compares_everything : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var dimension = DomainHelperForSpecs.LengthDimensionForSpecs();
         var c1 = new Container {Name = "O"};
         var p11 = new Parameter {Name = "P1"}.WithParentContainer(c1);
         p11.DisplayUnit = dimension.Units.First();
         p11.Formula = new ConstantFormula(5);


         var c2 = new Container {Name = "O"};
         var p21 = new Parameter {Name = "P1"}.WithParentContainer(c2);
         p21.DisplayUnit = dimension.Units.Last();
         p21.Formula = new ConstantFormula(5);

         _object1 = c1;
         _object2 = c2;

         _comparerSettings = new ComparerSettings {FormulaComparison = FormulaComparison.Value, OnlyComputingRelevant = false};
      }

      [Observation]
      public void should_have_some_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_parameters_having_the_same_value_but_using_different_value_origin_and_the_comparison_compares_everything : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var c1 = new Container { Name = "O" };
         var p11 = new Parameter { Name = "P1" }.WithParentContainer(c1);
         p11.Formula = new ConstantFormula(5);
         p11.ValueOrigin.Description = "HELLO";


         var c2 = new Container { Name = "O" };
         var p21 = new Parameter { Name = "P1" }.WithParentContainer(c2);
         p21.Formula = new ConstantFormula(5);
         p21.ValueOrigin.Description = "HELLO-2";

         _object1 = c1;
         _object2 = c2;

         _comparerSettings = new ComparerSettings { FormulaComparison = FormulaComparison.Value, OnlyComputingRelevant = false };
      }

      [Observation]
      public void should_have_some_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }


   public class When_comparing_parameters_having_the_different_base_values_but_the_same_display_value : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var dimension = DomainHelperForSpecs.LengthDimensionForSpecs();
         var c1 = new Container {Name = "O"};
         var p11 = new Parameter {Name = "P1"}.WithParentContainer(c1);
         p11.DisplayUnit = dimension.Units.First();
         p11.Formula = new ConstantFormula(1);
         NumericFormatterOptions.Instance.DecimalPlace = 3;

         var c2 = new Container {Name = "O"};
         var p21 = new Parameter {Name = "P1"}.WithParentContainer(c2);
         p21.DisplayUnit = dimension.Units.Last();
         p21.Formula = new ConstantFormula(0.001);

         _object1 = c1;
         _object2 = c2;

         _comparerSettings = new ComparerSettings {FormulaComparison = FormulaComparison.Value, OnlyComputingRelevant = true};
      }

      [Observation]
      public void should_have_some_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
         var propertyDiffItem = _report[0].DowncastTo<PropertyValueDiffItem>();
         propertyDiffItem.FormattedValue1.ShouldBeEqualTo("1.000 m");
         propertyDiffItem.FormattedValue2.ShouldBeEqualTo("1.000 mm");
      }
   }

   public class When_comparing_parameters_having_the_same_formula_but_depending_on_parameters_having_different_values_and_we_are_validating_using_formula_check : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var c1 = new Container {Name = "O"};
         var p11 = new Parameter {Name = "P1"}.WithParentContainer(c1);
         p11.Formula = new ConstantFormula(5);

         var p12 = new Parameter {Name = "P2"}.WithParentContainer(c1);
         p12.Formula = new ExplicitFormula("P1*5");
         p12.Formula.AddObjectPath(new FormulaUsablePath(new[] {ObjectPath.PARENT_CONTAINER, p11.Name}).WithAlias(p11.Name));


         var c2 = new Container {Name = "O"};
         var p21 = new Parameter {Name = "P1"}.WithParentContainer(c2);
         p21.Formula = new ConstantFormula(10);

         var p22 = new Parameter {Name = "P2"}.WithParentContainer(c2);
         p22.Formula = new ExplicitFormula("P1*5");
         p22.Formula.AddObjectPath(new FormulaUsablePath(new[] {ObjectPath.PARENT_CONTAINER, p21.Name}).WithAlias(p21.Name));
         _object1 = c1;
         _object2 = c2;

         _comparerSettings.FormulaComparison = FormulaComparison.Formula;
      }

      [Observation]
      public void should_have_one_report_entry_only_for_the_parameter_whose_value_was_set()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_containers_having_different_distributed_parameters : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var distributedFormulaFactory = IoC.Resolve<IDistributionFormulaFactory>();
         var c1 = new Container {Name = "O"};
         var p11 = new DistributedParameter() {Name = "P1"}.WithParentContainer(c1);
         var meanP11 = new Parameter().WithFormula(new ConstantFormula(1)).WithName(Constants.Distribution.MEAN).WithParentContainer(p11);
         var devP11 = new Parameter().WithFormula(new ConstantFormula(2)).WithName(Constants.Distribution.DEVIATION).WithParentContainer(p11);
         var percentile11 = new Parameter().WithFormula(new ConstantFormula(0.5)).WithName(Constants.Distribution.PERCENTILE).WithParentContainer(p11);
         p11.Formula = distributedFormulaFactory.CreateNormalDistributionFormulaFor(p11, meanP11, devP11);

         var c2 = new Container {Name = "O"};
         var p21 = new DistributedParameter() {Name = "P1"}.WithParentContainer(c2);
         var meanP21 = new Parameter().WithFormula(new ConstantFormula(1)).WithName(Constants.Distribution.MEAN).WithParentContainer(p21);
         var devP21 = new Parameter().WithFormula(new ConstantFormula(2)).WithName(Constants.Distribution.GEOMETRIC_DEVIATION).WithParentContainer(p21);
         var percentile21 = new Parameter().WithFormula(new ConstantFormula(0.5)).WithName(Constants.Distribution.PERCENTILE).WithParentContainer(p21);
         p21.Formula = distributedFormulaFactory.CreateLogNormalDistributionFormulaFor(p21, meanP21, devP21);

         _object1 = c1;
         _object2 = c2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_containers_having_different_distributed_parameters_in_formula_mode_with_one_parameter_fixed_by_the_user : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var distributedFormulaFactory = IoC.Resolve<IDistributionFormulaFactory>();
         var c1 = new Container { Name = "O" };
         var p11 = new DistributedParameter() { Name = "P1" }.WithParentContainer(c1);
         var meanP11 = new Parameter().WithFormula(new ConstantFormula(1)).WithName(Constants.Distribution.MEAN).WithParentContainer(p11);
         var devP11 = new Parameter().WithFormula(new ConstantFormula(2)).WithName(Constants.Distribution.DEVIATION).WithParentContainer(p11);
         var percentile11 = new Parameter().WithFormula(new ConstantFormula(0.5)).WithName(Constants.Distribution.PERCENTILE).WithParentContainer(p11);
         p11.Formula = distributedFormulaFactory.CreateNormalDistributionFormulaFor(p11, meanP11, devP11);

         var c2 = new Container { Name = "O" };
         var p21 = new DistributedParameter() { Name = "P1" }.WithParentContainer(c2);
         var meanP21 = new Parameter().WithFormula(new ConstantFormula(1)).WithName(Constants.Distribution.MEAN).WithParentContainer(p21);
         var devP21 = new Parameter().WithFormula(new ConstantFormula(2)).WithName(Constants.Distribution.GEOMETRIC_DEVIATION).WithParentContainer(p21);
         var percentile21 = new Parameter().WithFormula(new ConstantFormula(0.5)).WithName(Constants.Distribution.PERCENTILE).WithParentContainer(p21);
         p21.Formula = distributedFormulaFactory.CreateNormalDistributionFormulaFor(p21, meanP21, devP21);

         _object1 = c1;
         _object2 = c2;

         _comparerSettings.FormulaComparison = FormulaComparison.Formula;
         p11.Value = 5;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_parameters_having_the_different_rhs_formula : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var c1 = new Container { Name = "O" };
         var p11 = new Parameter { Name = "P1" }.WithParentContainer(c1);
         p11.Formula = new ConstantFormula(5);
         p11.RHSFormula = new ConstantFormula(-1);

         


         var c2 = new Container { Name = "O" };
         var p21 = new Parameter { Name = "P1" }.WithParentContainer(c2);
         p21.Formula = new ConstantFormula(5);
         p21.RHSFormula = new ConstantFormula(1);
         
         _object1 = c1;
         _object2 = c2;

         _comparerSettings.FormulaComparison = FormulaComparison.Formula;
      }

      [Observation]
      public void should_have_one_report_entry_only_for_the_parameter_whose_value_was_set()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

}