using OSPSuite.Core.Serialization;
using System.Xml.Linq;

namespace OSPSuite.Core.Converters.v13
{
   public class Converter121To130 : IObjectConverter
   {
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
