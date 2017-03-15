using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization
{
   public static class XElementExtensions
   {
      public static int GetPKMLVersion(this XElement element)
      {
         string versionString = element.GetAttribute(Constants.Serialization.Attribute.VERSION);
         if (string.IsNullOrEmpty(versionString))
            return PKMLVersion.NON_CONVERTABLE_VERSION;

         return versionString.ConvertedTo<int>();
      }
   }
}