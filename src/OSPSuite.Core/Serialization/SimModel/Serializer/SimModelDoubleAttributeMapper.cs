using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class SimModelDoubleAttributeMapper : DoubleAttributeMapper<SimModelSerializationContext>
   {
      public override string Convert(double valueToConvert, SimModelSerializationContext context)
      {
         if (double.IsPositiveInfinity(valueToConvert))
            return "INF";
         
         if (double.IsNegativeInfinity(valueToConvert))
            return "-INF";

         if (double.IsNaN(valueToConvert))
            return "NaN";

         return base.Convert(valueToConvert,context);
      }
   }
}