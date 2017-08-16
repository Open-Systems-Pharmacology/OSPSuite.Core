using System.Xml.Linq;
using OSPSuite.Utility;
using OSPSuite.Core.Serialization;

namespace OSPSuite.Core.Converter
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
         return (PKMLVersion.Current, false);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.Current, false);
      }

      public bool IsSatisfiedBy(int version)
      {
         return false;
      }
   }
}