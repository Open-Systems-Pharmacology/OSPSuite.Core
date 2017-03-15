using System.Xml.Linq;
using OSPSuite.Utility;
using OSPSuite.Core.Serialization;

namespace OSPSuite.Core.Converter
{
   public interface IObjectConverter : ISpecification<int>
   {
      int Convert(object objectToUpdate);
      int ConvertXml(XElement element);
   }

   public class NullConverter : IObjectConverter
   {
      public int Convert(object objectToUpdate)
      {
         return PKMLVersion.Current;
      }

      public int ConvertXml(XElement element)
      {
         return PKMLVersion.Current;
      }

      public bool IsSatisfiedBy(int version)
      {
         return false;
      }
   }
}