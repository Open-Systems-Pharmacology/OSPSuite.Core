using System;
using System.Globalization;
using Newtonsoft.Json;
using OSPSuite.Utility.Format;

namespace OSPSuite.Infrastructure.Serialization.Json
{
   public class NullableDoubleJsonConverter : JsonConverter
   {
      private const int DOUBLE_PRECISION = 10;

      private readonly NumericFormatter<double> _doubleFormatter = new NumericFormatter<double>(new NumericFormatterOptions
      {
         AllowsScientificNotation = true,
         DecimalPlace = DOUBLE_PRECISION
      });

      public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
      {
         var d = (double?)value;
         if (!d.HasValue)
            return;

         var formatted = _doubleFormatter.Format(d.Value);

         //the formatter always writes invariant notation. Parsing with the current culture would interpret
         //the decimal separator as a group separator e.g. in German, turning 1.0000000000 into 10000000000
         double.TryParse(formatted, NumberStyles.Float, CultureInfo.InvariantCulture, out double roundedFromString);

         writer.WriteValue(roundedFromString);
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer) =>
         //this will never be called for nullable as double reader will take precedence
         0;

      public override bool CanRead => false;

      public override bool CanConvert(Type objectType) => objectType == typeof(double?);
   }
}