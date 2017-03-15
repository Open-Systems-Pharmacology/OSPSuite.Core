using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimModelDoubleAttributeMapper : ContextSpecification<SimModelDoubleAttributeMapper>
   {
      protected SimModelSerializationContext _context;

      protected override void Context()
      {
         sut = new SimModelDoubleAttributeMapper();
         _context = new SimModelSerializationContext();
      }
   }

   
   public class When_converting : concern_for_SimModelDoubleAttributeMapper
   {
      [Observation]
      public void a_double_with_a_value_of_infinity_should_return_INF()
      {
         sut.Convert(double.PositiveInfinity, _context).ShouldBeEqualTo("INF");
      }

      [Observation]
      public void a_double_with_a_value_of_negative_infinity_should_return_minus_INF()
      {
         sut.Convert(double.NegativeInfinity, _context).ShouldBeEqualTo("-INF");
      }

      [Observation]
      public void a_double_with_a_value_of_not_a_number_should_return_NAN()
      {
         sut.Convert(double.NaN, _context).ShouldBeEqualTo("NaN");
      }


      [Observation]
      public void a_valid_double_value()
      {
         sut.Convert(1.546, _context).ShouldBeEqualTo("1.546");
      }
   }
}