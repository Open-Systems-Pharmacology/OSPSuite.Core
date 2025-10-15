using OSPSuite.Serializer.Attributes;
using System.Globalization;

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

         // We need to use the G15 format specifier to revert to the behavior of .NET Framework for compatibility
         // https://devblogs.microsoft.com/dotnet/floating-point-parsing-and-formatting-improvements-in-net-core-3-0/#potential-impact-to-existing-code
         return valueToConvert.ToString("G15", NumberFormatInfo.InvariantInfo);
      }
   }

   public class SimModelNullableDoubleAttributeMapper : NullableDoubleAttributeMapper<SimModelSerializationContext>
   {
      // We need to use the G15 format specifier to revert to the behavior of .NET Framework for compatibility
      // https://devblogs.microsoft.com/dotnet/floating-point-parsing-and-formatting-improvements-in-net-core-3-0/#potential-impact-to-existing-code
      protected override string ValueFor(double? valueToConvert, NumberFormatInfo numberFormatInfo) => valueToConvert.Value.ToString("G15", numberFormatInfo);
   }
}