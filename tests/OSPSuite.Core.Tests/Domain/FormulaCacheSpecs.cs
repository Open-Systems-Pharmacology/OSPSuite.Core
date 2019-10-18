using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_FormulaCache : ContextSpecification<IFormulaCache>
   {
      protected override void Context()
      {
         sut = new FormulaCache();
      }
   }

   public class When_checking_if_a_formula_cache_contains_a_formula : concern_for_FormulaCache
   {
      private IFormula _existingFormula;
      private IFormula _anotherFormulaWithSameId;
      private IFormula _aDifferentFormula;

      protected override void Context()
      {
         base.Context();
         _existingFormula = A.Fake<IFormula>().WithId("existingId");
         _anotherFormulaWithSameId = A.Fake<IFormula>().WithId(_existingFormula.Id);
         _aDifferentFormula = A.Fake<IFormula>().WithId("toto");
         sut.Add(_existingFormula);
      }

      [Observation]
      public void should_return_false_if_the_formula_is_null()
      {
         sut.Contains(null).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_cache_does_not_contain_any_formula_having_the_same_id_as_the_given_formula()
      {
         sut.Contains(_aDifferentFormula).ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_if_the_cache_contains_a_formula_having_the_same_id_as_the_given_formula()
      {
         sut.Contains(_anotherFormulaWithSameId).ShouldBeTrue();
      }
   }

   public class When_refreshing_the_cache : concern_for_FormulaCache
   {
      private IFormula _existingFormula;

      protected override void Context()
      {
         base.Context();
         _existingFormula = A.Fake<IFormula>().WithId("TATA");
         sut.Add(_existingFormula);
         _existingFormula.Id = "TOTO";
      }

      [Observation]
      public void should_have_updated_the_id()
      {
         sut.Contains(_existingFormula).ShouldBeFalse();
         sut.Refresh();
         sut.Contains(_existingFormula).ShouldBeTrue();
      }
   }
}