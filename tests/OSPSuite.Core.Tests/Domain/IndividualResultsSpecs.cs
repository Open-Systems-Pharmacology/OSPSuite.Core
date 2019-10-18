using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_IndividualResults : ContextSpecification<IndividualResults>
   {
      protected override void Context()
      {
         var quantityValue1 = new QuantityValues {QuantityPath = "Path1"};
         var quantityValue2 = new QuantityValues {QuantityPath = "Path2"};

         sut = new IndividualResults();
         sut.Time = new QuantityValues {QuantityPath = "Time"};
         sut.Add(quantityValue1);
         sut.Add(quantityValue2);
      }
   }

   public class When_updating_the_time_reference_in_all_results : concern_for_IndividualResults
   {
      protected override void Because()
      {
         sut.UpdateQuantityTimeReference();
      }

      [Observation]
      public void should_have_set_a_reference_to_the_defined_time_in_all_values()
      {
         sut.Each(x => x.Time.ShouldBeEqualTo(sut.Time));
      }
   }

   public class When_checking_if_an_individual_results_are_results_for_a_given_path : concern_for_IndividualResults
   {
      [Observation]
      public void should_return_true_if_a_quantity_was_defined_with_the_path()
      {
         sut.HasValuesFor("Path1").ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.HasValuesFor("Path3").ShouldBeFalse();
      }
   }
}