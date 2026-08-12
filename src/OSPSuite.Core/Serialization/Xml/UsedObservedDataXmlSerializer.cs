using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class UsedObservedDataXmlSerializer : OSPSuiteXmlSerializer<UsedObservedData>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
      }
   }
}
