using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class QuantitySelectionXmlSerializer : OSPSuiteXmlSerializer<QuantitySelection>
   {
      public override void PerformMapping()
      {
         Map(x => x.Path);
         Map(x => x.QuantityType);
      }
   }
}