using OSPSuite.Core.Serialization;
using System.Xml.Linq;

namespace OSPSuite.Core.Converters.v13
{
   public class Converter121To130 : IObjectConverter
   {
      // 13.0 pkml is not compatible with 12.1, but you don't need an explicit conversion to move forward.
      // To satisfy the next converter, the object must pass through v13.0 conversion
      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V12_1;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         return (PKMLVersion.V13_0, false);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V13_0, false);
      }
   }
}
