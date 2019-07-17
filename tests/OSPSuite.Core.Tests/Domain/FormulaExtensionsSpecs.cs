using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_FormulaExtensions : StaticContextSpecification
   {
   }

   public class When_checking_if_a_formula_is_cachable : concern_for_FormulaExtensions
   {
      [Observation]
      public void should_return_true_for_an_explicit_formula()
      {
         new ExplicitFormula().IsCachable().ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_for_a_black_box_formula()
      {
         new BlackBoxFormula().IsCachable().ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_for_a_dynamic_formula()
      {
         new SumFormula().IsCachable().ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_for_a_table_formula()
      {
         new TableFormula().IsCachable().ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_for_a_table_with_offset_formula()
      {
         new TableFormulaWithOffset().IsCachable().ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_for_a_table_with_xargument_formula()
      {
         new TableFormulaWithXArgument().IsCachable().ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         new ConstantFormula().IsCachable().ShouldBeFalse();
      }
   }
}