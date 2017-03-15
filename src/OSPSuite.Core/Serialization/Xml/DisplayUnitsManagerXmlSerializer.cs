using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DisplayUnitsManagerXmlSerializer : OSPSuiteXmlSerializer<DisplayUnitsManager>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.AllDisplayUnits, x => x.AddDisplayUnit);
      }
   }

   public class DisplayUnitMapXmlSerializer : OSPSuiteXmlSerializer<DisplayUnitMap>
   {
      public override void PerformMapping()
      {
         Map(x => x.Dimension);
      }

      protected override void TypedDeserialize(DisplayUnitMap displayUnitMap, XElement element, SerializationContext serializationContext)
      {
         base.TypedDeserialize(displayUnitMap, element, serializationContext);
         element.UpdateDisplayUnit(displayUnitMap);
      }

      protected override XElement TypedSerialize(DisplayUnitMap displayUnitMap, SerializationContext serializationContext)
      {
         var element = base.TypedSerialize(displayUnitMap, serializationContext);
         return element.AddDisplayUnitFor(displayUnitMap);
      }
   }
}