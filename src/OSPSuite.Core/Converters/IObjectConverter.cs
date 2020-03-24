using System.Xml.Linq;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility;

namespace OSPSuite.Core.Converters
{
   public interface IObjectConverter : ISpecification<int>
   {
      (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate);
      (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element);
   }

   public class NullConverter : IObjectConverter
   {
      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         return (PKMLVersion.CURRENT, false);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.CURRENT, false);
      }

      public bool IsSatisfiedBy(int version)
      {
         return false;
      }
   }
}