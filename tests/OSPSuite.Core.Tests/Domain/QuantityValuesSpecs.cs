using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_QuantityValues : ContextSpecification<QuantityValues>
   {
      protected override void Context()
      {
         sut = new QuantityValues {Id = 4, ColumnId = "ColumnId", QuantityPath = "Path1|Path2", Values = new float[] {1, 2, 3}};
      }
   }

   public class when_creating_a_quantity_values : concern_for_QuantityValues
   {
      [Observation]
      public void should_be_able_to_return_the_quantity_path_as_array()
      {
         sut.PathList.ShouldOnlyContainInOrder("Path1", "Path2");
      }

      [Observation]
      public void should_have_save_the_values_in_bytes()
      {
         sut.Data.ShouldNotBeNull();
         sut.Data.ToFloatArray().ShouldOnlyContainInOrder(1f, 2f, 3f);
      }
   }

   public class When_resolving_the_values_by_index : concern_for_QuantityValues
   {
      [Observation]
      public void should_return_nan_if_the_values_is_out_of_bound()
      {
         sut[5].ShouldBeEqualTo(float.NaN);
         sut.ValueAt(5).ShouldBeEqualTo(float.NaN);
      }

      [Observation]
      public void should_return_the_value_at_the_given_index_otherwise()
      {
         sut[1].ShouldBeEqualTo(2);
         sut.ValueAt(2).ShouldBeEqualTo(3);
      }
   }
}