using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OSPSuite.Infrastructure.Serialization.Json
{
   public class OSPSuiteJsonSerializerSettings : JsonSerializerSettings
   {
      public OSPSuiteJsonSerializerSettings()
      {
         NullValueHandling = NullValueHandling.Ignore;
         ContractResolver = new WritablePropertiesOnlyResolver();
         Converters.Add(new StringEnumConverter());
         Converters.Add(new NullableDoubleJsonConverter());
         Converters.Add(new ColorConverter());
      }
   }
}