using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core
{
   public class When_testing_if_a_quantity_values_is_null : StaticContextSpecification
   {
      [Observation]
      public void should_return_true_for_a_null_reference()
      {
         QuantityValues qv=null;
         qv.IsNull().ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_for_a_null_quantity_value()
      {
         QuantityValues qv = new NullQuantityValues();
         qv.IsNull().ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         QuantityValues qv = new QuantityValues();
         qv.IsNull().ShouldBeFalse();
      }
   }
}  