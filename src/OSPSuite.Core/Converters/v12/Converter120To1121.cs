using System.Xml.Linq;
using OSPSuite.Core.Serialization;

namespace OSPSuite.Core.Converters.v12
{
   public class Converter120To1121 : IObjectConverter
   {
      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V12_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         return (PKMLVersion.V12_1, true);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V12_1, true);
      }
   }
}