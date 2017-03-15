using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converter.v5_2;

namespace OSPSuite.Converter.v5_2
{
   internal abstract class concern_for_FormulaMapper : ContextSpecification<IFormulaMapper>
   {
      protected override void Context()
      {
         sut = new FormulaMapper();
      }
   }

   internal class When_mapping_a_formula_that_did_not_need_conversion : concern_for_FormulaMapper
   {
      [Observation]
      public void should_return_an_empty_string()
      {
         sut.NewFormulaFor("AAAA").ShouldBeEmpty();
      }
   }

   internal class When_mapping_a_formula_that_did_need_conversion : concern_for_FormulaMapper
   {
      [Observation]
      public void should_return_the_new_string()
      {
         sut.NewFormulaFor("Dose / MW * 1000").ShouldBeEqualTo("Dose / MW");
      }
   }

   internal class When_checking_if_a_formula_was_converted:concern_for_FormulaMapper
   {
      [Observation]
      public void should_return_true_for_a_converted_string()
      {
         sut.FormulaWasConverted("Dose / MW").ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_for_an_unknown_value()
      {
         sut.FormulaWasConverted("AAAA").ShouldBeFalse();
      }
   }

   internal class When_mapping_a_formula_that_did_need_conversion_but_that_has_some_empty_spaces : concern_for_FormulaMapper
   {
      [Observation]
      public void should_return_the_new_string()
      {
         sut.NewFormulaFor("Dose/      MW*1000").ShouldBeEqualTo("Dose / MW");
      }
   }
}