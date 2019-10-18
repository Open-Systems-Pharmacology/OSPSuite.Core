using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_using_formula_extensions : StaticContextSpecification
   {
      protected override void Context()
      {
      }
   }

   
   public class When_setting_the_formula_of_a_using_formula_with_the_extensions : concern_for_using_formula_extensions
   {
      private IUsingFormula _myUsingFormula;
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _myUsingFormula = A.Fake<IUsingFormula>();
         _formula =A.Fake<IFormula>();
      }
      protected override void Because()
      {
         _myUsingFormula.WithFormula(_formula);
      }
      [Observation]
      public void should_have_set_the_formula_of_the_object()
      {
         _myUsingFormula.Formula.ShouldBeEqualTo(_formula);
      }
   }
}	