using System;
using System.Globalization;
using OSPSuite.Serializer.Attributes;

namespace OSPSuite.Core.Serialization.Xml
{
   public class TimeSpanAttributeMapper : AttributeMapper<TimeSpan, SerializationContext>
   {
      public override object ConvertFrom(string attributeValue, SerializationContext context)
      {
         long ticks;
         if (long.TryParse(attributeValue, out ticks))
            return TimeSpan.FromTicks(ticks);

         return new TimeSpan();
      }

      public override string Convert(TimeSpan dateValue, SerializationContext context)
      {
         return dateValue.Ticks.ToString(CultureInfo.InvariantCulture);
      }
   }
}