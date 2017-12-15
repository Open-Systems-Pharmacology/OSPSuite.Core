using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ValueOriginXmlSerializer : OSPSuiteXmlSerializer<ValueOrigin>
   {
      public override void PerformMapping()
      {
         Map(x => x.Type);
         Map(x => x.Description);
      }
   }
}