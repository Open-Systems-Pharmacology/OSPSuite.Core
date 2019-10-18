using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ComparerSettings : ContextSpecification<ComparerSettings>
   {
      protected override void Context()
      {
         sut = new ComparerSettings();
      }
   }

   public class When_creating_a_default_set_of_settings_for_the_comparison : concern_for_ComparerSettings
   {
      [Observation]
      public void the_default_relative_tolerance_should_be_equal_to_the_predefined_epsilon()
      {
         sut.RelativeTolerance.ShouldBeEqualTo(Constants.DOUBLE_RELATIVE_EPSILON);
      }

      [Observation]
      public void should_only_compute_comparison_relevant_for_simulations()
      {
         sut.OnlyComputingRelevant.ShouldBeTrue();
      }

      [Observation]
      public void should_compare_quantity_by_formula()
      {
         sut.FormulaComparison.ShouldBeEqualTo(FormulaComparison.Formula);
      }

      [Observation]
      public void should_not_compare_hidden_entities()
      {
         sut.CompareHiddenEntities.ShouldBeFalse();
      }

      [Observation]
      public void should_show_value_origin_for_compared_entities()
      {
         sut.ShowValueOrigin.ShouldBeTrue();
      }
   }
}	