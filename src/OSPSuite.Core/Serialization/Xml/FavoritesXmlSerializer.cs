using OSPSuite.Serializer;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class FavoritesXmlSerializer : OSPSuiteXmlSerializer<Favorites>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x, x => x.Add).WithMappingName(Constants.Serialization.ALL).WithChildMappingName(Constants.Serialization.FAVORITE);
      }
   }
}