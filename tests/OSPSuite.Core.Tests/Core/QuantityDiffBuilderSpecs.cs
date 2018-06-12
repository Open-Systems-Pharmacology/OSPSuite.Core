using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   public class When_comparing_to_quantities_with_one_of_which_having_a_formula_that_cannot_be_evaluated_ : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var exp1 = new ExplicitFormula("5*P1");
         var exp2 = new ExplicitFormula("5*P1");
         exp1.AddObjectPath(new FormulaUsablePath(new string[] {ObjectPath.PARENT_CONTAINER, "P1"}).WithAlias("P1"));
         exp2.AddObjectPath(new FormulaUsablePath(new string[] {ObjectPath.PARENT_CONTAINER, "P1"}).WithAlias("P1"));
         _object1 = new Parameter {BuildMode = ParameterBuildMode.Local}.WithFormula(exp1);
         _object2 = new Parameter {BuildMode = ParameterBuildMode.Global}.WithFormula(exp2);
         _comparerSettings = new ComparerSettings {FormulaComparison = FormulaComparison.Value};
      }

      [Observation]
      public void should_have_been_able_to_create_a_report_containing_only_the_differences_in_the_two_parameters()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_a_parameter_whose_formula_was_overriden_with_a_value_and_comparing_only_formula_change : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("5*6");
         var f2 = new ExplicitFormula("5*6");
         _object1 = new Parameter().WithFormula(f1);
         var parameter = new Parameter().WithFormula(f2);
         parameter.Value = 10;
         _object2 = parameter;
         _comparerSettings = new ComparerSettings {FormulaComparison = FormulaComparison.Formula};
      }

      [Observation]
      public void should_show_one_difference_for_the_parameter()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_a_parameters_with_one_parameter_having_an_rhs_formula_and_another_one_not : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("5*6");
         var f2 = new ExplicitFormula("5*6");
         _object1 = new Parameter {RHSFormula = f1};
         var parameter = new Parameter();
         _object2 = parameter;
      }

      [Observation]
      public void should_show_the_difference_in_the_state_variable_flag()
      {
         _report.Count.ShouldBeEqualTo(1);
         _report[0].DowncastTo<PropertyValueDiffItem>().PropertyName.ShouldBeEqualTo(Captions.Diff.IsStateVariable);
      }
   }

   public class When_comparing_a_constants_parameters_and_formula_comparison_is_formula_and_hidden_entity_should_not_be_compared : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ConstantFormula(5);
         var f2 = new ConstantFormula(10);
         _object1 = new Parameter().WithFormula(f1);
         var parameter = new Parameter().WithFormula(f2);
         _object2 = parameter;
         _comparerSettings.FormulaComparison = FormulaComparison.Formula;
         _comparerSettings.CompareHiddenEntities = false;
      }

      [Observation]
      public void should_show_one_difference_for_the_parameter()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_parameters_that_are_hidden_and_the_settings_does_not_allow_for_hidden_parameter_comparison : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         _object1 = new Parameter().WithName("P1");
         var parameter = new Parameter().WithName("P2");
         parameter.Visible = false;
         _object2 = parameter;
         _comparerSettings = new ComparerSettings {CompareHiddenEntities = false};
      }

      [Observation]
      public void should_not_report_any_difference()
      {
         _report.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_comparing_two_parameters_with_different_values_and_the_value_origin_difference_should_be_displayed : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var parameter1 = new Parameter().WithName("P1").WithValue(5);
         parameter1.ValueOrigin.Method = ValueOriginDeterminationMethods.Assumption;
         var parameter2 = new Parameter().WithName("P2").WithValue(6);
         parameter2.ValueOrigin.Method = ValueOriginDeterminationMethods.InVitro;

         _object1 = parameter1;
         _object2 = parameter2;
         _comparerSettings = new ComparerSettings {ShowValueOrigin = true};
      }

      [Observation]
      public void should_report_a_difference_in_value_and_value_origin()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }


   public class When_comparing_two_parameters_with_different_values_and_the_value_origin_difference_should_not_be_displayed : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var parameter1 = new Parameter().WithName("P1").WithValue(5);
         parameter1.ValueOrigin.Method = ValueOriginDeterminationMethods.Assumption;
         var parameter2 = new Parameter().WithName("P2").WithValue(6);
         parameter2.ValueOrigin.Method = ValueOriginDeterminationMethods.InVitro;

         _object1 = parameter1;
         _object2 = parameter2;
         _comparerSettings = new ComparerSettings { ShowValueOrigin = false };
      }

      [Observation]
      public void should_report_a_difference_in_value_only()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
};