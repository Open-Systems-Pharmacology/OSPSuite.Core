using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ExplicitFormulaExtensions : StaticContextSpecification
   {
      protected ExplicitFormula _formula;

      protected override void Context()
      {
         _formula = A.Fake<ExplicitFormula>();
      }
   }

   public class When_setting_the_formula_string_of_an_explicit_formula_with_the_extension : concern_for_ExplicitFormulaExtensions
   {
      private string _formulaString;

      protected override void Context()
      {
         base.Context();
         _formulaString = "tralala";

      }
      protected override void Because()
      {
         _formula.WithFormulaString(_formulaString);
      }
      [Observation]
      public void should_have_set_the_formula_string_of_the_explicit_formula()
      {
         _formula.FormulaString.ShouldBeEqualTo(_formulaString);
      }
   }
}	