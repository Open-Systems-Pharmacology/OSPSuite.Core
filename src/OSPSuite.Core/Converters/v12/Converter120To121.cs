using OSPSuite.Core.Serialization;
using System.Xml.Linq;

namespace OSPSuite.Core.Converters.v12
{
   public class Converter120To121 : IObjectConverter
   {
      // 12.1 pkml is not compatible with 12.0, but you don't need an explicit conversion to move forward.
      // To satisfy the next converter, the object must pass through v12.1 conversion
      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V12_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         return (PKMLVersion.V12_1, false);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V12_1, false);
      }
   }
}