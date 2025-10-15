using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization.SimModel;
using OSPSuite.Core.Serialization.SimModel.Serializer;

namespace OSPSuite.Core.Mappers
{
   public class concern_for_SimModelNullableDoubleAttributeMapper : ContextSpecification<SimModelNullableDoubleAttributeMapper>
   {
      protected SimModelSerializationContext _context;

      protected override void Context()
      {
         sut = new SimModelNullableDoubleAttributeMapper();
         _context = new SimModelSerializationContext();
      }
      
   }

   public class When_converting_nullable_numbers_with_too_much_precision : concern_for_SimModelNullableDoubleAttributeMapper
   {
      [Observation]
      public void the_precision_should_be_reduced()
      {
         sut.Convert(0.11704000000000003, _context).ShouldBeEqualTo("0.11704");
      }
   }
}
