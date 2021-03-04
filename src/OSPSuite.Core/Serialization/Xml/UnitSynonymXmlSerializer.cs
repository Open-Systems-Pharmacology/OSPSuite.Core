using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Core.Serialization.Xml
{
   public class UnitSynonymXmlSerializer : XmlSerializer<UnitSynonym, SerializationContext>, IUnitSystemXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(x => x.Name);
      }
   }
}